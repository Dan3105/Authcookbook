using AuthServer.Infracstructure.External;
using System.Threading.Tasks;

namespace AuthServer.Infracstructure
{
    public class ThirdPartyConfiguration
    {
        public static async Task InitThirdPartyServicesForDevelopmentEnv()
        {
            await RedisContainer.InitRedisDockerForDevelopmentEnv();
        }
    }
}
