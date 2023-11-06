using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ILogger = Serilog.ILogger;

namespace KafkaToRabbitMq
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IKafkaReceiverService _receiver;
        private readonly IRabbitMqProducer _senderToRabbit;
        private readonly IConfiguration _configuration;


        public Worker(ILogger logger, IKafkaReceiverService receiver, IRabbitMqProducer sender, IConfiguration configuration)
        {
            _logger = logger;
            _receiver = receiver;
            _senderToRabbit = sender;
            _configuration = configuration;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _receiver.InitializeKafka();
            return base.StartAsync(cancellationToken);           
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Information($"Получена команда Stop");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {

                    var result = await _receiver.ReadFromKafka();

                    if (result != null)
                    {
                        _senderToRabbit.SendMessage(result.Value);
                        _receiver.Commit(result);
                    }

                    await Task.Delay(Convert.ToInt16(_configuration["POLLING_INTERVAL"]));
                    //Thread.Sleep(Convert.ToInt16(_configuration["POLLING_INTERVAL"]));                 
                }
            }
            catch (OperationCanceledException)
            {
                _logger.Information($"stoppingToken.IsCancellationRequested = {stoppingToken.IsCancellationRequested}");
                throw;
            }
        }
    }
}