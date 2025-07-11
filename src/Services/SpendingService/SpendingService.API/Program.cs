using Microsoft.EntityFrameworkCore;
using SpendingService.API.Services;
using SpendingService.Application.Handlers;
using SpendingService.Domain.Interfaces;
using SpendingService.Infrastructure.Db;
using SpendingService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add services
builder.Services.AddControllers();

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AddSpendingHandler>());

// ✅ Repository
builder.Services.AddScoped<ISpendingRepository, SpendingRepository>();

// ✅ Worker
builder.Services.AddHostedService<SpendingWorker>();

var app = builder.Build();

// ✅ Swagger + routing
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}
app.UseAuthorization();
app.MapControllers();

app.Run();
