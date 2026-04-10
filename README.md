# dotnet-pipeline-demo

A demo .NET multi-project solution with a [Buildkite](https://buildkite.com) pipeline defined dynamically using the [C# Buildkite SDK](https://buildkite.com/docs/pipelines/configure/dynamic-pipelines/sdk#c-sharp).

## Project structure

```
src/
├── Shared.A/          # Shared class library
├── App.A/             # Web app using Shared.A
└── App.B/             # Web app using Shared.A
test/
├── Shared.A.Tests/    # Unit tests for Shared.A
├── App.A.Tests/       # Integration tests for App.A
└── App.B.Tests/       # Integration tests for App.B
.buildkite/
├── pipeline.yml       # Bootstrap step (configured in Buildkite)
└── pipeline/          # C# project that generates the pipeline dynamically
```

## Local development

[mise](https://mise.jdx.dev) manages the .NET SDK version:

```bash
mise install
dotnet build
dotnet test
```

Preview the generated pipeline:

```bash
dotnet run --project .buildkite/pipeline
```

## Buildkite setup

### Pipeline steps (configured in Buildkite UI/API)

Set the following as the pipeline's step configuration:

```yaml
cache: ".buildkite/cache-volume"

steps:
  - label: ":pipeline: Generate pipeline"
    command: dotnet run --project .buildkite/pipeline | buildkite-agent pipeline upload
    plugins:
      - mise#v1.1.1: ~
```

This bootstrap step:

1. Uses the [mise plugin](https://github.com/buildkite-plugins/mise-buildkite-plugin) to install the .NET SDK on the agent
2. Runs the `.buildkite/pipeline/` C# project to dynamically generate pipeline steps
3. Pipes the generated YAML to `buildkite-agent pipeline upload`

### What gets generated

The C# pipeline project produces:

- **Restore** — `dotnet restore`
- **Build** — `dotnet build --configuration Release`
- **Tests** (grouped, parallel) — one step per test project

Each generated step includes the `mise#v1.1.1` plugin and `cache: .buildkite/cache-volume`.
