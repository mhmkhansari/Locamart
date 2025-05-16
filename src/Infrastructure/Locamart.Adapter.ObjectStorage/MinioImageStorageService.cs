using CSharpFunctionalExtensions;
using Locamart.Application.Contracts.Services;
using Locamart.Shared;
using Microsoft.Extensions.Options;
using Minio.DataModel.Args;
using Minio;

namespace Locamart.Adapter.ObjectStorage;

public class MinioImageStorageService(IMinioClient minioClient, IOptionsSnapshot<ObjectStorageOptions> options)
    : IImageStorageService
{
    private readonly string _bucketName = "product-images";
    private readonly string _baseUrl = "localhost";

    public async Task<Result<string,Error>> UploadImageAsync(Stream imageStream, string fileName, string contentType)
    {
        var result = await minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileName)
            .WithStreamData(imageStream)
            .WithObjectSize(imageStream.Length)
            .WithContentType(contentType));


        if (result is null)
            return Error.Create("object_storage_no_response", "No response from object storage");

        return $"{_baseUrl}/{_bucketName}/{fileName}";
    }
}
