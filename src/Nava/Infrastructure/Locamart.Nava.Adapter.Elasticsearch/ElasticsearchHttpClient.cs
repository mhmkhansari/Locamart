using System.Text;
using System.Text.Json;

namespace Locamart.Nava.Adapter.Elasticsearch;

public sealed class ElasticsearchHttpClient(HttpClient httpClient)
{
    public async Task<bool> IndexExistsAsync(string indexName)
    {
        var response = await httpClient.SendAsync(
            new HttpRequestMessage(HttpMethod.Head, $"/{indexName}")
        );

        return response.IsSuccessStatusCode;
    }

    public async Task CreateIndexAsync(string indexName, object indexDefinition)
    {
        var json = JsonSerializer.Serialize(indexDefinition);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PutAsync($"/{indexName}", content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create index {indexName}: {error}");
        }
    }

    /// <summary>
    /// Index a document to Elasticsearch
    /// </summary>
    /// <param name="indexName">Target index</param>
    /// <param name="document">The document object to index</param>
    /// <param name="id">Optional document ID</param>
    /// <param name="refresh">Whether to wait for refresh</param>
    public async Task<HttpResponseMessage?> IndexAsync<T>(
        string indexName,
        T document,
        string? id = null,
        bool refresh = false)
    {
        var json = JsonSerializer.Serialize(document, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var url = id is null ? $"/{indexName}/_doc" : $"/{indexName}/_doc/{id}";

        if (refresh)
        {
            url += "?refresh=wait_for";
        }

        var response = await httpClient.PutAsync(url, content);

        return response;

    }

    /// <summary>
    /// Generic POST request for Elasticsearch REST API
    /// </summary>
    /// <param name="url">Relative Elasticsearch endpoint (e.g., "/products/_search")</param>
    /// <param name="payload">The object to serialize as JSON body</param>
    /// <returns>HttpResponseMessage from Elasticsearch</returns>
    public async Task<HttpResponseMessage> PostAsync(string url, object payload)
    {
        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Elasticsearch POST request failed for '{url}': {error}");
        }

        return response;
    }
}

