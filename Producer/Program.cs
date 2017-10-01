using System;
using System.Text;
using RabbitMQ.Client;

namespace Producer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = AmqpTcpEndpoint.UseDefaultPort
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("hello", false, false, false, null);

                var acceptingMessages = true;
                while (acceptingMessages)
                {
                    var message = Console.ReadLine() ?? string.Empty;
                    if (string.Equals(message, "exit", StringComparison.CurrentCultureIgnoreCase))
                    {
                        acceptingMessages = false;
                    }
                    else
                    {
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(string.Empty, "hello", null, body);
                        Console.WriteLine(" [x] Sent {0}", message);
                    }
                }
            }
        }
    }
}