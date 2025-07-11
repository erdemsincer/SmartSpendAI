﻿using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UploadService.Application.Commands;
using UploadService.Application.Commands;
using UploadService.Application.Common.Messaging;
using UploadService.Application.Handlers;
using UploadService.Domain.Interfaces;
using UploadService.Infrastructure.Messaging;
using UploadService.Infrastructure.Persistence;
using UploadService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<UploadDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<UploadReceiptCommandHandler>());

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<UploadReceiptCommandValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Upload API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UploadDbContext>();
    db.Database.Migrate(); 
}

app.Run();
