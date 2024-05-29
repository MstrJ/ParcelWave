using MongoDB.Bson.Serialization.Attributes;
namespace ParcelProcessor.Models;

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