namespace Shared.A;

public static class Greeter
{
    public static string Greet(string name) =>
        string.IsNullOrWhiteSpace(name)
            ? "Hello, World!"
            : $"Hello, {name}!";

    public static string Farewell(string name) =>
        string.IsNullOrWhiteSpace(name)
            ? "Goodbye, World!"
            : $"Goodbye, {name}!";
}
