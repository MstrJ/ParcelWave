using FluentValidation;
using rabbit_producer.QueueSender;
using rabbit_producer.QueueSender.Interfaces;
using rabbit_producer.Services;
using rabbit_producer.Services.Interfaces;
using rabbit_producer.Validations;

var builder = WebApplication.CreateBuilder(args);
 
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
    .AddEnvironmentVariables();
    
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining(typeof(ParcelFacilityValidation));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(ParcelScannerValidation));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(ParcelWeightValidation));

builder.Services.AddTransient<IQueueSender, QueueSender>();
builder.Services.AddTransient<IParcelService, ParcelService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{ }