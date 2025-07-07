using AuthService.Application.Commands;
using AuthService.Application.Handlers;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Security;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Identity Hashing
builder.Services.AddScoped<IPasswordHasher<AuthService.Domain.Entities.User>, PasswordHasher<AuthService.Domain.Entities.User>>();

// Token
builder.Services.AddScoped<ITokenService, TokenService>();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<RegisterUserCommandHandler>());


// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();
builder.Services.AddFluentValidationAutoValidation();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
});

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    dbContext.Database.Migrate();
}
app.Run();
