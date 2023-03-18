using Confluent.Kafka;
using Grpc.Core;
using GrpcService1.DbService;
using KafkaLibNetCore;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace GrpcService1.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly INpgSqlService _pgSqlService;
        private readonly ICustomProducer _producer;
        private readonly ICustomConsumer<string, string> _consumer;
        private readonly IConfiguration _configuration;

        private string kafkaTopic;

        public GreeterService(Serilog.ILogger logger
            , INpgSqlService npgSqlService
            , ICustomProducer producer
            , ICustomConsumer<string, string> consumer
            , IConfiguration configuration
            )
        {
            _logger = logger;
            _pgSqlService = npgSqlService;
            _producer = producer;
            _consumer = consumer;   
            _configuration = configuration;

            InitializeKafka();
        }

        public void InitializeKafka()
        {
            try
            {

                _producer.ConfigProducer(_configuration["KAFKA_URL"],
                                          _configuration["SECURITY_PROTOCOL"],
                                          _configuration["SASL_MECHANISM"]);

                kafkaTopic = _configuration.GetValue<string>("KAFKA_TOPIC");
                _logger.Information("Информация по кафка успешно считана.");

                _producer.ErrorNotify += OnError;

                _consumer.ConfigConsumer(
                    _configuration["KAFKA_URL"],
                    _configuration["KAFKA_GROUP_ID"],
                    _configuration["SECURITY_PROTOCOL"],
                    _configuration["SASL_MECHANISM"],
                    _configuration["AUTO_OFFSET_RESET"],
                    _configuration.GetValue<bool>("ENABLE_AUTO_COMMIT")
                    );

                _consumer.SubscribeTopic(kafkaTopic);
            }
            catch (Exception exc)
            {
                _logger.Error("{0}, stack {1}", exc.Message, exc.InnerException != null ? exc.InnerException.Message : "не доступен.");
            }
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = $"Hello {request.Name} {request.Age}"//"Hello " + request.Name
            });
        }

        public override Task<PersonReply> GetAll(global::Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        {
            return Task.FromResult(new PersonReply
            {
                Message = $"Hello All"
            });
        }

        public override Task<ListOfUsers> GetAllUsers(global::Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        {
            var result = new ListOfUsers();
            result.Data.AddRange(_pgSqlService.GetAll());

            return Task.FromResult(result);
        }
        public override Task<User> GetUser(UniqueID request, ServerCallContext context)
        {
            User user = _pgSqlService.GetById(request.Id);
            return Task.FromResult(user);
        }

        public override Task<UniqueID> AddUser(User request, ServerCallContext context)
        {
            int id = _pgSqlService.AddUser(request);
            return Task.FromResult(new UniqueID
            {
                Id = id
            });

        }

        public override Task<UniqueID> AddRegion(Region request, ServerCallContext context)
        {
            int id = _pgSqlService.AddRegion(request);
            return Task.FromResult(new UniqueID
            {
                Id = id
            });

        }

        public override Task<PersonReply> WriteToKafka(User request, ServerCallContext context)
        {
            string key = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff");
            string jsonValue = JsonConvert.SerializeObject(request, Formatting.Indented);
            try
            {
                _producer.WriteToKafka(kafkaTopic, key, jsonValue);

                return Task.FromResult(new PersonReply
                {
                    Message = "Success"
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override Task<ListOfUsers> ReadFromKafka(Kafka request, ServerCallContext context)
        {
            var result = new ListOfUsers();
            var kafkaResult = _consumer.ReadFromKafka(500);
            List<User> users = new List<User>();
            try
            {
                if (kafkaResult?.Message.Value != null)
                {
                    User user = JsonConvert.DeserializeObject<User>(kafkaResult.Message.Value);
                    users.Add(user!);
                    result.Data.AddRange(users);

                }

                return Task.FromResult(result);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public override Task<ServiceResponse> AddSignalToKafka(Signal request, ServerCallContext context)
        {
            string key = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff");
            string jsonValue = JsonConvert.SerializeObject(request, Formatting.Indented);
            try
            {
                _producer.WriteToKafkaAsync("sdfsdf", key, jsonValue);

                return Task.FromResult(new ServiceResponse
                {
                    Code = 200,
                    ErrorMessage = ""
                });

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void OnError(string message)
        {
            _logger.Error($"Error send to kafka!!! {message}");
        }

    }
}