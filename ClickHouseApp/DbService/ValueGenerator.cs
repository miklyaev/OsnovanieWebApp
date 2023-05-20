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
        public ValueGenerator(IClickHouseService clickHouseService)
        {
            _clickHouseService = clickHouseService;
        }


        public List<Signal> GenerateValues(int qty)
        {
            List<Signal> signals = new List<Signal>();
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
            Random rndTag = new Random(100);
            //Получить очередное (в данном случае - первое) случайное число
            int tagNumber = rndTag.Next();

            Random rndType = new Random(4);
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
            object? result = null; 
            switch (type)
            {
                case TagTypeInfo.Int:
                    {
                        Random rndInt = new Random();
                        result = (object)rndInt.Next(0, 10000);
                    }
                    break;
                case TagTypeInfo.Float:
                    {
                        Random rndFloat = new Random();
                        result = rndFloat.Next(0, 10000) / 100;
                    }
                    break;
            }

            return result;
                      
        }
        public Task Execute(IJobExecutionContext context)
        {
            List<Signal> signals = GenerateValues(100);
             _clickHouseService.AddSignals(signals);
            return Task.CompletedTask;

        }

    }
}
