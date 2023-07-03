using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickHouseApp.DbService
{
    public interface IClickHouseConsumer
    {
        public bool ReadFromClickHouse();
    }
    public class ClickHouseConsumer : IJob, IClickHouseConsumer
    {
        private readonly ILogger<ValueGenerator> _logger;
        public ClickHouseConsumer(ILogger<ValueGenerator> logger)
        { 
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var result = Task.Run(() =>
            {
                ReadFromClickHouse();
            });
        }

        public bool ReadFromClickHouse()
        {
            _logger.LogInformation($"Read from {nameof(ClickHouseConsumer)}");
            return true;
        }
    }
}
