namespace consumer.Models.Interfaces;

public interface IParcelEntity
{
    Identifies? Identifies { get; set; }
    Attributes? Attributes { get; set; }
    CurrentState? CurrentState { get; set; }
}