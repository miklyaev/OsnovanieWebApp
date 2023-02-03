using KafkaLibNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;

namespace KafkaToRabbitMqConsole
{
    public interface IReceiverService
    {
        public string ReadFromKafka();
    }
    internal class ReceiverService : IReceiverService
    {
        private readonly ILogger _logger;
        private readonly ICustomConsumer<string, string> _consumer;
        private readonly IConfiguration _configuration;

        private int pollingInterval;

        public ReceiverService(ILogger logger, ICustomConsumer<string, string> consumer, IConfiguration configuration)
        {
            _logger = logger;
            _consumer = consumer;
            _configuration = configuration;
        }

        public string ReadFromKafka()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            try
            {
                pollingInterval = _configuration.GetValue<int>("POLLING_INTERVAL");
                _consumer.ConfigConsumer(_configuration["KAFKA_URL"],
                                         _configuration["KAFKA_GROUP_ID"],
                                         _configuration["SECURITY_PROTOCOL"],
                                         _configuration["SASL_MECHANISM"],
                                         _configuration["AUTO_OFFSET_RESET"],
                                         Convert.ToBoolean(_configuration["ENABLE_AUTO_COMMIT"]));

                _logger.Information("Инициализация. Завершена успешно.");

            }
            catch (Exception exc)
            {
                _logger.Error("{0}, stack {1}", exc.Message, exc.InnerException != null ? exc.InnerException.Message : "не доступен.");
            }
        }
    }
}
