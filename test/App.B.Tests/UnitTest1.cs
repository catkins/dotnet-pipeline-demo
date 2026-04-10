namespace App.B.Tests;

using Microsoft.AspNetCore.Mvc.Testing;

public class EndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public EndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Root_ReturnsGreeting()
    {
        var client = _factory.CreateClient();
        var response = await client.GetStringAsync("/");
        Assert.Equal("Hello, App.B!", response);
    }

    [Fact]
    public async Task Bye_ReturnsFarewell()
    {
        var client = _factory.CreateClient();
        var response = await client.GetStringAsync("/bye");
        Assert.Equal("Goodbye, App.B!", response);
    }
}
