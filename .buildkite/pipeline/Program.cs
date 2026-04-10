using Buildkite.Sdk;
using Buildkite.Sdk.Schema;

var misePlugin = new object[] { "mise#v1.1.1" };
var cache = ".buildkite/cache-volume";

var pipeline = new Pipeline();

// Restore dependencies
pipeline.AddStep(new CommandStep
{
    Label = ":dotnet: Restore",
    Key = "restore",
    Command = "dotnet restore",
    Plugins = misePlugin,
    Cache = cache,
});

// Build all projects
pipeline.AddStep(new CommandStep
{
    Label = ":hammer: Build",
    Key = "build",
    Command = "dotnet build --configuration Release --no-restore",
    DependsOn = new[] { "restore" },
    Plugins = misePlugin,
    Cache = cache,
});

// Test each project in a group
pipeline.AddStep(new GroupStep
{
    Group = ":test_tube: Tests",
    DependsOn = new[] { "build" },
    Steps = new List<IGroupStep>
    {
        new CommandStep
        {
            Label = ":dotnet: Shared.A Tests",
            Command = "dotnet test test/Shared.A.Tests --no-build --configuration Release",
            Plugins = misePlugin,
            Cache = cache,
        },
        new CommandStep
        {
            Label = ":dotnet: App.A Tests",
            Command = "dotnet test test/App.A.Tests --no-build --configuration Release",
            Plugins = misePlugin,
            Cache = cache,
        },
        new CommandStep
        {
            Label = ":dotnet: App.B Tests",
            Command = "dotnet test test/App.B.Tests --no-build --configuration Release",
            Plugins = misePlugin,
            Cache = cache,
        },
    },
});

Console.WriteLine(pipeline.ToYaml());
