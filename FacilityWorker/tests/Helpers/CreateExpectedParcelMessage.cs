using FacilityWorker.Models;

namespace tests.Helpers;

public abstract class CreateExpectedParcelMessage
{
    public static ParcelMessage Create(string upid, float width, float depth, float length)
    {
        var expectation = new ParcelMessage
        {
            Identifies = new Identifies
            {
                Barcode = null,
                UPID = upid
            },
            Attributes = new Attributes
            {
                Depth = depth,
                Width = width,
                Length = length,
                Weight = null
            },
            CurrentState = null
        };
        return expectation;
    }
}