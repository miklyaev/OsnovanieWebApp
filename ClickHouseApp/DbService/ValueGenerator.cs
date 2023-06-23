using ClickHouseApp.Dto;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickHouseApp.DbService
{
    public interface IValueGenerator
    {
        List<Signal> GenerateValues(int qty);
    }
    public class ValueGenerator : IJob, IValueGenerator
    {

        public readonly IClickHouseService _clickHouseService;
        private readonly ILogger<ValueGenerator> _logger;

        public readonly string[] _dictionary = new string[10]
        {
            "qwerty", "sos", "very", "good", "bad", "hello", "world", "john", "zzzz", "bbbbbb"
        };

        private static int _counter = 0;
        public ValueGenerator(IClickHouseService clickHouseService, ILogger<ValueGenerator> logger)
        {
            _clickHouseService = clickHouseService;
            _logger = logger;
        }


        public List<Signal> GenerateValues(int qty)
        {
            List<Signal> signals = new List<Signal>(100);
            int cnt = 0;
            while (cnt < qty)
            {
                signals.Add(RandomizeSignal());
                cnt++;
            }

            return signals;
        }

        private Signal RandomizeSignal()
        {
            //Создание объекта для генерации чисел
            Random rndTag = new Random();
            //Получить очередное (в данном случае - первое) случайное число
            int tagNumber = rndTag.Next(0, 100);

            Random rndType = new Random();
            int tagType = rndType.Next(1, 4);

            Signal signal = new Signal
            {
                SignalId = Guid.NewGuid(),
                TagName = $"Tag{tagNumber}",
                TagType = (TagTypeInfo)tagType,
                TagValue = GetValue((TagTypeInfo)tagType),
            };

            return signal;
        }

        private object GetValue(TagTypeInfo type)
        {
            object result = null; 
            switch (type)
            {
                case TagTypeInfo.Int:
                    {
                        Random rndInt = new Random();
                        result = rndInt.Next(0, 10000);
                    }
                    break;
                case TagTypeInfo.Float:
                    {
                        Random rndFloat = new Random();
                        result = (float)rndFloat.Next(0, 10000) / 100;
                    }
                    break;
                case TagTypeInfo.String:
                    {
                        Random rndFloat = new Random();
                        var ind = rndFloat.Next(0, 9);
                        result = _dictionary[ind];
                    }
                   break;
            }

            return result;
                      
        }
        public Task Execute(IJobExecutionContext context)
        {
            
            List<Signal> signals = GenerateValues(100);
             _clickHouseService.AddSignals(signals);
            _logger.LogInformation($"Запись сигналов в clickhouse {ValueGenerator._counter++}");
            return Task.CompletedTask;

        }

    }
}
