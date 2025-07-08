using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OCRService.Worker.Interfaces;
using OCRService.Worker.Processors;
using OCRService.Worker.Services;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IOcrProcessor, TesseractOcrProcessor>();
        services.AddHostedService<OcrWorker>();
    })
    .Build()
    .Run();
