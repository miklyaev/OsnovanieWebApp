using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaToRabbitMq.Exceptions
{
    public class RabbitMqException : Exception
    {
        public RabbitMqException(string message) : base(message) { }
    }
}
