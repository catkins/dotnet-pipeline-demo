namespace Shared.A.Tests;

public class GreeterTests
{
    [Fact]
    public void Greet_WithName_ReturnsPersonalizedGreeting()
    {
        var result = Greeter.Greet("Buildkite");
        Assert.Equal("Hello, Buildkite!", result);
    }

    [Fact]
    public void Greet_WithEmpty_ReturnsDefaultGreeting()
    {
        var result = Greeter.Greet("");
        Assert.Equal("Hello, World!", result);
    }

    [Fact]
    public void Farewell_WithName_ReturnsPersonalizedFarewell()
    {
        var result = Greeter.Farewell("Buildkite");
        Assert.Equal("Goodbye, Buildkite!", result);
    }

    [Fact]
    public void Farewell_WithEmpty_ReturnsDefaultFarewell()
    {
        var result = Greeter.Farewell("");
        Assert.Equal("Goodbye, World!", result);
    }
}
