namespace Core;

public class RequestContext
{
    public int? UserId { get; init; }
    public CancellationToken CancellationToken { get; init; }
}