# vJoySerialFeeder — Development Guide

## What is this project?

A Windows Forms application (.NET Framework 4.8) that reads serial data from RC controller protocols (IBUS, SBUS, CRSF, DSM, etc.) and maps it to virtual joystick devices (vJoy, vXbox on Windows; uinput on Linux via Mono). Licensed under GPLv3.

## Build

```bash
# Windows (requires MSBuild / Visual Studio Build Tools)
msbuild vJoySerialFeeder/vJoySerialFeeder.sln /p:Configuration=Release /p:Platform="Any CPU"

# Linux (requires Mono)
xbuild vJoySerialFeeder/vJoySerialFeederLinux.csproj /p:Configuration=Release
```

## Test

```bash
# Windows only — tests reference the main WinForms project which requires .NET Framework 4.8
dotnet test vJoySerialFeeder.Tests/ --configuration Release
```

Tests run automatically in CI (GitHub Actions on windows-latest). They cannot build on macOS/Linux because the main project has WinForms/.resx dependencies that require the .NET Framework SDK.

## Project structure

```
vJoySerialFeeder/
  MainForm.cs, MainFormWorker.cs    — Main UI and worker loop
  Configuration.cs                  — Profile/settings management (JSON serialization)
  SerialProtocols/                  — Protocol readers (IBUS, SBUS, DSM, CRSF, etc.)
  Mappings/                         — Channel-to-joystick mapping (axis, button, bitmap)
  VirtualJoysticks/                 — Platform-abstracted joystick drivers
  Scripting/                        — Lua scripting engine (MoonSharp)
  ProcessInteraction/               — COM Automation (Windows) and WebSocket server
  Lib/                              — Native DLLs (vJoy, vXbox, MSVC runtime)
  Properties/                       — Assembly info, resources, settings
Arduino/                            — Arduino sketches for hardware interfaces
Docs/                               — User documentation
```

## Two csproj files

- `vJoySerialFeeder.csproj` — Windows build (includes vJoy, vXbox, FastColoredTextBox, COM Automation)
- `vJoySerialFeederLinux.csproj` — Linux build (includes uinput, excludes Windows-only features)

They share the same source files with platform-specific swaps (e.g., `LuaEditorFormLinux.cs` vs `LuaEditorForm.cs`).

## Code style

- **Indentation:** Tabs
- **Braces:** K&R (opening brace on same line as declaration)
- **Namespaces:** Block-scoped (`namespace Foo { }`, not file-scoped)
- **Access modifiers:** Always explicit
- **Line endings:** CRLF for .cs/.csproj/.sln (enforced by .gitattributes)

## Key architecture notes

- **MainForm.Instance** is a static singleton used by non-UI code. We are actively decoupling this via an `IApplicationHost` interface — see `docs/superpowers/plans/2026-03-17-modernize-for-dotnet10.md`.
- **Mapping classes** currently mix data/logic with WinForms UI (GetControl, Paint). These will be split into data-only classes and UI adapter classes.
- **VirtualJoysticks/** has a clean abstract base (`VJoyBase`) with platform-specific implementations. This is already well-separated.
- **Native DLLs** in `Lib/` are not managed by NuGet. `vJoyInterfaceWrap.dll` is the C# wrapper for the vJoy driver.

## Dependencies

- **MoonSharp** — Lua interpreter (local DLL in Lib/, migrating to NuGet)
- **FastColoredTextBox** — Syntax-highlighted text editor for Lua (Windows only, local DLL)
- **vJoyInterfaceWrap** — vJoy C# wrapper (local DLL, no NuGet package)
- **System.IO.Ports** — Serial port access

## Modernization plan

We're incrementally modernizing toward .NET 10 and cross-platform support. See the full plan at `docs/superpowers/plans/2026-03-17-modernize-for-dotnet10.md`. Key principles:
- Every PR must compile, run, and be independently mergeable
- Separate UI from logic before moving to a Core library
- Tests before refactoring
