namespace Core.Crypto;

public interface IHashPasswordService
{
    public string EvaluateHash(string password);
    public bool ComparePassword(string actualPassword, string expectedPasswordHash);
}