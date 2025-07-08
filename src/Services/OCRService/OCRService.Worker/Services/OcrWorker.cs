using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OCRService.Shared.Events;
using OCRService.Worker.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace OCRService.Worker.Services;

public class OcrWorker : BackgroundService
{
    private readonly IOcrProcessor _ocrProcessor;
    private readonly ILogger<OcrWorker> _logger;
    private IConnection _connection;
    private IModel _channel;

    public OcrWorker(IOcrProcessor ocrProcessor, ILogger<OcrWorker> logger)
    {
        _ocrProcessor = ocrProcessor;
        _logger = logger;

        var factory = new ConnectionFactory()
        {
            HostName = "rabbitmq", // 🐇 Docker'daki servis adı
            Port = 5672
        };

        int retries = 5;
        while (retries > 0)
        {
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(queue: "file_uploaded", durable: true, exclusive: false, autoDelete: false, arguments: null);
                _logger.LogInformation("✅ RabbitMQ bağlantısı kuruldu ve kuyruk tanımlandı.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("⏳ RabbitMQ bağlantısı kurulamadı. Tekrar deneniyor... ({Retries} kaldı) Hata: {Error}", retries, ex.Message);
                retries--;
                Thread.Sleep(3000);
            }
        }

        if (_connection == null || _channel == null)
        {
            throw new Exception("❌ RabbitMQ bağlantısı sağlanamadı. Worker başlatılamıyor.");
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            var @event = JsonSerializer.Deserialize<FileUploadedEvent>(message);

            if (@event is not null)
            {
                _logger.LogInformation("📥 Event alındı: {FilePath}", @event.FilePath);
                var result = await _ocrProcessor.ProcessImageAsync(@event.FilePath);
                _logger.LogInformation("🧠 OCR sonucu (FileId: {@event.FileId}):\n{Text}", @event.FileId, result);
            }
        };

        _channel.BasicConsume(queue: "file_uploaded", autoAck: true, consumer: consumer);
        _logger.LogInformation("🔁 RabbitMQ dinleniyor...");

        return Task.CompletedTask;
    }
}
