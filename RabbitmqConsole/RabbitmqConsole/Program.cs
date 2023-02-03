using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitmqConsole
{
    internal class Program
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "192.168.0.104", UserName = "admin", Password = "130469" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //channel.QueueDeclare(queue: "Queue",
                //                     durable: false,
                //                     exclusive: false,
                //                     autoDelete: false,
                //                     arguments: null);
                
                //конфигурация рабита один продюсер - несколько консюмеров. Консюмера одновременно получают одинаковые сообщения
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                string message = "Hello World - 7";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "logs",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to read from RabbitMq");
            Console.ReadLine();
        }
    }
}
