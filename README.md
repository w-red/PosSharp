# PosSharp

A platform-agnostic, reactive UPOS (Unified POS) framework for .NET.

## Overview

PosSharp is designed to provide a modern, C#-first implementation of the UPOS standard. It decouples the core logic of POS devices (Printers, Cash Changers, Scanners, etc.) from platform-specific SDK dependencies like the legacy POS for .NET (OPOS).

## Key Features

- **Platform Neutral**: Target `net10.0` with support for older platforms via [PolySharp](https://github.com/Sergio0694/PolySharp).
- **Reactive States**: Built-in state management using [R3](https://github.com/Cysharp/R3).
- **Modern Lifecycle**: Task-based asynchronous API for standard UPOS operations (Open, Claim, Enable).
- **Extensible**: Designed to be the foundation for cross-platform device simulators and production-ready drivers.

## Project Structure

- **PosSharp.Abstractions**: Pure C# interfaces and enums defining the UPOS contracts.
- **PosSharp.Core**: Standard, reusable logic for state transitions and mediation.

## Getting Started

PosSharp uses the modern Visual Studio solution format (`.slnx`). You can open `PosSharp.slnx` in Visual Studio 2022 (latest preview) or use the `dotnet` CLI with the project files.
