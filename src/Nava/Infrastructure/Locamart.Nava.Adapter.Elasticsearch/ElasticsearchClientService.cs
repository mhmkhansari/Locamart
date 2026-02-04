using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Nava.Adapter.Elasticsearch.Models;
using Locamart.Nava.Application.Contracts.Dtos.Search;
using Locamart.Nava.Application.Contracts.IntegrationEvents;
using Locamart.Nava.Application.Contracts.Services;
using System.Text;
using System.Text.Json;
using Result = CSharpFunctionalExtensions.Result;

namespace Locamart.Nava.Adapter.Elasticsearch;

public class ElasticsearchClientService(ElasticsearchHttpClient client) : ISearchService
{
    public async Task<Result<IReadOnlyCollection<ProductSearchDto>, Error>> GetNearbyProducts(
     string query, long distance, double lat, double lon)
    {
        try
        {
            // Build Elasticsearch query as anonymous object
            var esQuery = new
            {
                _source = new[] { "productId", "productName", "description", "storeInventory" },
                query = new
                {
                    @bool = new
                    {
                        must = new[]
                        {
                        new
                        {
                            match = new
                            {
                                productName = new { query }
                            }
                        }
                    },
                        filter = new[]
                        {
                        new
                        {
                            nested = new
                            {
                                path = "storeInventory",
                                query = new
                                {
                                    @bool = new
                                    {
                                        filter = new object[]
                                        {
                                            new
                                            {
                                                range = new Dictionary<string, object>
                                                {
                                                    ["storeInventory.atp"] = new Dictionary<string, object>
                                                    {
                                                        ["gt"] = 0
                                                    }
                                                }
                                            },
                                            new
                                            {
                                                geo_distance = new Dictionary<string, object>
                                                {
                                                    ["distance"] = $"{distance}m",
                                                    ["storeInventory.storeLocation"] = new { lat, lon }
                                                }
                                            }
                                        }
                                    }
                                },
                                inner_hits = new
                                {
                                    size = 5,
                                    sort = new[]
                                    {
                                        new
                                        {
                                            _geo_distance = new Dictionary<string, object>
                                            {
                                                ["storeInventory.storeLocation"] = new { lat, lon },
                                                ["order"] = "asc",
                                                ["unit"] = "km"
                                        }
        }
                                    }
                                }
                            }
                        }
                    }
                    }
                },
                sort = new object[]
                {
                new { _score = new { order = "desc" } }
                },
                script_fields = new
                {
                    distance = new
                    {
                        script = new
                        {
                            source = "doc['storeInventory.storeLocation'].arcDistance(params.lat, params.lon) / 1000",
                            @params = new { lat, lon }
                        }
                    }
                },
                suggest = new
                {
                    did_you_mean = new
                    {
                        term = new
                        {
                            field = "productName",
                            text = query,
                            size = 1
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(esQuery, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/products/_search", content);

            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                return Result.Failure<IReadOnlyCollection<ProductSearchDto>, Error>(
                    Error.Create("elasticsearch_query_error", err));
            }

            var responseString = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseString);
            var hits = doc.RootElement.GetProperty("hits").GetProperty("hits");

            var products = new List<ProductSearchDto>();

            foreach (var hit in hits.EnumerateArray())
            {
                var source = hit.GetProperty("_source");

                var storeInventory = new List<InventorySearchDto>();
                if (source.TryGetProperty("storeInventory", out var inventories))
                {
                    foreach (var inv in inventories.EnumerateArray())
                    {
                        int atp = 0;
                        if (inv.TryGetProperty("availableQuantity", out var available) &&
                            inv.TryGetProperty("reservedQuantity", out var reserved))
                        {
                            atp = available.GetInt32() - reserved.GetInt32();
                        }

                        storeInventory.Add(new InventorySearchDto
                        {
                            StoreId = inv.GetProperty("storeId").GetString() ?? "",
                            StoreName = inv.GetProperty("storeName").GetString() ?? "",
                            StoreIdentifier = inv.GetProperty("storeIdentifier").GetString(),
                            Atp = atp,
                            DistanceInKilometers = 0
                        });
                    }
                }

                products.Add(new ProductSearchDto
                {
                    ProductId = Guid.Parse(source.GetProperty("productId").GetString()!),
                    ProductName = source.GetProperty("productName").GetString()!,
                    Description = source.GetProperty("description").GetString() ?? "",
                    StoreInventories = storeInventory
                });
            }

            return Result.Success<IReadOnlyCollection<ProductSearchDto>, Error>(products);
        }
        catch (HttpRequestException httpEx)
        {
            return Result.Failure<IReadOnlyCollection<ProductSearchDto>, Error>(
                Error.Create("http_request_error", httpEx.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure<IReadOnlyCollection<ProductSearchDto>, Error>(
                Error.Create("unexpected_error", ex.Message));
        }
    }

    public async Task<UnitResult<Error>> IndexProduct(ProductCreatedIntegrationEvent integrationEvent)
    {
        try
        {
            var productDocument = new ProductModel()
            {
                Id = integrationEvent.Id.ToString(),
                Name = integrationEvent.Name,
                Description = integrationEvent.Description,
                Images = integrationEvent.Images.Select(x => new ProductImageModel()
                {
                    Url = x.Url,
                    Order = x.Order,
                }).ToList(),
                CreatedBy = integrationEvent.CreatedBy.ToString(),
                IsDeleted = integrationEvent.IsDeleted,
                StoreInventory = new List<InventoryModel>()
            };

            var response = await client.IndexAsync("products", productDocument, productDocument.Id, refresh: true);

            if (response == null)
            {
                return Error.Create("product_index_error", "No response received from Elasticsearch");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Error.Create("product_index_error", errorContent);
            }

            return UnitResult.Success<Error>();
        }
        catch (HttpRequestException httpEx)
        {
            return Error.Create("product_index_http_error", httpEx.Message);
        }
        catch (Exception ex)
        {
            return Error.Create("product_index_unexpected_error", ex.Message);
        }
    }

    public static bool TryGetTypedList<T>(IReadOnlyDictionary<string, object> dict, string key, out List<T> result)
    {
        result = new List<T>();

        if (!dict.TryGetValue(key, out var value) || value == null)
            return false;

        switch (value)
        {
            case T[] array:
                result = array.ToList();
                return true;

            case List<T> list:
                result = list;
                return true;

            case IEnumerable<T> enumerable:
                result = enumerable.ToList();
                return true;

            case JsonElement { ValueKind: JsonValueKind.Array } jsonElement:
                try
                {
                    result = jsonElement.EnumerateArray()
                        .Select(e => (T)Convert.ChangeType(e.GetDouble(), typeof(T)))
                        .ToList();
                    return true;
                }
                catch
                {
                    return false;
                }

            default:
                return false;
        }
    }
}

