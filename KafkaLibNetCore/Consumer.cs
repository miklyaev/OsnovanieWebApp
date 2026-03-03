using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Collections.Generic;

namespace KafkaLibNetCore
{
    /// <summary>
    /// интерфейс для возможности использовать DI
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface ICustomConsumer<TKey, TValue>
    {
        public void ConfigConsumer(string kafkaUrl, string groupId,
            string security, string sasl, string autoReset, bool enableAutoCommit);

        //private ConsumerConfig GetConsumerConfig(string kafkaUrl, string groupId,
        //    string security, string sasl, string autoReset, bool enableAutoCommit);

        public void SubscribeTopic(string topic);
        public void SubscribeTopics(List<string> topics);
        public void ConsumerClose();
        public void Commit(ConsumeResult<TKey, TValue> result);
        public ConsumeResult<TKey, TValue> ReadFromKafka(int interval);
    }
    /// <summary>
    /// Кафка консюмер
    /// </summary>
    public class Consumer<TKey, TValue> : ICustomConsumer<TKey, TValue>
    {
        private readonly ILogger _logger;
        private ConsumerConfig _consumerConfig;
        private IConsumer<TKey, TValue> _consumer;

        public Consumer(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
        }
        /// <summary>
        /// Конфигурация консюмера
        /// параметры берутся из окружения или конфиг.файлов и передаются в этот метод
        /// </summary>
        /// <param name="kafkaUrl"></param>
        /// <param name="groupId"></param>
        /// <param name="security"></param>
        /// <param name="sasl"></param>
        /// <param name="autoReset"></param>
        public void ConfigConsumer(string kafkaUrl, string groupId, string security, string sasl, string autoReset, bool enableAutoCommit)
        {
            try
            {
                _consumerConfig = GetConsumerConfig(kafkaUrl, groupId, security, sasl, autoReset, enableAutoCommit);
            }
            catch (KafkaException err)
            {
                _logger.Error($"Error Kafka consumer: {err.Error.Reason}");
            }
        }
        /// <summary>
        /// Подписка на топик, с которого считываем
        /// </summary>
        /// <param name="topic"></param>
        public void SubscribeTopic(string topic)
        {
            var success = true;

            _consumer = new ConsumerBuilder<TKey, TValue>(_consumerConfig)
            .SetErrorHandler((producer, error) =>
            {
                _logger.Error($"Error Kafka consumer: {error.Reason}");
                success = false;
            }).Build();

            try
            {
                if (success)
                {
                    _consumer.Subscribe(topic);
                }
            }
            catch (KafkaException err)
            {
                _logger.Error($"Subscribe error Kafka consumer: {err.Error.Reason}, topic = {topic}");
            }

        }
        /// <summary>
        /// Подписка на несколько топиков, с которых будем считывать
        /// </summary>
        /// <param name="topics"></param>
        public void SubscribeTopics(List<string> topics)
        {
            var success = true;
            _consumer = new ConsumerBuilder<TKey, TValue>(_consumerConfig)
            .SetErrorHandler((producer, error) =>
            {
                _logger.Error($"Error Kafka consumer: {error.Reason}");
                success = false;
            }).Build();

            try
            {
                if (success)
                {
                    _consumer.Subscribe(topics);
                }
            }
            catch (KafkaException err)
            {
                _logger.Error($"Subscribe error Kafka consumer: {err.Error.Reason}");
            }

        }

        public void ConsumerClose()
        {
            _consumer.Close();
        }

        /// <summary>
        /// Считывание из топика кафка
        /// Должна вызываться из внешнего потока
        /// </summary>
        /// <param name="topic">имя топика кафка</param>
        /// <param name="interval">время ожидания сообщения в мс</param>
        /// <returns></returns>
        public ConsumeResult<TKey, TValue> ReadFromKafka(int interval)
        {
            try
            {
                var cr = _consumer.Consume(interval);//(cts.Token);

                if (cr != null)
                {
                    _logger.Information($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                    return cr;
                }
                else
                    return null;
            }
            catch (KafkaException err)
            {
                _logger.Error($"Error Kafka consumer: {err.Error.Reason}");
                return null;
            }
        }
        /// <summary>
        /// подтверждение коммита вручную
        /// актуально, если параметр enable_auto_commit = false
        /// </summary>
        /// <param name="result"></param>
        public void Commit(ConsumeResult<TKey, TValue> result)
        {
            try
            {
                _consumer.Commit(result);
            }
            catch (KafkaException err)
            {
                _logger.Error($"Error Kafka consumer: {err.Error.Reason}");
            }
        }
        /// <summary>
        /// Получение конфигурационных параметров подключения к серверу кафка
        /// </summary>
        /// <param name="kafkaUrl"></param>
        /// <param name="groupId"></param>
        /// <param name="security"></param>
        /// <param name="sasl"></param>
        /// <param name="autoReset"></param>
        /// <param name="enableAutoCommit"></param>
        /// <returns>класс ConsumerConfig</returns>
        private ConsumerConfig GetConsumerConfig(string kafkaUrl, string groupId,
            string security, string sasl, string autoReset, bool enableAutoCommit = true)
        {
            return new ConsumerConfig
            {
                BootstrapServers = kafkaUrl,
                GroupId = groupId,
                SecurityProtocol = StaticConfig.GetSecurityProtocol(security),
                SaslMechanism = StaticConfig.GetSaslMechanism(sasl),
                AutoOffsetReset = StaticConfig.GetAutoOffsetReset(autoReset),
                EnableAutoCommit = enableAutoCommit
            };
        }

    }
}
