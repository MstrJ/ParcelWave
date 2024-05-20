using FluentAssertions;
using ParcelProcessor.Models;
using tests.Helpers;

namespace tests;

public class TestParcelProcessor : IClassFixture<TestFixture>
{
    // private MongoHelper _mongoHelper;
    // private RabbitProducerHelper _rabbitProducerHelper;
    // private KafkaConsumeHelper _kafkaConsumeHelper;
    //
    // private TestParcelProcessor(TestFixture fixture)
    // {
    //     _mongoHelper = fixture.MongoHelper;
    //     _rabbitProducerHelper = fixture.RabbitProducerHelper;
    //     _kafkaConsumeHelper = fixture.KafkaConsumeHelper;
    //     
    // }
    //
    
    [InlineData("ad", 5)]
    [Theory]
    public async Task Scale_ParcelMessageEqualToParcelEntity(string upid, float weight)
    {
        // Arrange
        var parcelMessage = CreateParcelMessage.Weight(upid, weight);
        
        // Act
        await TestFixture.RabbitProducerHelper.Send(parcelMessage);
        
        // Assert
        await TestFixture.MongoHelper.WaitForMongoParcelAndVerify(upid, parcel =>
        {
            var expected = CreateParcelEntity.Weight(upid, weight);

            parcel._Id.Should().NotBeNullOrEmpty();
            expected._Id = parcel._Id;
            parcel.Identifies?.Barcode.Should().NotBeNullOrEmpty();
            expected.Identifies.Barcode = parcel.Identifies.Barcode;
                
            parcel.Should().BeEquivalentTo(expected);
        });
    }
    
    [InlineData("", 10)]
    [InlineData("fsdfbf", -5)]
    [InlineData("cdfsdg1", 0)]
    [Theory]
    public async Task Scale_ParcelExpectedShouldBeNull(string upid, float weight)
    {
        // Arrange
        var parcelMessage = CreateParcelMessage.Weight(upid, weight);
        
        // Act
        await TestFixture.RabbitProducerHelper.Send(parcelMessage);
            
        // Assert
        await TestFixture.MongoHelper.WaitForMongoParcelAndVerify(upid, actual =>  actual.Should().BeNull());

    }
    
    [InlineData("upid4",-5,4,2)] 
    [InlineData("upid2",0,1,2)] 
    [InlineData("upidd",0,0,0)] 
    [Theory]
    public async Task Scanner_ParcelExpectedShouldBeNull(string upid, float width,float length,float depth)
    {
        // Arrange
        var parcelMessage = CreateParcelMessage.Dimensions(upid, width, length, depth);
        
        // Act
        await TestFixture.RabbitProducerHelper.Send(parcelMessage);
            
        // Assert
        await TestFixture.MongoHelper.WaitForMongoParcelAndVerify(upid, actual => actual.Should().BeNull());
    }
    
    [InlineData("upidd",10)]
    [InlineData("dsad",5)]
    [InlineData("fdsfsd",-1)]
    [InlineData("fdsfsd",6)]
    [Theory]
    public async Task Facility_ParcelExpectedShouldBeNull(string upid,int facilityIndex)
    {
        // Arrange
        var facility = (Facility)facilityIndex;
        var parcelMessage = CreateParcelMessage.Facility(upid,facility);
        
        // Act
        await TestFixture.RabbitProducerHelper.Send(parcelMessage);
            
        // Assert
        await TestFixture.MongoHelper.WaitForMongoParcelAndVerify(upid,actual => actual.Should().BeNull());
    }

    [InlineData("dasd", 5, 4, 3, 2, 1)]
    [Theory]
    public async Task ParcelMessage_FullShouldNotBeNull(string upid, float weight, float width, float lenght,
        float depht, int facilityIndex)
    {
        // Arrange
        var facility = (Facility)facilityIndex;
        var parcelMessage = CreateParcelMessage.Full(upid, weight, width, lenght, depht, facility);

        // Act
        await TestFixture.RabbitProducerHelper.Send(parcelMessage);

        // Assert
        await TestFixture.MongoHelper.WaitForMongoParcelAndVerify(upid, actual =>
        {
            var expected = CreateParcelEntity.Full(upid, weight, width, lenght, depht, facility);
            
            actual._Id.Should().NotBeNullOrEmpty();
            expected._Id = actual._Id;
            
            actual.Identifies?.Barcode.Should().NotBeNullOrEmpty();
            expected.Identifies.Barcode = actual.Identifies.Barcode;
            
            actual.Should().BeEquivalentTo(expected);
        });
    }


    [InlineData("dsad", 0, -1, 0, 2.5f, 10)]
    [InlineData("upid", 0, 0, 0, 0, 0)]
    [Theory]
    public async Task ParcelMessage_FullShouldBeNullAndKafkaReceiveNullAfter5Seconds(string upid,float weight,float width,float lenght,float depht, int facilityIndex)
    {
        // Arrange
        var facility = (Facility)facilityIndex;
        var parcelMessage = CreateParcelMessage.Full(upid, weight, width, lenght, depht, facility);
        
        // Act
        await TestFixture.RabbitProducerHelper.Send(parcelMessage);
        var kafkaMessage = TestFixture.KafkaConsumeHelper.ConsumeLatestMessage(5);
        
        // Assert
        await TestFixture.MongoHelper.WaitForMongoParcelAndVerify(upid,actual => actual.Should().BeNull());
        kafkaMessage.Should().BeNull();
    }
}