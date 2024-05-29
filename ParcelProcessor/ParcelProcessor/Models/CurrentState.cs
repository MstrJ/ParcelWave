using MongoDB.Bson.Serialization.Attributes;
namespace ParcelProcessor.Models;

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
