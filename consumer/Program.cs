using consumer.Repositories;
using consumer.Repositories.Interfaces;
using consumer.Services;
using consumer.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Serilog;

class Program
{
    public static async Task Main(string[] args)
    {

        var builder = new ConfigurationBuilder();
        BuildConfig(builder);

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console().Enrich.FromLogContext()
            .CreateLogger();

        var configuration = builder.Build();
                        
        Log.Logger.Information("Application is starting");
        while (true)
        {
            var host = new HostBuilder().ConfigureServices((c, services) =>
            {
                services.AddSingleton<IConfiguration>(configuration);
                services.AddSingleton(Log.Logger);
                services.AddTransient<IParcelService, ParcelService>();
                services.AddTransient<ScannerStepValidator>();
                services.AddTransient<IKafkaProducerService, KafkaProducerService>();
                    
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
            
            await Task.Delay(5000);
        }
    }

    private static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT" ?? "Development")}.json", optional: true)
            .AddEnvironmentVariables();
    }
}
