using ILogger = Serilog.ILogger;

namespace SignalRApp
{
    public class ReceiverService : BackgroundService
    {
        public readonly IConfiguration _configuration;
        public readonly ILogger _logger;
        public readonly IRabbitMqConsumer _rabbitMqConsumer;
        public readonly IChat _chat;
        public ReceiverService(IConfiguration configuration, IRabbitMqConsumer rabbitMqConsumer, ILogger logger, IChat chat)
        {
            _configuration = configuration;
            _logger = logger;
            _rabbitMqConsumer = rabbitMqConsumer;
            _chat = chat;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _rabbitMqConsumer.ReceiveNotify += OnReceiveFromRabbitMq;
            _rabbitMqConsumer.StartReceivingSignal();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Information($"Получена команда Stop");
            _rabbitMqConsumer.ReceiveNotify -= OnReceiveFromRabbitMq;
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(500);
                }

                _logger.Information($"stoppingToken.IsCancellationRequested = {stoppingToken.IsCancellationRequested}");
            }
            catch (OperationCanceledException)
            {
                _logger.Information($"stoppingToken.IsCancellationRequested = {stoppingToken.IsCancellationRequested}");
            }
        }

        public void OnReceiveFromRabbitMq(string message)
        {
            _chat.Send(message, "system");
        }
    }
}
