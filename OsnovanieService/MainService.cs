using Grpc.Net.Client;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OsnovanieService.Model;
using Serilog;

namespace OsnovanieService
{
    public class Test
    {
        public int typeId { get; set; }
        public string typeName { get; set; }
    }
    public interface IMainService
    {
        public string GetHelloWorld();
        public CheckRequestResponse ViewRequest();
        public ListRequestInfo ListRequest();
        public Task<User?> GetUser(int userId);
        public Task<ListOfUsers?> GetAllUsers();
        public Task<UniqueID> AddUser(User user);
        public Task<UniqueID> AddRegion(Region region);
        public Task<PersonReply> AddUserToKafka(User user);
        public Task<ListOfUsers> ReadFromKafka(string topic);
        public Task<Google.Protobuf.WellKnownTypes.Empty> AddSignalToKafka(Signal signal);

    }
    public class MainService : IMainService
    {
        public readonly IConfiguration _configuration;
        public readonly ILogger _logger;
        public readonly IDistributedCache _cache;

        public MainService(IConfiguration config, ILogger log, IDistributedCache distributedCache)
        {
            _configuration = config;
            _logger = log;
            _cache = distributedCache;
        }
        public string GetHelloWorld()
        {
            _logger.Information("Hello world!!!");
            return "Hello world!!!";
        }

        public CheckRequestResponse ViewRequest()
        {
            var jsonString = File.ReadAllText("view.json");
            return JsonConvert.DeserializeObject<CheckRequestResponse>(jsonString);
        }

        public ListRequestInfo ListRequest()
        {
            var jsonString = File.ReadAllText("list.json");
            return JsonConvert.DeserializeObject<ListRequestInfo>(jsonString);
        }

        public async Task<User?> GetUser(int userId)
        {
            var json = await _cache.GetStringAsync(Convert.ToString(userId));
            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<User>(json);
            }
            using var channel = GrpcChannel.ForAddress("https://localhost:7195");
            var client = new Greeter.GreeterClient(channel);
            UniqueID request = new UniqueID
            {
                Id = userId
            };
            var reply = await client.GetUserAsync(request);
            await _cache.SetStringAsync(Convert.ToString(userId), JsonConvert.SerializeObject(reply));
            return reply;
        }

        public async Task<ListOfUsers?> GetAllUsers()
        {
            var json = await _cache.GetStringAsync("all");
            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<ListOfUsers?>(json);
            }
            using var channel = GrpcChannel.ForAddress("https://localhost:7195");
            var client = new Greeter.GreeterClient(channel);
            global::Google.Protobuf.WellKnownTypes.Empty request = new global::Google.Protobuf.WellKnownTypes.Empty();
            var reply = await client.GetAllUsersAsync(request);
            await _cache.SetStringAsync("all", JsonConvert.SerializeObject(reply));
            return reply;
        }

        public async Task<UniqueID> AddUser(User user)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7195");
            var client = new Greeter.GreeterClient(channel);
            return await client.AddUserAsync(user);
        }

        public async Task<PersonReply> AddUserToKafka(User user)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7195");
            var client = new Greeter.GreeterClient(channel);
            return await client.WriteToKafkaAsync(user);
        }

        public async Task<ListOfUsers> ReadFromKafka(string topic)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7195");
            var client = new Greeter.GreeterClient(channel);
            return await client.ReadFromKafkaAsync(new Kafka
            {
                TopicName = topic
            });
        }

        public async Task<UniqueID> AddRegion(Region region)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7195");
            var client = new Greeter.GreeterClient(channel);
            return await client.AddRegionAsync(region);
        }

        public async Task<Google.Protobuf.WellKnownTypes.Empty> AddSignalToKafka(Signal signal)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7195");
            var client = new Greeter.GreeterClient(channel);
            return await client.AddSignalToKafkaAsync(signal);

        }
    }
}