using Confluent.Kafka;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ILogger = Serilog.ILogger;

namespace KafkaToRabbitMq
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IKafkaReceiverService _receiver;
        private readonly IRabbitMq _senderRoRabbit;

        public Worker(ILogger logger, IKafkaReceiverService receiver, IRabbitMq sender)
        {
            _logger = logger;
            _receiver = receiver;
            _senderRoRabbit = sender;
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

                    var result = _receiver.ReadFromKafka().Result;                   

                    if (result != null)
                    {
                        _senderRoRabbit.SendMessage(result.Value);
                        _receiver.Commit(result);
                    }
                    
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