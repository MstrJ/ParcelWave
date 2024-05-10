using rabbit_producer.Repositories;
using rabbit_producer.Repositories.Interfaces;
using rabbit_producer.Services;
using rabbit_producer.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
    
builder.Services.AddTransient<IParcelRepository, ParcelRepository>();
builder.Services.AddTransient<IParcelService, ParcelService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();