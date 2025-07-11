using AIClassifierService.API.Services;
using AIClassifierService.Core.Interfaces;
using AIClassifierService.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IReceiptClassifier, MockReceiptClassifier>();
builder.Services.AddHostedService<ClassifierWorker>();

var app = builder.Build();
app.Run();
