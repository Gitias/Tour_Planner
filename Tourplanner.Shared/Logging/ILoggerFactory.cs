namespace Tourplanner.Shared.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger<TContext>();
    }
}
