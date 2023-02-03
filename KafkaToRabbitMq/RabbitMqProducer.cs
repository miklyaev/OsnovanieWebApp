using Confluent.Kafka;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace KafkaToRabbitMq
{
    public interface IRabbitMqProducer
    {
        public void SendMessage(string message);

        public string ReceiveMessage(string topic);
    }
    public class RabbitMqProducer : IRabbitMqProducer, IDisposable
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;

        public RabbitMqProducer()
        {
            var factory = new ConnectionFactory() { HostName = "192.168.0.104", UserName = "admin", Password = "130469" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }

        public string ReceiveMessage(string topic)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "logs",
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);
        }
    }
}
