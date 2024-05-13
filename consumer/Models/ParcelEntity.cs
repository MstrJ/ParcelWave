using consumer.Models.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace consumer.Models;

public class ParcelEntity : IParcelEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _Id { get; set; }
    
    [BsonElement("Identifies")]
    public Identifies? Identifies { get; set; }
    [BsonElement("Attributes")]
    public Attributes? Attributes { get; set; }
    [BsonElement("CurrentState")]
    public CurrentState? CurrentState { get; set; }
}


public class Identifies
{
    [BsonElement("UPID")]
    public string UPID { get; set; }
    [BsonElement("Barcode")]
    public string Barcode { get; set; }
}

public class Attributes
{
    [BsonElement("Weight")]
    public float? Weight { get; set; }
    [BsonElement("Width")]
    public float? Width { get; set; }
    [BsonElement("Length")]
    public float? Length { get; set; }    
    [BsonElement("Depth")]
    public float? Depth { get; set; }
}

public class CurrentState
{ 
    [BsonElement("Facility")]
    public Facility? Facility { get; set; }
}

public enum Facility
{
    Ord,Wdr,Dfw,Atl
}