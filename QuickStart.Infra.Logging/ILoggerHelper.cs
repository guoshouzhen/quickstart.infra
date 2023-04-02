namespace QuickStart.Infra.Logging
{
    public interface ILoggerHelper<T>
    {
        void DebugLog(string message, Exception exception, string folderName = "");
        void DebugLog(string message, string folderName = "");
        void ErrorLog(string message, Exception exception, string folderName = "");
        void ErrorLog(string message, string folderName = "");
        void InfoLog(string message, string folderName = "");
        void CustomizedLog(string message, string folderName);
    }
}
