namespace Core.BlobStorage;

public interface IBlobStorageClient : IDisposable
{
    public Task<Guid> PutAsync(string container, byte[] blob, CancellationToken cancellationToken = default);

    public Task<byte[]> GetAsync(string container, Guid key, CancellationToken cancellationToken = default);

    public Task<string> GetDownloadingUrlAsync(string container, Guid key, CancellationToken cancellationToken = default);
    
    public Task RemoveAsync(string container, Guid key, CancellationToken cancellationToken = default);
}