using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SignalRApp.Exceptions;
using SignalRApp.Model;
using System;
using System.Text;
using System.Threading.Channels;

namespace SignalRApp
{
    interface IRabbitMqConsumer
    {
        public void StartReceivingSignal();
    }
    public class RabbitMqConsumer : IRabbitMqConsumer, IDisposable
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly IConfiguration _configuration;
        private readonly string _exchange;
        private readonly string _queue;
        private readonly EventingBasicConsumer _consumer;

        delegate void ReceiveHandler(string message);
        /// <summary>
        /// событие, сигнализирующее, что consumer принял сообщение
        /// </summary>
        event ReceiveHandler? ReceiveNotify;

        public RabbitMqConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
            _exchange = configuration["EXCHANGE"];
            _queue = configuration["QUEUE"];
           

            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _configuration["HostName"],
                    UserName = _configuration["UserName"],
                    Password = _configuration["Password"]
                };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(_exchange, ExchangeType.Fanout); //декларируем Exchange

                _channel.QueueDeclare(_queue,
                 durable: true,
                 exclusive: false,
                 autoDelete: false,
                 arguments: null);                                       //декларируем очередь

                _channel.QueueBind(_queue, _exchange, "");              //привязываем очередь к обменнику
                _channel.BasicQos(0, 10, false);

                _consumer = new EventingBasicConsumer(_channel);
                _consumer.Received += OnMessageReceived;

                Console.WriteLine("RabbitMq consumer started");
            }
            catch (Exception exc)
            {
                throw new RabbitMqException(exc.Message);
            }
        }

        private void OnMessageReceived(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            ReceiveNotify?.Invoke(message);
        }

        public void Dispose()
        {
            _consumer.Received -= OnMessageReceived;
            _channel.Close();
            _channel.Dispose();
            _connection.Close();
            _connection.Dispose();
        }
        public void StartReceivingSignal()
        {
            _channel.BasicConsume(_queue, true, _consumer);

        }
    }
}
