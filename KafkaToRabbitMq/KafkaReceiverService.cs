using Confluent.Kafka;
using KafkaLibNetCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;

namespace KafkaToRabbitMq
{
    public interface IKafkaReceiverService
    {
        public void InitializeKafka();
        /// <summary>
        /// Считывание из топика кафка
        /// Должна вызываться из внешнего потока
        /// </summary>
        /// <param name="topic">имя топика кафка</param>
        /// <param name="interval">время ожидания сообщения в мс</param>
        /// <returns></returns>
        public Task<ConsumeResult<string, string>?> ReadFromKafka();
        /// <summary>
        /// подтверждение коммита вручную
        /// актуально, если параметр enable_auto_commit = false
        /// </summary>
        /// <param name="result"></param>
        public void Commit(ConsumeResult<string, string> res);
    }
    internal class KafkaReceiverService : IKafkaReceiverService
    {
        private readonly ILogger _logger;
        private readonly ICustomConsumer<string, string> _consumer;
        private readonly IConfiguration _configuration;

        private int pollingInterval;

        public KafkaReceiverService(ILogger logger, ICustomConsumer<string, string> consumer, IConfiguration configuration)
        {
            _logger = logger;
            _consumer = consumer;
            _configuration = configuration;
        }

        public async Task<ConsumeResult<string, string>?> ReadFromKafka()
        {
            var result = await Task.Run(() =>_consumer.ReadFromKafka(Convert.ToInt32(_configuration["POLLING_INTERVAL"])));

            if (result != null)
            {
                _logger.Information("Из кафки успешно считано: topic={0}, offset={1}, key={2}, message={3}\n",
                result.Topic,
                result.Offset,
                result.Message.Key,
                result.Message.Value);
                return result;
            }

            return null;          
        }

        public void InitializeKafka()
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

                _consumer.SubscribeTopic(_configuration["KAFKA_TOPIC"]);
                _logger.Information("Инициализация успешно завершена.");

            }
            catch (Exception exc)
            {
                _logger.Error("{0}, stack {1}", exc.Message, exc.InnerException != null ? exc.InnerException.Message : "не доступен.");
            }
        }

        public void Commit(ConsumeResult<string, string> res)
        {
            _consumer.Commit(res);
        }
    }
}
