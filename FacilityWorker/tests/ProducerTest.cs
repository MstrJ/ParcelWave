using FluentAssertions;
using FacilityWorker.Services.Dto;
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
    [InlineData("18", 2f, 2f, 1f)]
    [Theory]
    public async Task TestRabbitScannerProducer(string upid,float width, float depth, float length)
    {
        // Arrange
        var dto = new ParcelScannerDTO(upid, width, depth, length);
        var content = dto.ToHttpContent();
        
        // Act
        var r = await _client.PostAsync("Facility/SendDimensions", content);
        r.EnsureSuccessStatusCode();
        var expectation = CreateExpectedParcelMessage.Create(upid, width, depth, length);
        var actual = await RabbitConsumer.ConsumeLatestMessageAsync();
            
        // Assert
        actual.Should().BeEquivalentTo(expectation);
    }
}