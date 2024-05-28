using FluentValidation;
using ParcelProcessor.Services;
using ParcelProcessor.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using ParcelProcessor.Communication.Rabbit;
using ParcelProcessor.Communications.Kafka;
using ParcelProcessor.Communications.Rabbit;
using ParcelProcessor.Repository;
using ParcelProcessor.Repository.Interfaces;
using ParcelProcessor.Services.Interfaces;
using Serilog;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder();
        BuildConfig(builder);

        var configuration = builder.Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
                        
        Log.Logger.Information("Application is starting");
        
        var host = new HostBuilder().ConfigureServices((c, services) =>
        {
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton(Log.Logger);
            services.AddValidatorsFromAssemblyContaining(typeof(ScannerStepValidator));
            services.AddValidatorsFromAssemblyContaining(typeof(ScaleStepValidator));
            services.AddValidatorsFromAssemblyContaining(typeof(ParcelMessageIdentifiesValidator));
            services.AddValidatorsFromAssemblyContaining(typeof(FacilityStepValidator));
            services.AddTransient<IParcelService, ParcelService>();
            services.AddTransient<INetworkNotifier, KafkaProducerService>();
            services.AddSingleton<IValidatorService, ValidatorService>();
            services.AddSingleton<IParcelRepository, ParcelRepository>();
            services.AddSingleton<IRabbitConsumerService, RabbitConsumerService>();
            services.AddSingleton<IMongoClient, MongoClient>(x =>
            {
                var uri = configuration["MongoUri"];
                var client = new MongoClient(uri);
                return client;
            });
                
        }).UseSerilog().Build();
        
        var rabbitConsumerService = host.Services.GetRequiredService<IRabbitConsumerService>();
        
        await rabbitConsumerService.ConsumeParcel();
            
        await host.RunAsync();
    }
    
    private static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT" ?? "Development")}.json", optional: true)
            .AddEnvironmentVariables();
    }
}
