using Microsoft.Extensions.Options;
using NLog;
using QuickStart.Infra.Logging.ConfigOptions;

namespace QuickStart.Infra.Logging
{
    public class LoggerHelper<T> : ILoggerHelper<T>
    {
        private readonly TraceIdOptions _traceIdOptions;
        private readonly NLogOptions _logOptions;
        private readonly LogLevel _configLevel;
        private readonly string _loggerName;
        private readonly Logger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="traceIdOption"></param>
        /// <param name="logOption"></param>
        public LoggerHelper(IOptionsSnapshot<TraceIdOptions> traceIdOption, IOptionsMonitor<NLogOptions> logOption)
        {
            _traceIdOptions = traceIdOption.Value;
            _logOptions = logOption.CurrentValue;
            _configLevel = LogLevel.FromString(_logOptions.LogLevel);
            _loggerName = typeof(T).FullName ?? string.Empty;
            _logger = LogManager.GetLogger(_loggerName);
        }

        /// <inheritdoc/>
        public void DebugLog(string message, Exception exception, string folderName = "")
        {
            Log(LogLevel.Debug, message, exception, folderName);
        }

        /// <inheritdoc/>
        public void DebugLog(string message, string folderName = "")
        {
            Log(LogLevel.Debug, message, null, folderName);
        }

        /// <inheritdoc/>
        public void InfoLog(string message, string folderName = "")
        {
            Log(LogLevel.Info, message, null, folderName);
        }

        /// <inheritdoc/>
        public void ErrorLog(string message, Exception exception, string folderName = "")
        {
            Log(LogLevel.Error, message, exception, folderName);
        }

        /// <inheritdoc/>
        public void ErrorLog(string message, string folderName = "")
        {
            Log(LogLevel.Error, message, null, folderName);
        }

        /// <inheritdoc/>
        public virtual void CustomizedLog(string message, string folderName)
        {
            string logName = "MyCust.UserLog";
            var cusLogger = LogManager.GetLogger(logName);
            if (string.IsNullOrWhiteSpace(folderName)) 
            {
                folderName = "default";
            }
            LogEventInfo logEventInfo = new LogEventInfo(LogLevel.Info, logName, message);
            logEventInfo.Properties["defaultlogpath"] = _logOptions.LogPath;
            logEventInfo.Properties["machinename"] = Environment.MachineName;
            logEventInfo.Properties["folder"] = folderName;
            cusLogger.Log(logEventInfo);
        }

        /// <summary>
        /// Record log into file.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="folderName"></param>
        protected virtual void Log(LogLevel logLevel, string message, Exception? exception, string folderName) 
        {
            if (logLevel < _configLevel) 
            {
                return;
            }

            LogEventInfo logEventInfo; 
            if (exception != null)
            {
                logEventInfo = new LogEventInfo(logLevel, _loggerName, null, "{0}", new object[1] { message }, exception);
            }
            else 
            {
                logEventInfo = new LogEventInfo(logLevel, _loggerName, message);
            }
            logEventInfo.Properties["defaultlogpath"] = _logOptions.LogPath;
            logEventInfo.Properties["machinename"] = Environment.MachineName;
            logEventInfo.Properties["traceid"] = _traceIdOptions.TraceId;
            logEventInfo.Properties["folder"] = Path.Combine(DateTime.Now.ToString("yyyy-MM"), folderName ?? string.Empty);
            _logger.Log(logEventInfo);
        }
    }
}
