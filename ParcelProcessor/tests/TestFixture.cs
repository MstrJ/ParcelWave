using Microsoft.AspNetCore.Mvc.Testing;

namespace tests;

public class TestFixture<T> : IDisposable where T : class
{
    private readonly WebApplicationFactory<T> _factory;
    public HttpClient Client { get; }
    
    public TestFixture()
    {
        _factory = new WebApplicationFactory<T>();
        Client = _factory.CreateClient();
    }

    public void Dispose()
    {
        Client.Dispose();
        _factory.Dispose();
    }

}

// 

// 1 helper, mongo get by upid parcel repo., z uzyciu repo,
// 2 helper  kafka, consume latest
