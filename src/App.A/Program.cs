using Shared.A;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Greeter.Greet("App.A"));
app.MapGet("/bye", () => Greeter.Farewell("App.A"));

app.Run();

public partial class Program { }
