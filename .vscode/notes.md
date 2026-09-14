# Why launch.json/tasks.json were changed

## Symptom
F5 debugging `ShareX.ImageEditor.App` (and `ShareX`) failed with:
`launch: program '...\bin\Debug\win-arm64\ShareX.ImageEditor.App.exe' does not exist.`
This happened on an x64 machine, not ARM64.

## Root cause
- `ShareX.ImageEditor.App.csproj` / `ShareX.csproj` declare `<Platforms>x64;ARM64</Platforms>` (no plain `AnyCPU`).
- `Directory.build.props` only sets `RuntimeIdentifier` (`win-x64`/`win-arm64`) when `$(Platform)` is exactly `x64` or `ARM64`. Since `RuntimeIdentifier` is set, MSBuild appends it to the output path (`bin\Debug\<rid>\`).
- The original `launch.json` configs used `"type": "dotnet"` with only a `projectPath` (no explicit platform). The C# extension's auto-generated build task built with `/p:Platform=AnyCPU`, which matches neither condition above, so the build output went straight to `bin\Debug\` (no RID subfolder).
- Separately, the debugger's own project evaluation (used to find what to launch) resolved a *different* path than the build task did — it guessed a RID-qualified path (`win-arm64`), which the `AnyCPU` build never populated.
- Net result: build succeeded, but the debugger looked in a folder that was never built into.

## Fix
- Added explicit `tasks.json` build tasks per project/platform (`x64` and `ARM64`), each passing `-p:Platform=<platform>` so the RID (and therefore output folder) is deterministic.
- Replaced the ambiguous `"type": "dotnet"` launch configs with `"type": "coreclr"` configs that point `program` directly at the known output path (`bin/Debug/win-x64/...` or `bin/Debug/win-arm64/...`) and use a matching `preLaunchTask`, removing any auto-resolution guesswork.
- ARM64 configs are included for completeness/CI parity but can only run on an actual ARM64 Windows host (a win-x64 machine cannot execute a win-arm64 apphost).

## Where ARM64 is normally built
- CI (`.github/workflows/build.yml` / `pr.yml`) builds a full matrix of `configuration x platform` (including `ARM64`) on every push.
- Locally, ARM64 is only built when explicitly requested, e.g. `dotnet build <project> -c Debug -p:Platform=ARM64`, or via the "build ... (ARM64)" task added here.
