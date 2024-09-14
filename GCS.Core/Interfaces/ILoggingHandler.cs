using Microsoft.Extensions.Logging;

namespace GCS.Core
{
    public interface ILoggingHandler
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogDebug(string message);
    }
}