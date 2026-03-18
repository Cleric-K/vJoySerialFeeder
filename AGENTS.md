# vJoySerialFeeder — Agent Development Guide

This file provides guidance for AI coding agents (Claude Code, Codex, Gemini, etc.) working on this project.

## Build

```bash
# Windows (requires MSBuild / Visual Studio Build Tools)
msbuild vJoySerialFeeder/vJoySerialFeeder.sln /p:Configuration=Release /p:Platform="Any CPU"

# Linux (requires Mono)
xbuild vJoySerialFeeder/vJoySerialFeederLinux.csproj /p:Configuration=Release
```

## Test

```bash
dotnet test vJoySerialFeeder.Tests/
```

## What is this project?

A Windows Forms application (.NET Framework 4.8) that reads serial data from RC controller protocols (IBUS, SBUS, CRSF, DSM, etc.) and maps it to virtual joystick devices (vJoy, vXbox on Windows; uinput on Linux via Mono). Licensed under GPLv3.

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
Arduino/                            — Arduino sketches for hardware interfaces
Docs/                               — User documentation
vJoySerialFeeder.Tests/             — Unit tests (NUnit)
```

## Two csproj files

- `vJoySerialFeeder.csproj` — Windows build (includes vJoy, vXbox, FastColoredTextBox, COM)
- `vJoySerialFeederLinux.csproj` — Linux build (includes uinput, excludes Windows-only features)

## Code style

- **Indentation:** Tabs (not spaces)
- **Braces:** Opening brace on same line as declaration (K&R style)
- **Namespaces:** Block-scoped (`namespace Foo { }`, not file-scoped)
- **Access modifiers:** Always explicit
- **Line endings:** CRLF for .cs/.csproj/.sln files

When modifying existing files, match the surrounding style exactly. Do not reformat code you didn't change.

## Architecture — important context

- **MainForm.Instance** is a static singleton referenced by non-UI code. We are decoupling this via `IApplicationHost` interface. If you're adding new code that needs to call MainForm, use `IApplicationHost` instead.
- **Mapping classes** mix data/logic with WinForms UI. We are splitting these — data-only classes in one file, UI adapters in another. If modifying mappings, keep this direction in mind.
- **VirtualJoysticks/** is well-abstracted with `VJoyBase` base class and platform-specific implementations. Follow this pattern for new joystick backends.
- **Protocol readers** inherit from `SerialReader`. Each parses a specific wire protocol into channel values. `Configure()` methods currently show WinForms dialogs — we're separating this.

## Modernization plan

See `docs/superpowers/plans/2026-03-17-modernize-for-dotnet10.md` for the full incremental modernization plan. Key rules:
- Every PR must compile and run independently
- Add tests before refactoring
- Separate UI from logic incrementally
- Don't change the project structure (SDK-style csproj migration comes later)

## Dependencies

- **MoonSharp** — Lua interpreter (local DLL, migrating to NuGet)
- **FastColoredTextBox** — Lua editor component (Windows only, local DLL)
- **vJoyInterfaceWrap** — vJoy C# wrapper (local DLL, no NuGet available)

Native DLLs in `Lib/` are committed to the repo — do not delete them.

## Things to avoid

- Do not add `using System.Windows.Forms` to files that don't already have it
- Do not reference `MainForm.Instance` from new code — use `IApplicationHost`
- Do not reformat entire files — only change what's needed for the task
- Do not convert to file-scoped namespaces or other C# 10+ syntax (we're still on .NET Framework 4.8)
- Do not delete or modify files in `Lib/` without explicit instruction
