using EcommerceApp.Configuration;
using EcommerceApp.Middleware;
using EcommerceApp.Services;
using FluentValidation.AspNetCore;
using MediatR;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));
builder.Services.AddSingleton<MongoService>();

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddSingleton<CloudinaryService>();

var redisHost = builder.Configuration["Redis:Host"] ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(redisHost));
builder.Services.AddSingleton<RedisCacheService>();

builder.Services.AddMediatR(typeof(Program).Assembly);

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Ecommerce API", Version = "v1" });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();