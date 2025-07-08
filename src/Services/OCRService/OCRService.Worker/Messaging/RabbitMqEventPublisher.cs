using System.Text;
using System.Text.Json;
using OCRService.Worker.Interfaces;
using RabbitMQ.Client;

namespace OCRService.Worker.Messaging;

public class RabbitMqEventPublisher : IEventPublisher
{
    private readonly IConnection _connection;

    public RabbitMqEventPublisher()
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
        _connection = factory.CreateConnection();
    }

    public Task PublishAsync<T>(T @event, string queueName)
    {
        using var channel = _connection.CreateModel();
        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

        var json = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        return Task.CompletedTask;
    }
}
