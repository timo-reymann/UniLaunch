# UniLaunch design

## Product purpose

UniLaunch is a cross-platform desktop editor and autostart runner. It lets people define
applications to start at login, with reusable rules that decide when each application should
launch. The editor manages that configuration; the platform-specific executable evaluates it
when the operating system starts UniLaunch in autostart mode.

## User-facing model

Configuration is organized around four concepts:

- **Target** — an application or executable to launch.
- **Rule** — a condition, such as `always`, a time range, or selected weekdays.
- **Ruleset** — a named group of rules. All rules in a ruleset must match.
- **Entry** — associates one target with one ruleset.

The configuration schema and file locations are documented in the
[README configuration section](README.md#configuration). YAML is the default format; JSON is
also supported.

## Application experience

### Editor window

The shared Avalonia editor has three primary tabs: **Targets**, **Rulesets**, and **Entries**.
The file menu supports opening and saving configuration files. The header also provides
Settings and About actions. The default main-window size is 800×600.

Settings cover the connectivity-check endpoint and timeout, system/light/dark theme selection,
English or German UI language, and update checks on launch. The Settings window opens at
680×650 and can be resized. User interface preferences are stored separately from the
autostart configuration.

The UI uses Avalonia Fluent styling, shared dark and light theme adjustments, and bundled
branding and fonts. UI strings belong in `UniLaunch.UI/Assets/Resources.resx` and its German
translation `Resources.de.resx`; `Resources.Designer.cs` exposes those resources to the UI.

### Tray behavior

The editor creates a tray icon at startup. Its menu provides **Open UniLaunch**, a disabled
version item, and **Exit**. Clicking the icon or Open shows and activates the editor window.
Closing the main window exits the application; it does not hide the editor into the tray.

When a valid autostart run launches its targets, UniLaunch displays the same tray icon for
30 seconds as a visible confirmation, then exits. This confirmation is best-effort; Linux runs
without `DISPLAY` or `WAYLAND_DISPLAY` skip it. Linux desktop tray visibility also depends on a
StatusNotifier/AppIndicator host.

### Autostart execution

The platform executable acquires a single-instance guard, registers the target and rule types
available on that platform, loads configuration, and checks for `--autostart`:

- **Normal launch:** ensure the platform's autostart registration exists, then open the editor.
- **Autostart launch with usable entries:** evaluate entries against the current local date/time,
  launch matching targets, print launch results, show the brief tray confirmation, then exit.
- **Autostart launch without usable entries or with a configuration load failure:** open the
  editor rather than silently doing nothing.

Targets that do not require connectivity launch without waiting. Network-dependent targets wait
for the configured connectivity check. Eligible targets are launched concurrently; UniLaunch
does not supervise the launched applications after starting them.

Run `UniLaunch --autostart` (or the platform executable with that argument) to exercise the
autostart path manually.

## Code structure

| Project | Responsibility |
| --- | --- |
| `UniLaunch.Core` | Configuration model and storage, rules, target abstractions, launch engine, connectivity checks, update-check logic, and shared utilities. It does not depend on Avalonia. |
| `UniLaunch.UI` | Shared Avalonia application, editor windows, views/view-models, UI services, resources, themes, and tray behavior. |
| `UniLaunch.Linux` | Linux entry point, XDG configuration/autostart integration, and executable target registration. |
| `UniLaunch.MacOS` | macOS entry point, LaunchAgent integration, executable and `.app` targets. |
| `UniLaunch.Windows` | Windows entry point, registry autostart integration, executable targets, and installer configuration. |
| `UniLaunch.CodeGenerator` | Source generator used by the UI's view-model code. |
| `UniLaunch.UIDesigner` | Host application for the standalone Avalonia visual designer. |
| `UniLaunch.Core.Tests`, `UniLaunch.Linux.Tests` | Core behavior and Linux-specific tests. |

Each platform project owns its process entry point and platform registration. Shared behavior
belongs in Core or UI as appropriate; platform-specific APIs and target types belong in their
platform project. The macOS `.app` target uses `/usr/bin/open`; the executable target starts
standalone binaries and redirects their standard streams so launched programs are not tied to
UniLaunch's console.

## Design constraints

- Keep autostart execution useful without opening an editor window when a valid configuration
  exists.
- Keep the shared engine independent of UI and operating-system APIs.
- Treat serialized type names such as `targetType`, `ruleName`, `executable`, `appFile`,
  `always`, `time`, and `week-day` as configuration compatibility points.
- Register only the target and rule types supported by a platform; unsupported types must not
  be assumed to work cross-platform.
- Preserve user configuration when changing the editor or launch engine; editor preferences
  are stored independently.
