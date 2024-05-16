using System.Text;
using Newtonsoft.Json;

namespace tests.Helpers;

public static class StringContentExtension
{
    public static StringContent ToHttpContent(this object obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        return data;
    }
}