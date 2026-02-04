namespace Locamart.Nava.Adapter.Elasticsearch.Definitions;

public static class ProductIndexDefinition
{
    public static object Build() => new
    {
        settings = new
        {
            number_of_shards = 3,
            number_of_replicas = 1,
            analysis = new
            {
                filter = new
                {
                    fa_edge_ngrams = new
                    {
                        type = "edge_ngram",
                        min_gram = 2,
                        max_gram = 12
                    }
                },
                analyzer = new
                {
                    fa_base = new
                    {
                        tokenizer = "standard",
                        filter = new[]
                        {
                            "lowercase",
                            "arabic_normalization",
                            "persian_normalization",
                            "decimal_digit"
                        }
                    },
                    fa_ngram = new
                    {
                        tokenizer = "standard",
                        filter = new[]
                        {
                            "lowercase",
                            "arabic_normalization",
                            "persian_normalization",
                            "decimal_digit",
                            "fa_edge_ngrams"
                        }
                    }
                }
            }
        },
        mappings = new
        {
            properties = new
            {
                id = new { type = "keyword" },

                name = new
                {
                    type = "text",
                    analyzer = "fa_base",
                    search_analyzer = "fa_base",
                    fields = new
                    {
                        ngram = new { type = "text", analyzer = "fa_ngram" },
                        raw = new { type = "keyword" }
                    }
                },

                description = new
                {
                    type = "text",
                    analyzer = "fa_base"
                },

                images = new
                {
                    type = "object",
                    properties = new
                    {
                        url = new { type = "keyword" },
                        isPrimary = new { type = "boolean" },
                        order = new { type = "integer" }
                    }
                },

                createdAt = new
                {
                    type = "date"
                },

                createdBy = new
                {
                    type = "keyword"
                },

                updatedAt = new
                {
                    type = "date"
                },

                updatedBy = new
                {
                    type = "keyword"
                },

                isDeleted = new
                {
                    type = "boolean"
                },
                storeInventory = new
                {
                    type = "nested",
                    properties = new
                    {
                        storeId = new { type = "keyword" },
                        storeName = new { type = "keyword" },
                        storeIdentifier = new { type = "keyword" },
                        price = new { type = "double" },
                        availableQuantity = new { type = "integer" },
                        reservedQuantity = new { type = "integer" },
                        storeLocation = new { type = "geo_point" }
                    }
                }
            }
        }
    };
}