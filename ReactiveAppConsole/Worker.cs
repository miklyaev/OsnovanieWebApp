using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAppConsole
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRabbitMqConsumer _consumer;
        private readonly IConfiguration _configuration;

        public Worker(IConfiguration configuration, ILogger<Worker> logger, IRabbitMqConsumer consumer)
        {
            _logger = logger;
            _consumer = consumer;
            _configuration = configuration;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Observable.Interval(TimeSpan.FromSeconds(1))
            //             .Window(2)
            //             .Subscribe(group =>
            //             {
            //                 _logger.LogInformation($"{DateTime.Now.Second}: Starting new group");
            //                 group.Subscribe(
            //                 x => _logger.LogInformation($"{DateTime.Now.Second}: Saw {x}"),
            //                 () => _logger.LogInformation($"{DateTime.Now.Second}: Ending group"));
            //             });


            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {

                    _consumer.StartReceivingSignal();
                    _logger.LogInformation($"StartReceivingSignal = {stoppingToken.IsCancellationRequested}");
                    Thread.Sleep(5000);
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogInformation($"stoppingToken.IsCancellationRequested = {stoppingToken.IsCancellationRequested}");
                return Task.FromException(ex);
            }

            return Task.CompletedTask;
        }
    }
}
