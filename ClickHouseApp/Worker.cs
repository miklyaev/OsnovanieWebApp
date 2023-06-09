using ClickHouseApp.DbService;
using Microsoft.Extensions.Options;

namespace ClickHouseApp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IClickHouseService _clickHouseService;
        private readonly IConfiguration _configuration;
        public Worker(ILogger<Worker> logger, IClickHouseService clickHouseService, IConfiguration configuration)
        {
            _logger = logger;
            _clickHouseService = clickHouseService;
            _clickHouseService.AddUser(new Dto.User
            {
                UserId = 10,
                UserName= "Second",
                Weight= 3.6
            });

            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(Convert.ToInt32(_configuration["POLLING_INTERVAL"]), stoppingToken);
            }
        }
    }
}