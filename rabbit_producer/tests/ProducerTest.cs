using FluentAssertions;
using rabbit_producer.Models;
using rabbit_producer.Models.Dto;
using tests.Helpers;

namespace tests;

public class ProducerTest : IClassFixture<TestFixture<Program>>
{
    private readonly HttpClient _client;
    
    public ProducerTest(TestFixture<Program> fixture)
    {
        _client = fixture.Client;
    }
    
    [InlineData("20dx", 2f, 1.5f, 2f)]    
    [InlineData("15", 5f, 0.5f, 1f)]
    [InlineData("18", -5, 0, 1f)]
    [Theory]
    public async Task TestRabbitScannerProducer(string upid,float width, float depth, float length)
    {
        var dto = new ParcelScannerDTO(upid, width, depth, length);
        var content = dto.ToHttpContent();
        
        await _client.PostAsync("ParcelProducer/SendWithScannerValues", content);
        
        var expectation = new ParcelMessage
        {
            Identifies = new Identifies
            {
                Barcode = null,
                UPID = upid
            },
            Attributes = new Attributes
            {
                Depth = depth,
                Width = width,
                Length = length,
                Weight = null
            },
            CurrentState = null
        };
            
        
        var actual = await RabbitConsumer.Consume();
        
        actual.Should().BeEquivalentTo(expectation);
 
    }
}