namespace QuickStart.Infra.Logging.ConfigOptions
{
    /// <summary>
    /// NLog options.
    /// </summary>
    public class NLogOptions
    {
        /// <summary>
        /// Log file saved path.
        /// </summary>
        public string LogPath { get; set; }
        /// <summary>
        /// NLog config file relative path.
        /// </summary>
        public string ConfigFileRelativePath { get; set; }
        /// <summary>
        /// Log level, eg: Debug Info Error.
        /// </summary>
        public string LogLevel { get; set; }
    }
}
