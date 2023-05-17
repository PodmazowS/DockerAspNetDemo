using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace DockerAspNetDemo.Services
{
    public class RabbitMQMessageProducer : IMessageProducer
    {
        public static readonly string QueueName = "statistics";
        public ConnectionFactory _factory;

        public RabbitMQMessageProducer()
        {
            _factory = new ConnectionFactory() { HostName = "localhost"};
        }
        public void SendMessage<T>(T message)
        {
            var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(QueueName, exclusive: false);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: QueueName, body: body);
        }
    }
}
