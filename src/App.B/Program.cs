using Shared.A;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Greeter.Greet("App.B"));
app.MapGet("/bye", () => Greeter.Farewell("App.B"));

app.Run();

public partial class Program { }
