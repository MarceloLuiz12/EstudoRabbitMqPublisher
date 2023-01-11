
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "127.0.0.1" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
{
    channel.QueueDeclare(queue: "Fila1", durable: false, exclusive: false, autoDelete: false, arguments: null);
    channel.QueueDeclare(queue: "Fila2", durable: false, exclusive: false, autoDelete: false, arguments: null);

    channel.ExchangeDeclare(exchange: "logs", type: "fanout");

    channel.QueueBind(queue: "text", exchange: "logs", "");
    channel.QueueBind(queue: "external", exchange: "logs", "");

    int count = 0;

    while (true)
    {
        string message = $"Fanout message! {count++}";

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "logs", routingKey: "", basicProperties: null, body: body);

        Console.WriteLine($"[x] Sent: {message}");
        Thread.Sleep(100);
    }
}