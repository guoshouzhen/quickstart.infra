namespace QuickStart.Infra.Redis.ConfigOptions
{
    public class RedisClusterOptions
    {
        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Redis cluster endpoints.
        /// </summary>
        public string[] EndPoints { get; set; }
    }
}
