using Buildkite.Sdk;
using Buildkite.Sdk.Schema;

var misePlugin = new object[] { "mise#v1.1.1" };
var cache = ".buildkite/cache-volume";

object DownloadArtifacts(string step, string[] paths) =>
    new Dictionary<string, object>
    {
        ["artifacts#v1.9.4"] = new { download = paths, step },
    };

var pipeline = new Pipeline();

// Restore dependencies
pipeline.AddStep(new CommandStep
{
    Label = ":dotnet: Restore",
    Key = "restore",
    Command = "dotnet restore",
    ArtifactPaths = "src/**/obj/**;test/**/obj/**",
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
    ArtifactPaths = "src/**/bin/**;src/**/obj/**;test/**/bin/**;test/**/obj/**",
    Plugins = new object[]
    {
        "mise#v1.1.1",
        DownloadArtifacts("restore", ["src/**/obj/**", "test/**/obj/**"]),
    },
    Cache = cache,
});

// Test each project in a group
var testPlugins = new object[]
{
    "mise#v1.1.1",
    DownloadArtifacts("build", ["src/**/bin/**", "src/**/obj/**", "test/**/bin/**", "test/**/obj/**"]),
};

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
            Plugins = testPlugins,
            Cache = cache,
        },
        new CommandStep
        {
            Label = ":dotnet: App.A Tests",
            Command = "dotnet test test/App.A.Tests --no-build --configuration Release",
            Plugins = testPlugins,
            Cache = cache,
        },
        new CommandStep
        {
            Label = ":dotnet: App.B Tests",
            Command = "dotnet test test/App.B.Tests --no-build --configuration Release",
            Plugins = testPlugins,
            Cache = cache,
        },
    },
});

Console.WriteLine(pipeline.ToYaml());
