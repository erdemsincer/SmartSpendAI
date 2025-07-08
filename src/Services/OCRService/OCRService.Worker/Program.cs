using Microsoft.EntityFrameworkCore;
using OCRService.Infrastructure;
using OCRService.Infrastructure.Persistence;
using OCRService.Worker.Interfaces;
using OCRService.Worker.Messaging;
using OCRService.Worker.Processors;
using OCRService.Worker.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IOcrProcessor, TesseractOcrProcessor>();
        services.AddHostedService<OcrWorker>();

        services.AddInfrastructure(context.Configuration); // DI + DbContext burada
        services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();

    })
    .Build();

// 🚀 Otomatik Migration
using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OCRDbContext>();
    db.Database.Migrate();
}

host.Run();
