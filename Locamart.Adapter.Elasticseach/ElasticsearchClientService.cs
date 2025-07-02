using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Locamart.Adapter.Elasticsearch.Models;
using Locamart.Application.Contracts.Dtos;
using Locamart.Application.Contracts.Services;
using Locamart.Shared;
using System.Text.Json;
using Result = CSharpFunctionalExtensions.Result;

namespace Locamart.Adapter.Elasticsearch;

public class ElasticsearchClientService(ElasticsearchClient client) : ISearchService
{
    public async Task<Result<IReadOnlyCollection<ProductDto>, Error>> GetNearbyProducts(
        string query, long distance, double lat, double lon)
    {
        try
        {
            var response = await client.SearchAsync<ProductSearch>(s => s
                .Indices("products")
                .Source(new SourceFilter
                {
                    Includes = new[] { "productId", "storeId", "storeName", "productName", "description", "storeLocation" }
                })
                .Query(q => q
                    .Bool(b => b
                        .Must(m => m
                            .Match(mq => mq
                                .Field(f => f.description)
                                .Query(query)
                            )
                        )
                        .Filter(f => f
                            .GeoDistance(g => g
                                .Field(f => f.storeLocation)
                                .Distance($"{distance}km")
                                .Location(new LatLonGeoLocation { Lat = lat, Lon = lon })
                            )
                        )
                    )
                )
                .Sort(s => s
                    .Field(f => f
                        .Field("_score")
                        .Order(SortOrder.Desc)
                    )
                    .GeoDistance(g => g
                        .Field(f => f.storeLocation)
                        .Location(new LatLonGeoLocation { Lat = lat, Lon = lon })
                        .Order(SortOrder.Asc)
                        .Unit(DistanceUnit.Kilometers)
                    )
                )
                .ScriptFields(sf => sf
                    .Add("distance", new ScriptField
                    {
                        Script = new Script
                        {
                            Source = "doc['storeLocation'].arcDistance(params.lat, params.lon) / 1000",
                            Params = new Dictionary<string, object>
                                {
                                    { "lat", lat },
                                    { "lon", lon }
                                }
                        }
                    }
                    )
                )
            );

            if (!response.IsValidResponse)
            {
                return Result.Failure<IReadOnlyCollection<ProductDto>, Error>(new Error("Err",
                    response.DebugInformation));
            }

            var products = response.Hits.Select(hit =>
            {
                Guid.TryParse(hit.Source.productId, out var productId);
                Guid.TryParse(hit.Source.storeId, out var storeId);
                double firstDistance = 0;
                if (TryGetTypedList<double>(hit.Fields!, "distance", out var distances))
                {
                    firstDistance = distances.FirstOrDefault();
                }


                return new ProductDto
                {
                    ProductId = productId,
                    StoreId = storeId,
                    StoreName = hit.Source.storeName,
                    ProductName = hit.Source.productName,
                    DistanceInKilometers = firstDistance
                };
            }).ToList();

            return Result.Success<IReadOnlyCollection<ProductDto>, Error>(products);
        }

        catch (Exception ex)
        {
            return Error.Create("get_nearby_products_error", ex.Message);
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

            case JsonElement jsonElement when jsonElement.ValueKind == JsonValueKind.Array:
                try
                {
                    // Works for simple value types like double, int, etc.
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

