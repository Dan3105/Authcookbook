using Docker.DotNet;
using Docker.DotNet.Models;

namespace AuthServer.Infracstructure.External
{
    public class RedisContainer
    {
        public static async Task InitRedisDockerForDevelopmentEnv()
        {
            var client = new DockerClientConfiguration().CreateClient();
            var containers = await client.Containers.ListContainersAsync(
                new ContainersListParameters { All = true });
            string containerId = string.Empty;
            var existing = containers.FirstOrDefault(c => c.Names.Contains($"/{RedisConstant.RedisContainerName}"));

            if (existing != null)
            {
                if (existing.State == "running")
                {
                    return;
                }
                containerId = existing.ID;
            } else
            {
                CreateContainerResponse container = await InitNewRedisContainer(client);
                containerId = container.ID;
            }

            await client.Containers.StartContainerAsync(
                containerId,
                new ContainerStartParameters());
        }

        private static async Task<CreateContainerResponse> InitNewRedisContainer(DockerClient client)
        {
            await client.Images.CreateImageAsync(
                            new ImagesCreateParameters
                            {
                                FromImage = "redis",
                                Tag = "latest"
                            },
                            null,
                            new Progress<JSONMessage>());

            // Create and start container
            var container = await client.Containers.CreateContainerAsync(
                new CreateContainerParameters
                {
                    Image = "redis:latest",
                    Name = RedisConstant.RedisContainerName,
                    HostConfig = new HostConfig
                    {
                        PortBindings = new Dictionary<string, IList<PortBinding>>
                        {
                            {
                                RedisConstant.RedisProtocol,
                                new List<PortBinding> { new PortBinding { HostPort = RedisConstant.RedisPort } }
                            }
                        }
                    }
                });
            return container;
        }
    }
}