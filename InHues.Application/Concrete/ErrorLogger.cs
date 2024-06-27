using InHues.Application.Common.Interfaces;
using InHues.Domain.Persistence;
using Microsoft.Extensions.Logging;
using System;

namespace InHues.Application.Concrete
{
    public class ErrorLogger : IErrorLogger
    {
        private readonly ILogger<ErrorLogger> _logger;
        private readonly IMainContext _mainContext;

        public ErrorLogger(ILogger<ErrorLogger> logger,IMainContext mainContext)
        {
            _logger = logger;
            _mainContext = mainContext;
        }
        public void LogError(string message, string stackTrace)
        {
            try {
                _mainContext.ErrorLogs.Add(new()
                {
                    Message = message,
                    StackTrace = stackTrace,
                });
                _mainContext.SaveChangesAsync();
            }
            catch(Exception error) {
                _logger.LogError(error.Message);
                _logger.LogTrace(error.StackTrace);
            }
        }
    }
}
