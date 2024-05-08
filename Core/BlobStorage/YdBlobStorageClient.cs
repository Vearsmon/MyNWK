using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Core.Helpers;
using JetBrains.Annotations;

namespace Core.BlobStorage;

[UsedImplicitly]
public class YdBlobStorageClient : IBlobStorageClient
{
    private const int BufferSize = 32768;
    private readonly AmazonS3Client amazonS3Client;

    public YdBlobStorageClient()
    {
        var config = new AmazonS3Config 
        {
            ServiceURL = "https://s3.yandexcloud.net"
        };

        amazonS3Client = new AmazonS3Client(config);
    }

    public async Task<Guid> PutAsync(
        string container,
        byte[] blob,
        CancellationToken cancellationToken = default)
    {
        if (blob.IsNullOrEmpty())
        {
            throw new ArgumentException("Should not be null", nameof(blob));
        }
        
        var key = Guid.NewGuid();
        var request = new PutObjectRequest
        {
            BucketName = container,
            Key = key.ToString(),
            InputStream = new MemoryStream(blob)
        };

        var response = await amazonS3Client
            .PutObjectAsync(request ,cancellationToken)
            .ConfigureAwait(false);

        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new BlobStorageException(
                $"Could not put object. " +
                $"Container: {container}; StatusCode: {response.HttpStatusCode}");
        }

        return key;
    }

    public async Task<byte[]> GetAsync(
        string container,
        Guid key,
        CancellationToken cancellationToken = default)
    {
        using var response = await amazonS3Client
            .GetObjectAsync(container, key.ToString(), cancellationToken)
            .ConfigureAwait(false);
        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new BlobStorageException(
                $"Could not read object." +
                $"Container: {container}; Key: {key}; Status code: {response.HttpStatusCode}");
        }

        await using var stream = response.ResponseStream;
        return await stream
            .ReadToEndAsync(BufferSize, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<string> GetDownloadingUrlAsync(
        string container,
        Guid key)
    {
        var request = new GetPreSignedUrlRequest()
        {
            BucketName = container,
            Key = key.ToString(),
            Expires = DateTime.UtcNow.AddMinutes(10),
            Verb = HttpVerb.GET,
            Protocol = Protocol.HTTPS
        };

        return await amazonS3Client.GetPreSignedURLAsync(request).ConfigureAwait(false);
    }

    public async Task RemoveAsync(
        string container,
        Guid key,
        CancellationToken cancellationToken = default) =>
        await amazonS3Client
            .DeleteObjectAsync(
                container,
                key.ToString(),
                cancellationToken)
            .ConfigureAwait(false);

    public void Dispose()
    {
        amazonS3Client.Dispose();
    }
}