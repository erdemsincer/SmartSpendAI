using Microsoft.EntityFrameworkCore;
using ParserService.API.Services;
using ParserService.Core.Interfaces;
using ParserService.Core.Services;
using ParserService.Infrastructure;
using ParserService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Add services to the container BEFORE builder.Build()

// Swagger + Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<ParserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI
builder.Services.AddSingleton<IReceiptParser, SimpleReceiptParser>();
builder.Services.AddScoped<IParsedReceiptRepository, ParsedReceiptRepository>();
builder.Services.AddHostedService<ParserWorker>();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ParserDbContext>();
    db.Database.Migrate(); // ➕ Burada otomatik migration uygulanır
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
