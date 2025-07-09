using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OCRService.Shared.Events;
using ParserService.Core.Entities;
using ParserService.Core.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ParserService.API.Services;

public class ParserWorker : BackgroundService
{
    private readonly ILogger<ParserWorker> _logger;
    private readonly IReceiptParser _parser;
    private readonly IServiceScopeFactory _scopeFactory;
    private IConnection _connection;
    private IModel _channel;

    public ParserWorker(ILogger<ParserWorker> logger, IReceiptParser parser, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _parser = parser;
        _scopeFactory = scopeFactory;

        var factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "ocr_processed", durable: true, exclusive: false, autoDelete: false);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var @event = JsonSerializer.Deserialize<OcrProcessedEvent>(message);

                if (@event is not null)
                {
                    _logger.LogInformation("📥 OCR sonucu alındı: {FileId}", @event.FileId);

                    var parsed = _parser.Parse(@event.FileId, @event.UserId, @event.RawText);

                    using var scope = _scopeFactory.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IParsedReceiptRepository>();

                    await repository.AddAsync(parsed);

                    _logger.LogInformation("📊 Fiş DB’ye yazıldı: {Merchant} - {TotalAmount}₺", parsed.Merchant, parsed.TotalAmount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ OCR sonucu işlenirken hata oluştu");
            }
        };

        _channel.BasicConsume(queue: "ocr_processed", autoAck: true, consumer: consumer);
        _logger.LogInformation("🔁 ParserWorker ocr_processed kuyruğunu dinliyor...");
        return Task.CompletedTask;
    }
}
