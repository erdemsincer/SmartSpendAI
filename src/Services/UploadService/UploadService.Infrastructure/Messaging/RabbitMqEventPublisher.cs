using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using UploadService.Application.Common.Messaging;

namespace UploadService.Infrastructure.Messaging;

public class RabbitMqEventPublisher : IEventPublisher
{
    private readonly IConnection _connection;
    private readonly RabbitMQ.Client.IModel _channel;

    public RabbitMqEventPublisher()
    {
        var factory = new ConnectionFactory { HostName = "rabbitmq" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public Task PublishAsync<T>(T @event, string queueName)
    {
        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
        _channel.BasicPublish(exchange: "", routingKey: queueName, body: body);

        return Task.CompletedTask;
    }
}
