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