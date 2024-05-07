namespace Core.BlobStorage;

public class BlobStorageException : Exception
{
    public BlobStorageException(string? message) 
        : base(message)
    {
    }
}