namespace QuickStart.Infra.Logging
{
    /// <summary>
    /// Logger helper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILoggerHelper<T>
    {
        /// <summary>
        /// Record Debug log with an exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="folderName"></param>
        void DebugLog(string message, Exception exception, string folderName = "");
        /// <summary>
        /// Record Debug log.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="folderName"></param>
        void DebugLog(string message, string folderName = "");
        /// <summary>
        /// Record Error log with an exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="folderName"></param>
        void ErrorLog(string message, Exception exception, string folderName = "");
        /// <summary>
        /// Record Error log.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="folderName"></param>
        void ErrorLog(string message, string folderName = "");
        /// <summary>
        /// Record Info log.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="folderName"></param>
        void InfoLog(string message, string folderName = "");
        /// <summary>
        /// Record customized log.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="folderName"></param>
        void CustomizedLog(string message, string folderName);
    }
}
