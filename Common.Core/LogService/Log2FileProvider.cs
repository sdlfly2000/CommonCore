using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Common.Core.LogService
{
    public class Log2FileProvider : ILoggerProvider
    {
        private readonly IConfiguration _configuration;

        private ILog2File? _logger;

        public Log2FileProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (_logger == null)
            {
                _logger = new Log2File(_configuration);
            }

            return _logger;
        }

        public void Dispose()
        {
            if(_logger != null)
            {
                _logger.Dispose();
            }
        }
    }
}
