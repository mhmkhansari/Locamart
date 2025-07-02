using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch;
using Locamart.Adapter.Elasticsearch.Models;
using Locamart.Application.Contracts.Dtos;
using Locamart.Application.Contracts.Services;
using Locamart.Shared;
using Result = CSharpFunctionalExtensions.Result;

namespace Locamart.Adapter.Elasticsearch;

public class ElasticsearchClientService(ElasticsearchClient client) : ISearchService
{
    public async Task<Result<IReadOnlyCollection<ProductDto>, Error>> GetNearbyProducts(
            string query, long distance, double lat, double lon)
    {
        var response = await client.SearchAsync<ProductSearch>(s => s
            .Query(q => q
                .Bool(b => b
                    .Must(m => m
                        .Match(mq => mq
                            .Field(f => f.Description)
                            .Query(query)
                        )
                    )
                    .Filter(f => f
                        .GeoDistance(g => g
                            .Field(f => f.StoreLocation)
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
                    .Field(f => f.StoreLocation)
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
            return Result.Failure<IReadOnlyCollection<ProductDto>, Error>(new Error("Err", response.DebugInformation));
        }

        var products = response.Hits.Select(hit =>
        {
            Guid.TryParse(hit.Source.ProductId, out var productId);
            Guid.TryParse(hit.Source.StoreId, out var storeId);

            var distanceInKm = (double)hit.Fields["distance"];

            return new ProductDto
            {
                ProductId = productId,
                StoreId = storeId,
                StoreName = hit.Source.StoreName,
                ProductName = hit.Source.ProductName,
                DistanceInKilometers = distanceInKm
            };
        }).ToList();

        return Result.Success<IReadOnlyCollection<ProductDto>, Error>(products);
    }
}

