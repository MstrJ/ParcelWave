using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ParcelProcessor.Repository.Dto;

public class ParcelEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _Id { get; set; }
    [BsonElement("Identifiers")]
    public Identifiers? Identifiers { get; set; }
    [BsonElement("Attributes")]
    public Attributes? Attributes { get; set; }
    [BsonElement("CurrentState")]
    public CurrentState? CurrentState { get; set; }
    
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
    
    public override bool Equals(object obj)
    {
        if (obj is Attributes other)
        {
            return Weight == other.Weight
                   && Width == other.Width
                   && Length == other.Length
                   && Depth == other.Depth;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Weight, Width, Length, Depth);
    }
}

public class CurrentState
{ 
    [BsonElement("Facility")]
    public Facility? Facility { get; set; }
    
    public override bool Equals(object obj)
    {
        if (obj is CurrentState other)
        {
            return Facility == other.Facility;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Facility);
    }
}

public enum Facility
{
    Ord,Dfw,Atl
}