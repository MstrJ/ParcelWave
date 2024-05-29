using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ParcelProcessor.Models;

namespace ParcelProcessor.Repository.Dto;

public class ParcelEntity : ParcelBaseModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _Id { get; set; }
    [BsonElement("Identifiers")]
    public Identifiers? Identifiers { get; set; }

    
    public override bool Equals(object obj)
    {
        if (obj is ParcelEntity other)
        {
            return _Id == other._Id
                   && Equals(Identifiers, other.Identifiers)
                   && Equals(Attributes, other.Attributes)
                   && Equals(CurrentState, other.CurrentState);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_Id, Identifiers, Attributes, CurrentState);
    }
}


public class Identifiers
{
    [BsonElement("UPID")]
    public string UPID { get; set; }
    [BsonElement("Barcode")]
    public string Barcode { get; set; }
    
    public override bool Equals(object obj)
    {
        if (obj is Identifiers other)
        {
            return UPID == other.UPID
                   && Barcode == other.Barcode;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(UPID, Barcode);
    }
}



