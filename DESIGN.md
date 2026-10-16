# UniLaunch visual design

This document describes the visual language and UI tokens used by the editor. For runtime
behavior, project boundaries, and platform architecture, see [ARCHITECTURE.md](ARCHITECTURE.md).

## Visual direction

UniLaunch uses a friendly, colorful desktop-editor style: a recognizable illustrated logo,
rounded custom header, icon-led navigation, and clear form controls. The Fluent theme provides
the standard control shapes and states; the project theme dictionaries apply UniLaunch's own
palette and surface treatments.

Keep light and dark themes as parallel expressions of the same palette and hierarchy. Use the
purple header family for app identity, neutral surfaces for editing areas, and reserve the
warm unsaved-change color for that state.

## Color tokens

The live values are defined in `UniLaunch.UI/Styles/LightThemeAdjustments.axaml` and
`DarkThemeAdjustments.axaml`.

| Token | Light | Dark | Use |
| --- | --- | --- | --- |
| `MenuBar.Background` | `#AEA5DE` | `#27213C` | Main window header surface |
| `MenuBar.Background.Accent` | `#8F89B9` | `#3A2F5B` | Secondary header/menu accent |
| `MenuBar.Tab.Background` | `#AEA5DE` → `#A6A0D3` → `#9F97C8` | `#27213C` → `#302943` → `#392F52` | Vertical tab-strip gradient; values are top, midpoint, bottom |
| `MenuBar.PrimaryMenuItem.Foreground` | `#000000` | `#E2E2E2` | Header menu icons/text |
| `SidePanel.Background` | `#14000000` | `#14FFFFFF` | Subtle translucent overlay on the item list |
| `FileIcon.Unsaved.Color` | `#FFCC00` | `#EC942C` | Unsaved configuration indicator |
| `RuleSet.RuleForeground` | `#81000000` | `#8AFFFFFF` | Secondary ruleset text |

The leading alpha in the side-panel and ruleset values is intentional. When adjusting a token,
check it against both the surface it overlays and its counterpart in the other theme.

## Typography

- **Interface text:** Inter, supplied by Avalonia's `WithInterFont()` setup.
- **Brand wordmark:** bundled Lilita One display face, registered as the `Syne` font resource
  in `UniLaunch.UI/App.axaml` for the existing header binding.
- **Header wordmark:** 37 UI units; **tab labels:** 18; **menu items:** 14.

Use the existing font resources and sizing hierarchy rather than adding one-off typefaces or
large control-specific overrides.

## Layout and components

- **Main window:** opens at 800×600 UI units, with an 800×375 minimum. The custom header
  contains the logo/wordmark and file, Settings, and About actions. The tab strip is directly
  below it; the working area pairs a left-hand item list with a right-hand editor.
- **Navigation:** Targets, Rulesets, and Entries use the shared centered tab strip and icon-led
  tab controls.
- **Settings window:** opens at 680×650, with a 680×600 minimum, and is resizable. Settings
  content uses the same compact label/form hierarchy as the editor.
- **Theme:** keep dark and light adjustments in their respective theme dictionaries. Shared
  control structure and component styling belong in the shared UI project.

## Interaction and state

- The top header's file, Settings, and About actions use icons; menu text uses the standard
  menu size.
- The selected item is shown in the details pane. The file icon changes color when the current
  configuration has unsaved edits.
- The header drag area moves the window only while the primary pointer button is held. Pointer
  capture is used so release outside the header still ends a drag.
- A tray menu offers Open, a read-only version item, and Exit. Normal window close exits rather
  than hiding the editor into the tray.

## Localization and assets

UI copy is localized in `UniLaunch.UI/Assets/Resources.resx` (English) and
`Resources.de.resx` (German); resource properties are exposed through `Resources.Designer.cs`.
Keep labels concise enough for both languages. The logo, window/tray icon, and bundled font
assets live under `UniLaunch.UI/Assets/` and `Resources/`.
