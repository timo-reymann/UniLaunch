# UniLaunch architecture

## Product and configuration model

UniLaunch is a cross-platform desktop editor and autostart runner. A configuration is organized
around four concepts:

- **Target** — an application or executable to launch.
- **Rule** — a condition, such as `always`, a time range, or selected weekdays.
- **Ruleset** — a named group of rules. All rules in a ruleset must match.
- **Entry** — associates one target with one ruleset.

The configuration schema and file locations are documented in the
[README configuration section](README.md#configuration). YAML is the default format; JSON is
also supported.

## Runtime modes

Each platform executable acquires a single-instance guard, registers supported target and rule
types, loads configuration, and checks for `--autostart`:

- **Normal launch:** ensure platform autostart registration exists, then open the editor.
- **Autostart with usable entries:** evaluate entries against local date/time, launch matching
  targets, print results, show the tray confirmation for 30 seconds, then exit.
- **Autostart without usable entries or with a configuration load failure:** open the editor
  rather than silently doing nothing.

Targets not requiring connectivity launch without waiting. Network-dependent targets wait for
the configured connectivity check. Eligible targets launch concurrently; UniLaunch does not
supervise launched applications after starting them. The tray confirmation is best-effort and
is skipped on Linux when neither `DISPLAY` nor `WAYLAND_DISPLAY` is set.

## Project boundaries

| Project | Responsibility |
| --- | --- |
| `UniLaunch.Core` | Configuration model and storage, rules, target abstractions, launch engine, connectivity checks, update checks, and shared utilities. It does not depend on Avalonia. |
| `UniLaunch.UI` | Shared Avalonia application, editor windows, views/view-models, UI services, resources, themes, and tray behavior. |
| `UniLaunch.Linux` | Linux entry point, XDG configuration/autostart integration, and executable target registration. |
| `UniLaunch.MacOS` | macOS entry point, LaunchAgent integration, executable and `.app` targets. |
| `UniLaunch.Windows` | Windows entry point, registry autostart integration, executable targets, and installer configuration. |
| `UniLaunch.CodeGenerator` | Source generator used by UI view-model code. |
| `UniLaunch.UIDesigner` | Host application for the standalone Avalonia visual designer. |
| `UniLaunch.Core.Tests`, `UniLaunch.Linux.Tests` | Core behavior and Linux-specific tests. |

Each platform project owns its process entry point and OS registration. Shared behavior belongs
in Core or UI as appropriate; platform APIs and target types belong in their platform project.
The macOS `.app` target uses `/usr/bin/open`. The executable target redirects standard streams
so launched programs are not tied to UniLaunch's console.

## Compatibility constraints

- Keep the shared engine independent of UI and operating-system APIs.
- Treat serialized discriminators and names such as `targetType`, `ruleName`, `executable`,
  `appFile`, `always`, `time`, and `week-day` as configuration compatibility points.
- Register only target and rule types supported by a platform; do not assume every type works
  cross-platform.
- Keep editor preferences stored separately from autostart configuration.

## Running the autostart path

Run `UniLaunch --autostart` (or the platform executable with that argument) to exercise the
autostart path manually. User-facing configuration examples and installation instructions are
in the [README](README.md).
