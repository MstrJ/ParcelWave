using ParcelProcessor.Models.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ParcelProcessor.Models;

public class ParcelMessage
{
    public Identifies? Identifies { get; set; }
    public Attributes? Attributes { get; set; }
    public CurrentState? CurrentState { get; set; }
}
