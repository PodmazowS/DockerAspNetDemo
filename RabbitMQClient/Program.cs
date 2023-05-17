using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQClient
{
    internal class Program
    {
        private readonly static string QueueName = "statistics";

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(QueueName, exclusive: false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, args) =>
            {
                var body = args.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject(json);
                Console.WriteLine($"Received message: {message}");
            };
            channel.BasicConsume(QueueName, autoAck: true, consumer: consumer);
            Console.ReadKey();
        }
    }
}