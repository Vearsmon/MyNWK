using Amazon.S3;
using Core.BlobStorage;
using FluentAssertions;

namespace TestsCore;

[TestFixture]
public class BlobStorage_Test
{
    private readonly IBlobStorageClient blobStorageClient;
    private readonly List<Guid> blobsCreated;

    private static readonly string blobContainer = BlobContainers.ProductImages;

    public BlobStorage_Test()
    {
        blobStorageClient = new YdBlobStorageClient();
        blobsCreated = new List<Guid>();
    }

    [TearDown]
    public async Task Teardown()
    {
        foreach (var key in blobsCreated)
        {
            await blobStorageClient.RemoveAsync(blobContainer, key);
        }
        blobsCreated.Clear();
    }

    [Test]
    public async Task CreateObject_Than_Get_Should_Be_Equivalent()
    {
        var blobExpected = GenerateBlob(128);
        var key = await PutBlobAsync(blobExpected);

        var actualBlob = await blobStorageClient.GetAsync(
            BlobContainers.ProductImages,
            key);

        actualBlob.Should().BeEquivalentTo(blobExpected);
    }

    [Test]
    public async Task Delete_Than_Get_Should_Throw()
    {
        var blobExpected = GenerateBlob(128);
        var key = await PutBlobAsync(blobExpected, false);

        await blobStorageClient.RemoveAsync(blobContainer, key);
        
        var exception = Assert.ThrowsAsync<AmazonS3Exception>(() => blobStorageClient.GetAsync(blobContainer, key));
        
        exception.Should().NotBeNull();
        exception!.ErrorCode.Should().Be("NoSuchKey");
    }

    [Test]
    public async Task CreateObject_Than_Get_Downloading_URL_Should_Success()
    {
        var blob = GenerateBlob(128);
        var key = await PutBlobAsync(blob, false);
        
        Assert.DoesNotThrowAsync(() => blobStorageClient.GetDownloadingURLAsync(blobContainer, key));
        Console.WriteLine(await blobStorageClient.GetDownloadingURLAsync(blobContainer, key));
    }
    
    private async Task<Guid> PutBlobAsync(byte[] blob, bool pushDeletionTask = true)
    {
        var key = await blobStorageClient.PutAsync(blobContainer, blob);
        
        if (pushDeletionTask)
        {
            blobsCreated.Add(key);
        }
        return key;
    }

    private static byte[] GenerateBlob(int bytesCount)
    {
        var blob = new byte[bytesCount];
        Random.Shared.NextBytes(blob);
        
        return blob;
    }
}