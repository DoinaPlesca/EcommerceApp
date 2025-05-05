using EcommerceApp.Configuration;
using EcommerceApp.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));
builder.Services.AddSingleton<MongoService>();

builder.Services.AddMediatR(typeof(Program));

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddSingleton<CloudinaryService>();

var redisHost = builder.Configuration["Redis:Host"] ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(redisHost));
builder.Services.AddSingleton<RedisCacheService>();

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();