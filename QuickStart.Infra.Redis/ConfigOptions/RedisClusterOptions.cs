namespace QuickStart.Infra.Redis.ConfigOptions
{
    public class RedisClusterOptions
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 集群节点
        /// </summary>
        public string[] EndPoints { get; set; }
    }
}
