using API;
using API.Middleware;
using Application;
using Domain.Converters;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new CamelCaseEnumConverter());
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddAPI();

var app = builder.Build();

// check DB is up
{
    var maxRetries = 10;
    var delay = TimeSpan.FromSeconds(3);
    for (var i = 0; i < maxRetries; i++)
    {
        try
        {
            Console.Write($"Attempting database connection: {i + 1}/{maxRetries}: ");
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await db.Database.OpenConnectionAsync();
            await db.Database.CloseConnectionAsync();
            Console.WriteLine("Success");

            break;
        }
        catch (Exception)
        {
            if (i == maxRetries - 1)
                throw;

            await Task.Delay(delay);
        }
    }
}

// applying migrations automatically
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
