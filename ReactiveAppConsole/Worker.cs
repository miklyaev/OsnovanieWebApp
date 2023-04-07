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

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Observable.Interval(TimeSpan.FromSeconds(1))
                         .Window(2)
                         .Subscribe(group =>
                         {
                             _logger.LogInformation($"{DateTime.Now.Second}: Starting new group");
                             group.Subscribe(
                             x => _logger.LogInformation($"{DateTime.Now.Second}: Saw {x}"),
                             () => _logger.LogInformation($"{DateTime.Now.Second}: Ending group"));
                         });

            return Task.CompletedTask;

        }
    }
}
