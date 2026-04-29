# Changelog

[日本語 (Japanese)](CHANGELOG.jp.md)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.2.0] - 2026-04-30

### Added
- **Lock-Free State Management**: Introduced `AtomicState<T>` for high-concurrency, CAS-based (Compare-And-Swap) state transitions.
- **Mediator Snapshot**: Added `MediatorSnapshot` record to capture all mediator properties in a single, atomic operation.
- **Enhanced Testing Stubs**: Extended `StubUposDevice` to expose protected members for rigorous white-box testing.

### Changed
- **Architecture Refactoring**: Refactored `UposMediator` to use `AtomicState<MediatorSnapshot>`, ensuring thread-safe property updates without manual locks.
- **Fluent Capabilities API**: Renamed `UposCapabilities` methods from `Get*` to `As*` (e.g., `AsInt()`, `AsString()`) for a cleaner and more intuitive interface.
- **Documentation**: Updated README, API Reference, and Wiki to reflect the new architecture and API changes.

### Fixed
- **Mutation Testing Survivors**: Achieved a **100% mutation testing score** for `UposDeviceBase`. Resolved surviving mutants related to capability initialization and data event flushing.

---

## [1.1.0] - 2026-04-27

### Added
- **State Validation Helpers**: Added `UposMediatorExtensions` with methods like `ValidateOpen()`, `ValidateClaimed()`, etc., to simplify device state checks.
- **Enhanced Exception**: Added `ErrorCode` property to `UposStateException` for direct access to standard UPOS error codes.
- **Test Infrastructure Modernization**: Upgraded test suite to **xUnit v3** (v3.2.2) and updated various NuGet packages (Microsoft.NET.Test.Sdk, SonarAnalyzer, etc.) to their latest versions for better performance and stability.

### Changed
- **Documentation Overhaul**: Migrated all API reference links to GitHub Wiki for comprehensive technical details.
- **API Overview**: Reorganized `api_list.md` as an "Overview" and included `Capabilities` property in the summary tables.
- **Link Accessibility**: Improved Markdown link formatting in README and docs for better clickability and readability.

### Fixed
- **Lifecycle Management**: Improved state reset and transition verification logic in `UposLifecycleManager`.

---

## [1.0.1] - 2026-04-21

### Changed
- **NuGet Integration**: Finalized migration to official v1.0.1 NuGet package structure.
- **Static Analysis**: Resolved all SonarAnalyzer violations to ensure a zero-warning build state.

### Fixed
- **Resource Management**: Fixed minor disposal logic issues in controller test suites.

---

## [1.0.0-preview.1] - 2026-04-20

### Added
- **Initial Framework Bootstrap**: Established the core PosSharp architecture including the Mediator pattern and Reactive state synchronization.
- **UPOS Core Abstractions**: Implemented standard UPOS interfaces and base classes (`IUposDevice`, `UposDeviceBase`).
- **Reactive Properties**: Integrated R3 for real-time property monitoring (State, PowerState, etc.).
