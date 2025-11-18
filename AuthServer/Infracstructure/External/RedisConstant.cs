namespace AuthServer.Infracstructure.External
{
    public class RedisConstant
    {
        public readonly static string RedisInstanceName = "AuthServer_";
        public readonly static string RedisContainerName = "my-redis";
        public readonly static string RedisPort = "6379";
        public readonly static string RedisHost = "localhost";
        public readonly static string RedisProtocol = $"{RedisPort}:tcp";
        public static string RedisConnectionString => $"{RedisHost}:{RedisPort}";
    }
}
