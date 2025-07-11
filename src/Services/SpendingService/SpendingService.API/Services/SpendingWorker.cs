using AIClassifierService.Shared.Events;
using MediatR;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SpendingService.Application.Commands;
using SpendingService.Shared.Events;
using System.Text;
using System.Text.Json;

namespace SpendingService.API.Services;

public class SpendingWorker : BackgroundService
{
    private readonly ILogger<SpendingWorker> _logger;
    private readonly IMediator _mediator;
    private IConnection _connection;
    private IModel _channel;

    public SpendingWorker(ILogger<SpendingWorker> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;

        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "receipt_classified", durable: true, exclusive: false, autoDelete: false);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var @event = JsonSerializer.Deserialize<ReceiptClassifiedEvent>(json);

            if (@event != null)
            {
                _logger.LogInformation("📥 Event geldi: {ReceiptId} - {Category}", @event.ReceiptId, @event.Category);

                var command = new AddSpendingCommand
                {
                    UserId = @event.UserId, // Burayı event'e ekleyeceğiz birazdan
                    ReceiptId = @event.ReceiptId,
                    Category = @event.Category,
                    Amount = @event.Amount,
                    Timestamp = @event.ClassifiedAt
                };

                await _mediator.Send(command);
                _logger.LogInformation("✅ Harcama DB'ye kaydedildi.");
            }
        };

        _channel.BasicConsume(queue: "receipt_classified", autoAck: true, consumer: consumer);
        _logger.LogInformation("🔁 receipt_classified kuyruğu dinleniyor...");

        return Task.CompletedTask;
    }
}
