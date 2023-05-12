using ClickHouseApp.DbService;
using Microsoft.Extensions.Options;

namespace ClickHouseApp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IClickHouseService _clickHouseService;
        public Worker(ILogger<Worker> logger, IClickHouseService clickHouseService)
        {
            _logger = logger;
            _clickHouseService = clickHouseService;
            _clickHouseService.AddUser(new Dto.User
            {
                UserId = 1,
                UserName = "Test",
                Age = 30,
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}