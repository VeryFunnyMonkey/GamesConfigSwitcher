using GCS.Core;
using Microsoft.Extensions.Logging;

namespace GCS.CLI
{
    public class ConsoleLogger : ILoggingHandler
    {
        private readonly ILogger<ConsoleLogger> _logger;

        public ConsoleLogger(ILogger<ConsoleLogger> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message)
        {
            Console.WriteLine(message);
            _logger.LogInformation(message);
        }

        public void LogError(string message)
        {
            Console.Error.WriteLine(message);
            _logger.LogError(message);
        }

        public void LogWarning(string message)
        {
            Console.WriteLine(message);
            _logger.LogWarning(message);
        }
    }
}