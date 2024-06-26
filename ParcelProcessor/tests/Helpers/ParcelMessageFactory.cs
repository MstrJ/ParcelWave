using ParcelProcessor.Communications.Rabbit.Dto;
using ParcelProcessor.Models;

namespace tests.Helpers;

public static class ParcelMessageFactory
{
    public static ParcelMessage Weight(string upid, float weight)
    {
        return new ParcelMessage
        {
            UPID = upid,
            Attributes = new Attributes { Weight = weight },
            CurrentState = new CurrentState()
        };
    }    
    
    public static ParcelMessage Dimensions(string upid, float width,float length,float depth)
    {
        return new ParcelMessage
        {
            UPID = upid,
            Attributes = new Attributes { Length = length, Depth = depth, Width = width },
            CurrentState = new CurrentState()
        };
    }

    public static ParcelMessage Facility(string upid, Facility facility)
    {
        return new ParcelMessage
        {
            UPID = upid,
            CurrentState = new CurrentState { Facility = facility }
        };
    }
    
    public static ParcelMessage Full(string upid,float weight, float width,float lenght,float depth, Facility facility)
    {
        return new ParcelMessage
        {
            UPID = upid,
            Attributes = new Attributes { Weight = weight, Length = lenght, Depth = depth, Width = width },
            CurrentState = new CurrentState { Facility = facility }
        };
    }
}