using System.Diagnostics;

namespace Core.Helpers;

public static class PreciseTimestampGenerator
{
    private static DateTime synchronizationMark;
    
    private static readonly Stopwatch Stopwatch;
    private static readonly TimeSpan ConfidenceWindow = TimeSpan.FromMilliseconds(128);
    private static readonly object LockObject = new();
    
    static PreciseTimestampGenerator()
    {
        synchronizationMark = DateTime.UtcNow;
        Stopwatch = new Stopwatch();
        
        Synchronize();
    }

    public static DateTime Generate()
    {
        lock (LockObject)
        {
            Synchronize();

            var elapsed = Stopwatch.Elapsed;
            if (elapsed < TimeSpan.Zero)
            {
                return synchronizationMark;
            }

            return synchronizationMark + elapsed;
        }
    }

    private static void Synchronize()
    {
        while (Stopwatch.Elapsed > ConfidenceWindow || DateTime.UtcNow - synchronizationMark > ConfidenceWindow)
        {
            synchronizationMark = DateTime.UtcNow;
            Stopwatch.Restart();
        }
    }
}