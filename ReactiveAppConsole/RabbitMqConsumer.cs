﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReactiveAppConsole.Model;
using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Text;
using static ReactiveAppConsole.RabbitMqConsumer;

namespace ReactiveAppConsole
{
    public interface IRabbitMqConsumer
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
        public readonly ILogger<RabbitMqConsumer> _logger;
        private readonly EventingBasicConsumer _consumer;
        private IDisposable _subscription;

        public ConcurrentDictionary<DateTime, Signal> _dictionary = new ConcurrentDictionary<DateTime, Signal>();

        public RabbitMqConsumer(IConfiguration configuration, ILogger<RabbitMqConsumer> logger)
        {
            _configuration = configuration;
            _exchange = configuration["EXCHANGE"];
            _queue = configuration["QUEUE"];
            _logger = logger;


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

                _logger.LogInformation("RabbitMq consumer started");
            }
            catch (Exception exc)
            {
                throw new RabbitMqException(exc.Message);
            }
        }

        public void Dispose()
        {
            _channel.Close();
            _channel.Dispose();
            _connection.Close();
            _connection.Dispose();
            _subscription.Dispose();
            _logger.LogInformation("RabbitMq consumer disposed");
        }
        public void StartReceivingSignal()
        {
            _logger.LogInformation("RabbitMq consumer started to receive signals");
            _channel.BasicConsume(_queue, true, _consumer);

            var messages = Observable.FromEventPattern<EventHandler<BasicDeliverEventArgs>, BasicDeliverEventArgs>(
                     h => _consumer.Received += h,
                     h => _consumer.Received -= h)
                     .Select(receiveEvent => Encoding.UTF8.GetString(receiveEvent.EventArgs.Body.ToArray()))
                     .Synchronize();
            //Пока просто пишем в консоль принятые объекты
            _subscription= messages.Subscribe(message => _logger.LogInformation($"message = {message}")
                                    , exc => _logger.LogError($"error = {exc}")
                                    , () => _logger.LogInformation("completed")
                                    );

        }
    }
}
