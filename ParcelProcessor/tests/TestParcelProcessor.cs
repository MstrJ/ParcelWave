using FluentAssertions;
using ParcelProcessor.Repository.Dto;
using tests.Helpers;

namespace tests;

public class TestParcelProcessor : IClassFixture<TestFixture>
{
    [InlineData("ad", 5)]
    [Theory]
    public async Task Scale_ParcelMessageEqualToParcelEntity(string upid, float weight)
    {
        // Arrange
        var parcelMessage = ParcelMessageFactory.Weight(upid, weight);
        
        // Act
        await TestFixture.RabbitProducerHelper.Send(parcelMessage);
        
        // Assert
        await TestFixture.MongoHelper.WaitForMongoParcelAndVerify(upid, parcel =>
        {
            var expected = ParcelEntityFactory.Weight(upid, weight);

            parcel._Id.Should().NotBeNullOrEmpty();
            expected._Id = parcel._Id;
            parcel.Identifiers?.Barcode.Should().NotBeNullOrEmpty();
            expected.Identifiers.Barcode = parcel.Identifiers.Barcode;
                
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
        var parcelMessage = ParcelMessageFactory.Weight(upid, weight);
        
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
        var parcelMessage = ParcelMessageFactory.Dimensions(upid, width, length, depth);
        
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
        var parcelMessage = ParcelMessageFactory.Facility(upid,facility);
        
        // Act
        await TestFixture.RabbitProducerHelper.Send(parcelMessage);
            
        // Assert
        await TestFixture.MongoHelper.WaitForMongoParcelAndVerify(upid,actual => actual.Should().BeNull());
    }

    [InlineData("upidd213", 5, 4, 3, 2, 1)]
    [Theory]
    public async Task SendParcelMessage_And_VerifyInMongo_And_Kafka(string upid, float weight, float width, float length, float depth, int facilityIndex)
    {
        // Arrange
        var facility = (Facility)facilityIndex;
        var parcelMessage = ParcelMessageFactory.Full(upid, weight, width, length, depth, facility);

        // Act
        await TestFixture.RabbitProducerHelper.Send(parcelMessage);
        
        // Assert
        await TestFixture.MongoHelper.WaitForMongoParcelAndVerify(upid, actual =>
        {
            var expected = ParcelEntityFactory.Full(upid, weight, width, length, depth, facility);
            
            actual._Id.Should().NotBeNullOrEmpty();
            expected._Id = actual._Id;
            
            actual.Identifiers?.Barcode.Should().NotBeNullOrEmpty();
            expected.Identifiers!.Barcode = actual.Identifiers!.Barcode;
            
            actual.Should().BeEquivalentTo(expected);
        });
        var kafkaMessage = TestFixture.KafkaConsumeHelper.ConsumeLatestMessage(10);
        kafkaMessage.Should().NotBeNull();
    }

    [InlineData("dsad", 0, -1, 0, 2.5f, 10)]
    [InlineData("upid", 0, 0, 0, 0, 0)]
    [Theory]
    public async Task ParcelMessage_FullShouldBeNullAndKafkaReceiveNullAfter2Seconds(string upid,float weight,float width,float lenght,float depht, int facilityIndex)
    {
        // Arrange
        var facility = (Facility)facilityIndex;
        var parcelMessage = ParcelMessageFactory.Full(upid, weight, width, lenght, depht, facility);
        
        // Act
        await TestFixture.RabbitProducerHelper.Send(parcelMessage);
        var kafkaMessage = TestFixture.KafkaConsumeHelper.ConsumeLatestMessage(2);
        
        // Assert
        await TestFixture.MongoHelper.WaitForMongoParcelAndVerify(upid,actual => actual.Should().BeNull());
        kafkaMessage.Should().BeNull();
    }
}