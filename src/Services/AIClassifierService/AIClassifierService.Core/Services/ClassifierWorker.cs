using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AIClassifierService.Shared.Events;
using AIClassifierService.Core.Interfaces;

namespace AIClassifierService.API.Services;

public class ClassifierWorker : BackgroundService
{
    private readonly ILogger<ClassifierWorker> _logger;
    private readonly IReceiptClassifier _classifier;
    private IConnection _connection;
    private IModel _channel;

    public ClassifierWorker(ILogger<ClassifierWorker> logger, IReceiptClassifier classifier)
    {
        _logger = logger;
        _classifier = classifier;

        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "parsed_receipt", durable: true, exclusive: false, autoDelete: false);
        _channel.QueueDeclare(queue: "receipt_classified", durable: true, exclusive: false, autoDelete: false);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var @event = JsonSerializer.Deserialize<ParsedReceiptEvent>(json);

            if (@event is not null)
            {
                var category = _classifier.PredictCategory(@event.Merchant, @event.TotalAmount);
                _logger.LogInformation("📊 Tahmin edildi: {Merchant} → {Category}", @event.Merchant, category);

                var classified = new ReceiptClassifiedEvent
                {
                    ReceiptId = @event.ReceiptId,
                    Category = category,
                    ClassifiedAt = DateTime.UtcNow
                };

                var payload = JsonSerializer.Serialize(classified);
                var bytes = Encoding.UTF8.GetBytes(payload);

                _channel.BasicPublish(exchange: "", routingKey: "receipt_classified", body: bytes);
                _logger.LogInformation("✅ receipt_classified event gönderildi.");
            }
        };

        _channel.BasicConsume(queue: "parsed_receipt", autoAck: true, consumer: consumer);
        _logger.LogInformation("🔁 parsed_receipt kuyruğu dinleniyor...");

        return Task.CompletedTask;
    }
}
