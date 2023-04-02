using Microsoft.Extensions.Options;
using NLog;
using QuickStart.Infra.Logging;
using QuickStart.Infra.Logging.ConfigOptions;
using LogLevel = NLog.LogLevel;

namespace QuickStartDemo
{
    public class MyLoggerHelper<T> : LoggerHelper<T>
    {
        private readonly TraceIdOptions _traceIdOptions;
        private readonly NLogOptions _logOptions;
        private readonly LogLevel _configLevel;
        private readonly string _loggerName;
        private readonly Logger _logger;
        public MyLoggerHelper(IOptionsSnapshot<TraceIdOptions> traceIdOption, IOptionsMonitor<NLogOptions> logOption)
            : base(traceIdOption, logOption)
        {
            _traceIdOptions = traceIdOption.Value;
            _logOptions = logOption.CurrentValue;
            _configLevel = LogLevel.FromString(_logOptions.LogLevel);
            _loggerName = typeof(T).FullName ?? string.Empty;
            _logger = LogManager.GetLogger(_loggerName);
        }

        public override void CustomizedLog(string message, string folderName)
        {
            string logName = "MyCust.UserLog";
            var cusLogger = LogManager.GetLogger(logName);
            if (string.IsNullOrWhiteSpace(folderName))
            {
                folderName = "default";
            }
            LogEventInfo logEventInfo = new LogEventInfo(LogLevel.Info, logName, "复写了啊啊啊啊啊" + message);
            logEventInfo.Properties["defaultlogpath"] = _logOptions.LogPath;
            logEventInfo.Properties["machinename"] = Environment.MachineName;
            logEventInfo.Properties["folder"] = folderName;
            cusLogger.Log(logEventInfo);
        }

        protected override void Log(LogLevel logLevel, string message, Exception? exception, string folderName)
        {
            if (logLevel < _configLevel)
            {
                return;
            }

            LogEventInfo logEventInfo;
            if (exception != null)
            {
                logEventInfo = new LogEventInfo(logLevel, _loggerName, null, "{0}", new object[1] { "复写了啊啊啊啊啊" + message }, exception);
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
