
namespace ParcelProcessor.Models;

public static class ParcelExtensions
{
    public static bool IsReady(this object obj)
    {
        if (obj == null) return false;

        foreach (var prop in obj.GetType().GetProperties())
        {
            var value = prop.GetValue(obj, null);

            if (value == null) return false;
            
            if (!prop.PropertyType.IsPrimitive && prop.PropertyType != typeof(string) && !prop.PropertyType.IsEnum)
            {
                if (!IsReady(value)) return false;
            }
        }

        return true;
    }
    
}