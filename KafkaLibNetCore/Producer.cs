using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Threading.Tasks;

namespace KafkaLibNetCore
{
    /// <summary>
    /// интерфейс для возможности использовать DI
    /// </summary>
    public interface ICustomProducer
    {
        /// <summary>
        /// Сихронная запись в кафка
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool WriteToKafka(string topic, string key, string value);
        /// <summary>
        /// Асинхронная запись в кафка
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task WriteToKafkaAsync(string topic, string key, string value);
        /// <summary>
        /// Конфигурация продюсера
        /// </summary>
        /// <param name="kafkaUrl"></param>
        /// <param name="security"></param>
        /// <param name="sasl"></param>
        void ConfigProducer(string kafkaUrl, string security, string sasl);

        delegate void ErrorHandler(string message);
        /// <summary>
        /// событие, при провале записи в кафку
        /// </summary>
        event ErrorHandler? ErrorNotify;

        delegate void SuccessHandler(string message);
        /// <summary>
        /// событие, сигнализирующее, что в кафку успешно записано
        /// </summary>
        event SuccessHandler? SuccessNotify;
    }
    /// <summary>
    /// Kafka продюсер
    /// </summary>
    public class Producer : ICustomProducer
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        IProducer<string, string> _producer = null;
        public event ICustomProducer.ErrorHandler? ErrorNotify;
        public event ICustomProducer.SuccessHandler? SuccessNotify;

        public Producer(ILogger logger, IConfiguration config)
        {
            _configuration = config;
            _logger = logger;
        }

        public void ConfigProducer(string kafkaUrl, string security, string sasl)
        {
            ProducerConfig config = GetProducerConfig(kafkaUrl, security, sasl);
            _producer = new ProducerBuilder<string, string>(config)
                .SetErrorHandler((producer, error) =>
                {
                    _logger.Error($"Error Kafka producer: {error.Reason}");

                }).Build();

        }
        private ProducerConfig GetProducerConfig(string kafkaUrl, string security, string sasl)
        {
            return new ProducerConfig
            {
                BootstrapServers = kafkaUrl,
                SecurityProtocol = StaticConfig.GetSecurityProtocol(security),
                SaslMechanism = StaticConfig.GetSaslMechanism(sasl)
            };
        }

        /// <summary>
        /// Запись в кафка (асинхронно), должна вызываться из внешнего потока
        /// Успех записи или провал обработать функцией обратного вызова (повесить на событие: ErrorNotify или SuccessNotify)
        /// </summary>
        /// <param name="topic">Топик куда пишем</param>
        /// <param name="key">Ключ (необязательно)</param>
        /// <param name="value">Само сообщение</param>
        public Task WriteToKafkaAsync(string topic, string key, string value)
        {
            var dr = _producer.ProduceAsync(topic, new Message<string, string>()
            {
                Key = key,
                Value = value
            });

            _ = dr.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    _logger.Error($"Error Kafka producer: {task.Exception.Message}");
                    ErrorNotify?.Invoke($"Failed sending to kafka topic = {topic}, key = {key}, exception = {task.Exception.Message}");
                }
                else if (task.IsCompleted)
                {
                    SuccessNotify?.Invoke($"Success sending to kafka topic = {topic}, key= {key}, value = {value}");
                }
            
            });

            return Task.CompletedTask;
        }
        /// <summary>
        /// Запись в кафка (синхронно), должна вызываться из внешнего потока в блоке try/catch
        /// Успех записи - возвращает true, провал - выкидывает исключение
        /// </summary>
        /// <param name="topic">Топик куда пишем</param>
        /// <param name="key">Ключ (необязательно)</param>
        /// <param name="value">Само сообщение</param>
        /// <returns>Успех записи</returns>
        public bool WriteToKafka(string topic, string key, string value)
        {
            var dr = _producer.ProduceAsync(topic, new Message<string, string>()
            {
                Key = key,
                Value = value
            });
            dr.Wait();
            return true;
        }

    }
}
