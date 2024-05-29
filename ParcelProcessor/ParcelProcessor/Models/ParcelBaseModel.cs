using MongoDB.Bson.Serialization.Attributes;
namespace ParcelProcessor.Models;
public class ParcelBaseModel
{
    [BsonElement("Attributes")]
    public Attributes? Attributes { get; set; }
    [BsonElement("CurrentState")]
    public CurrentState? CurrentState { get; set; }
}