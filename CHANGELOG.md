# Changelog

[日本語 (Japanese)](CHANGELOG.jp.md)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.1.0] - 2026-04-27

### Added
- **State Validation Helpers**: Added `UposMediatorExtensions` with methods like `ValidateOpen()`, `ValidateClaimed()`, etc., to simplify device state checks.
- **Enhanced Exception**: Added `ErrorCode` property to `UposStateException` for direct access to standard UPOS error codes.

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
