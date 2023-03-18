using Confluent.Kafka;
using KafkaToRabbitMq.Exceptions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static KafkaLibNetCore.ICustomProducer;

namespace KafkaToRabbitMq
{
    public interface IRabbitMq
    {
        public void SendMessage(string message);

        public string ReceiveMessage(string topic);
    }
    public class RabbitMq : IRabbitMq, IDisposable
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly IConfiguration _configuration;
        private readonly string _exchange;
        private readonly string _queue;

        public RabbitMq(IConfiguration configuration)
        {
            _configuration = configuration;
            _exchange = configuration["EXCHANGE"];
            _queue = configuration["QUEUE"];

            try
            {
                var factory = new ConnectionFactory() { HostName = _configuration["HostName"],
                                                        UserName = _configuration["UserName"],
                                                        Password = _configuration["Password"] 
                                                       };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                 
                if (Convert.ToBoolean(configuration["PRODUCER"])) //Если режим продюсера
                {
                    _channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout);
                    Console.WriteLine("RabbitMq producer started");
                }
                else  //консюмер
                {
                    _channel.ExchangeDeclare(_exchange, ExchangeType.Fanout);
                    _channel.QueueDeclare(_queue,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

                    _channel.QueueBind(_queue, _exchange, string.Empty);
                    _channel.BasicQos(0, 10, false);

                    var consumer = new EventingBasicConsumer(_channel);
                    consumer.Received += (sender, e) =>
                    {
                        var body = e.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(message);
                    };

                    _channel.BasicConsume(_queue, true, consumer);
                    Console.WriteLine("RabbitMq consumer started");
                }
  
            }
            catch (Exception exc)
            {
                throw new RabbitMqException(exc.Message);
            }
            
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

            _channel.BasicPublish(exchange: _exchange,
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);
        }
    }
}
