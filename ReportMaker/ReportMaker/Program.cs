using MongoDB.Driver;
using ReportMaker.Repositories;
using ReportMaker.Repositories.Interfaces;
using ReportMaker.Services;
using ReportMaker.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT" ?? "Development")}.json",
        optional: true)
    .AddEnvironmentVariables().Build();

builder.Services.AddTransient<IParcelRepository, ParcelRepository>();
builder.Services.AddTransient<IPdfService, PdfService>();
builder.Services.AddTransient<IPlotService, PlotService>();
builder.Services.AddTransient<IReportService, ReportService>();

builder.Services.AddSingleton<IMongoClient, MongoClient>(x =>
{
    var uri = config["MongoUri"];
    var client = new MongoClient(uri);
    return client;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
    
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
    
app.UseAuthorization();

app.MapControllers();

app.Run();