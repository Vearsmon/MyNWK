namespace Core.Crypto;

public interface ITgAuthService
{
    public static IReadOnlyList<string> KeysToUseInHash { get; } = new[]
    {
        "auth_date",
        "first_name",
        "id",
        "last_name",
        "photo_url",
        "username"
    };

    public bool IsUserMetaValid(string expectedHash, Dictionary<string, string> data);
    
    public bool ValidateData(byte[] key, string expectedHash, Dictionary<string, string> data);
}