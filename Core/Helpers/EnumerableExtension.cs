using JetBrains.Annotations;

namespace Core.Helpers;

public static class EnumerableExtension
{
    public static async Task ForEachAsync<TEntity>(
        this IEnumerable<Task<TEntity>> enumerable,
        [InstantHandle] Action<TEntity> action)
    {
        var tasks = enumerable
            .Select(item => item.ContinueWith(t => action(t.Result)))
            .ToList();

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }
    
    public static async Task ForEachAsync<TEntity>(
        this IEnumerable<TEntity> enumerable,
        [InstantHandle] Func<TEntity, Task> func)
    {
        var tasks = enumerable
            .Select(func)
            .ToList();

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }
    
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable)
    {
        return enumerable is null || !enumerable.Any();
    }

    public static async Task<byte[]> ReadToEndAsync(
        this Stream stream, 
        int bufferSize,
        CancellationToken? cancellationToken = null)
    {
        var nonNullableCancellationToken = cancellationToken ?? CancellationToken.None;

        var result = new List<byte>(bufferSize);
        var buffer = new byte[bufferSize];
        
        var bytesRead = await stream
            .ReadAsync(buffer, nonNullableCancellationToken)
            .ConfigureAwait(false);
        while (bytesRead > 0)
        {
            result.AddRange(buffer.Take(bytesRead));
            bytesRead = await stream
                .ReadAsync(buffer, nonNullableCancellationToken)
                .ConfigureAwait(false);
        }

        return result.ToArray();
    }
}