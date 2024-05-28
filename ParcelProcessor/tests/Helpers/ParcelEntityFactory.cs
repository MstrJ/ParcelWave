using ParcelProcessor.Repository.Dto;

namespace tests.Helpers;

public static class ParcelEntityFactory
{
    public static ParcelEntity Weight(string upid, float weight, string? id = null, string? barcode = null)
    {
        return new ParcelEntity
        {
            _Id = id,
            Identifiers = new Identifiers
            {
                UPID = upid,
                Barcode = barcode
            },
            Attributes = new Attributes
            {
                Weight = weight
            },
            CurrentState = new CurrentState()
        };
    }      
    
    public static ParcelEntity Facility(string upid,Facility facility , string? id = null, string? barcode = null)
    {
        return new ParcelEntity
        {
            _Id = id,
            Identifiers = new Identifiers
            {
                UPID = upid,
                Barcode = barcode
            },
            CurrentState = new CurrentState
            {
                Facility = facility
            }
        };
    }  
    
    public static ParcelEntity Dimensions(string upid, float width,float length, float depth,  string? id = null, string? barcode = null)
    {
        return new ParcelEntity
        {
            _Id = id,
            Identifiers = new Identifiers
            {
                UPID = upid,
                Barcode = barcode
            },
            Attributes = new Attributes
            {
                Width = width,
                Depth = depth,
                Length = length
            },
            CurrentState = new CurrentState()
        };
    }
    
    public static ParcelEntity Full(string upid, float weight, float width,float length, float depth,Facility facility, string? id = null, string? barcode = null)
    {
        return new ParcelEntity
        {
            _Id = id,
            Identifiers = new Identifiers
            {
                UPID = upid,
                Barcode = barcode
            },
            Attributes = new Attributes
            {
                Weight = weight,
                Width = width,
                Depth = depth,
                Length = length
            },
            CurrentState = new CurrentState
            {
                Facility = facility
            }
        };
    }
}