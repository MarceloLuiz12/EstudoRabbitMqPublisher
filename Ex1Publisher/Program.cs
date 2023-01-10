using RabbitMQ.Client;
using System.Text;

namespace Ex1Publisher
{
    internal class Program
    {
        static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "127.0.0.1" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "Hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

            int count = 0;

            while (true)
            {
                string message = $"Olá Mundo! {count++}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "Hello",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($"[X] Sent: {message}");

                Thread.Sleep(100);
            }
        }
    }
}