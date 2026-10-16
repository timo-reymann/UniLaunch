# Agent guide

Start with [DESIGN.md](DESIGN.md) for the current product behavior, architecture, platform
boundaries, and UI conventions. Use [README.md](README.md) for user-facing configuration,
installation, and usage details, and [CONTRIBUTING.md](CONTRIBUTING.md) for contribution policy.

## Repository map

- `UniLaunch.Core/` — platform-neutral launch engine, rules, configuration/storage, targets,
  connectivity checking, and update checking.
- `UniLaunch.UI/` — shared Avalonia UI, view-models, resources, themes, and tray implementation.
- `UniLaunch.Linux/`, `UniLaunch.MacOS/`, `UniLaunch.Windows/` — platform entry points and
  operating-system integrations.
- `UniLaunch.Core.Tests/`, `UniLaunch.Linux.Tests/` — automated tests.
- `UniLaunch.CodeGenerator/` — view-model source generator.

## Working conventions

- Keep operating-system-specific code in its platform project; do not add Avalonia dependencies
  to `UniLaunch.Core`.
- Maintain compatibility of serialized configuration names and discriminators. Review the
  README examples when changing configuration models or rule/target behavior.
- Follow the existing C# and Avalonia XAML patterns. The UI uses compiled bindings and
  ReactiveUI; keep resource strings localized in both `Resources.resx` and
  `Resources.de.resx`, and update `Resources.Designer.cs` when adding resources.
- Test platform-specific changes on the relevant platform when possible. Platform projects have
  distinct entry points and registration behavior even though they share Core and UI.
- Keep documentation aligned with current behavior. Update `DESIGN.md` for architecture or
  interaction changes and `README.md` for user-facing configuration or usage changes.

## Build and test

The repository targets .NET 10 (SDK selection is in `global.json`). Common checks:

```sh
dotnet build UniLaunch.sln
dotnet test UniLaunch.sln
# or
make test
```

Platform release builds are orchestrated by the Makefile: `make macos-build`,
`make linux-build`, or `make windows-build`. They may require platform-specific tools; see the
development requirements in `README.md`.
