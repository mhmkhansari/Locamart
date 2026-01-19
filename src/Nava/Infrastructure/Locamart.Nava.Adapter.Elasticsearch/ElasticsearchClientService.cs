using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Locamart.Dina;
using Locamart.Nava.Adapter.Elasticsearch.Models;
using Locamart.Nava.Application.Contracts.Dtos.Search;
using Locamart.Nava.Application.Contracts.IntegrationEvents;
using Locamart.Nava.Application.Contracts.Services;
using System.Text.Json;
using Result = CSharpFunctionalExtensions.Result;

namespace Locamart.Nava.Adapter.Elasticsearch;

public class ElasticsearchClientService(ElasticsearchClient client) : ISearchService
{
    public async Task<Result<IReadOnlyCollection<ProductSearchDto>, Error>> GetNearbyProducts(
        string query, long distance, double lat, double lon)
    {
        try
        {
            var response = await client.SearchAsync<ProductModel>(s => s
                .Indices("products")
                .Source(new SourceFilter
                {
                    Includes = new[] { "productId", "productName", "Description" }
                })
                .Query(q => q
                    .Bool(b => b
                        .Must(m => m
                            .Match(mq => mq
                                .Field(f => f.ProductName)
                                .Query(query)
                            )
                        )
                        .Filter(f => f
                            .Nested(n => n
                                .Path(p => p.StoreInventory)
                                .Query(nq => nq
                                    .Bool(nb => nb
                                        .Filter(nf => nf
                                            .Range(fx => fx
                                                .Number(fa => fa
                                                    .Field("storeInventory.atp")
                                                    .Gt(0)
                                                )
                                            )
                                            .GeoDistance(g => g
                                                .Field("storeInventory.storeLocation")
                                                .Distance($"{distance}m")
                                                .Location(new LatLonGeoLocation { Lat = lat, Lon = lon })
                                            )

                                        )
                                    )

                                )
                                .InnerHits(ih => ih
                                    .Size(5)
                                    .Sort(s => s
                                        .GeoDistance(g => g
                                            .Field("storeInventory.storeLocation")
                                            .Location(new LatLonGeoLocation { Lat = lat, Lon = lon })
                                            .Order(SortOrder.Asc)
                                            .Unit(DistanceUnit.Kilometers)
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
                .Sort(s => s
                    .Field(f => f
                        .Field("_score")
                        .Order(SortOrder.Desc)
                    )
                )

                .Suggest(s => s
                    .Suggesters(d => d
                        .Add("did_you_mean", x => x
                            .Term(h => h
                                .Field(f => f.ProductName)
                                .Text(query)
                                .Size(1)
                            )
                        )
                    )
                )
            );

            if (!response.IsValidResponse)
            {
                return Result.Failure<IReadOnlyCollection<ProductSearchDto>, Error>(Error.Create("Error in elasticsearch query",
                    response.DebugInformation));
            }

            var products = response.Hits.Select(hit =>
            {
                Guid.TryParse(hit.Source!.ProductId, out var productId);
                double firstDistance = 0;
                if (TryGetTypedList<double>(hit.Fields!, "distance", out var distances))
                {
                    firstDistance = Math.Round(distances.FirstOrDefault(), 1);
                }

                return new ProductSearchDto
                {
                    ProductId = productId,
                    ProductName = hit.Source.ProductName,
                    Description = hit.Source.Description,
                    StoreInventories = hit.Source.StoreInventory.Select(x => new InventorySearchDto()
                    {
                        Atp = x.AvailableQuantity - x.ReservedQuantity,
                        StoreId = x.StoreId,
                        StoreName = x.StoreName,
                        StoreIdentifier = x.StoreIdentifier,
                        DistanceInKilometers = 0

                    }).ToList()
                };
            }).ToList();

            return Result.Success<IReadOnlyCollection<ProductSearchDto>, Error>(products);
        }

        catch (Exception ex)
        {
            return Error.Create("get_nearby_products_error", ex.Message);
        }

    }

    public async Task<UnitResult<Error>> IndexProduct(ProductCreatedIntegrationEvent integrationEvent)
    {
        var productDocument = new ProductModel()
        {
            ProductId = integrationEvent.Id.ToString(),
            ProductName = integrationEvent.ProductName,
            Description = integrationEvent.Description,
            Images = integrationEvent.Images
            storeLocation =
                GeoLocation.LatitudeLongitude(new LatLonGeoLocation(integrationEvent.StoreLatitude,
                    integrationEvent.StoreLongitude))
        };

        var result = await client.IndexAsync(productDocument, i => i
            .Index("products")
            .Id(productDocument.ProductId)
            .Refresh(Refresh.WaitFor));

        if (!result.IsSuccess() && result.ElasticsearchServerError is not null)
            return Error.Create("product_index_error", result.ElasticsearchServerError.Error.ToString());

        return UnitResult.Success<Error>();
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

