using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Analysis;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;

namespace Locamart.Adapter.Elasticsearch;

public class IndexInitialization
{
    public static async Task EnsureProductIndexExistsAsync(ElasticsearchClient client)
    {
        var exists = await client.Indices.ExistsAsync("products");
        if (exists.Exists) await client.Indices.DeleteAsync("products");

        var create = await client.Indices.CreateAsync("products", c => c
            .Settings(s => s
                .NumberOfShards(1)
                .NumberOfReplicas(0)
                .Analysis(a => a
                    .TokenFilters(tf => tf
                        .EdgeNGram("fa_edge_ngrams", e => e
                            .MinGram(2)
                            .MaxGram(12)
                            .Side(EdgeNGramSide.Front)))
                    .Analyzers(an => an
                        .Custom("fa_base", ca => ca
                            .Tokenizer("standard")
                            .Filter("lowercase", "arabic_normalization", "persian_normalization", "decimal_digit"))
                        .Custom("fa_ngram", ca => ca
                            .Tokenizer("standard")
                            .Filter("lowercase", "arabic_normalization", "persian_normalization", "decimal_digit", "fa_edge_ngrams")))
                )
            )
            .Mappings(m => m.Properties(p => p
                .Keyword("productId")
                .Keyword("storeId")
                .Keyword("storeName")
                .Keyword("storeIdentifier")
                .DoubleNumber("price")
                .Text("description", t => t.Analyzer("fa_base").SearchAnalyzer("fa_base"))
                .Text("productName", t => t
                    .Analyzer("fa_base")
                    .SearchAnalyzer("fa_base")
                    .Fields(ff => ff
                        .Text("ngram", nt => nt.Analyzer("fa_ngram").SearchAnalyzer("fa_base"))
                        .Keyword("raw")))
                .GeoPoint("storeLocation")
            ))
        );

        if (!create.IsValidResponse)
            throw new Exception(create.DebugInformation);
    }
}

