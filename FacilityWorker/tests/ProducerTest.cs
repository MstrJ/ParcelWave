using FluentAssertions;
using FacilityWorker.Models.Dto;
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
    [InlineData("18", 2f, -2f, 1f)]
    [Theory]
    public async Task TestRabbitScannerProducer(string upid,float width, float depth, float length)
    {
        var dto = new ParcelScannerDTO(upid, width, depth, length);
        var content = dto.ToHttpContent();
            
        var r = await _client.PostAsync("ParcelProducer/SendDimensions", content);
        r.EnsureSuccessStatusCode();
        var expectation = CreateExpectedParcelMessage.Create(upid, width, depth, length);
        var actual = await RabbitConsumer.Consume();
        
        actual.Should().BeEquivalentTo(expectation);
    }
}