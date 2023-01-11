using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "127.0.0.1" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "ImageProcess", durable: false, exclusive: false, autoDelete: false, arguments: null);
channel.QueueDeclare(queue: "ImageArchive", durable: false, exclusive: false, autoDelete: false, arguments: null);

channel.ExchangeDeclare(exchange: "image", type: "direct");

channel.QueueBind(queue: "ImageProcess", exchange: "image", routingKey: "crop");
channel.QueueBind(queue: "ImageProcess", exchange: "image", routingKey: "resize");
channel.QueueBind(queue: "ImageArchive", exchange: "image", routingKey: "resize");

int count = 0;

while (true)
{
    string message = $"Image process message! {count++}";

    var body1 = Encoding.UTF8.GetBytes($"{message} - crop");
    var body2 = Encoding.UTF8.GetBytes($"{message} - resize");

    channel.BasicPublish(exchange: "image", routingKey: "crop", basicProperties: null, body: body1);
    channel.BasicPublish(exchange: "image", routingKey: "resize", basicProperties: null, body: body2);

    Console.WriteLine($"[x] Sent: {message}");

    Thread.Sleep(100);
}