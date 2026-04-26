# Changelog

All notable changes to this project will be documented in this file. (このプロジェクトにおける主要な変更点は、このファイルに記録されます。)

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.1.0] - 2026-04-27

### Added (追加)
- **State Validation Helpers (状態検証ヘルパー)**: Added `UposMediatorExtensions` with methods like `ValidateOpen()`, `ValidateClaimed()`, etc., to simplify device state checks. (`UposMediatorExtensions` を追加し、デバイスの状態チェックを簡略化する `ValidateOpen()` や `ValidateClaimed()` などのメソッドを導入しました。)
- **Enhanced Exception (例外の強化)**: Added `ErrorCode` property to `UposStateException` for direct access to standard UPOS error codes. (`UposStateException` に `ErrorCode` プロパティを追加し、標準 UPOS エラーコードへ直接アクセスできるようにしました。)

### Changed (変更)
- **Documentation Overhaul (ドキュメントの刷新)**: Migrated all API reference links to GitHub Wiki for comprehensive technical details. (すべての API リファレンスのリンク先を GitHub Wiki へ移行し、詳細な技術情報を参照可能にしました。)
- **API Overview (API 概要)**: Reorganized `api_list.md` as an "Overview" and included `Capabilities` property in the summary tables. (`api_list.md` を「概要」として再構成し、概要テーブルに `Capabilities` プロパティを追加しました。)
- **Link Accessibility (リンクのアクセシビリティ)**: Improved Markdown link formatting in README and docs for better clickability and readability. (README およびドキュメント内の Markdown リンク形式を改善し、クリックしやすさと読みやすさを向上させました。)

### Fixed (修正)
- **Lifecycle Management (ライフサイクル管理)**: Improved state reset and transition verification logic in `UposLifecycleManager`. (`UposLifecycleManager` における状態リセットと遷移検証ロジックを改善しました。)

---

## [1.0.1] - 2026-04-21

### Changed (変更)
- **NuGet Integration (NuGet 統合)**: Finalized migration to official v1.0.1 NuGet package structure. (公式 v1.0.1 NuGet パッケージ構造への移行を完了しました。)
- **Static Analysis (静的解析)**: Resolved all SonarAnalyzer violations to ensure a zero-warning build state. (SonarAnalyzer の違反をすべて解消し、警告ゼロのビルド状態を確保しました。)

### Fixed (修正)
- **Resource Management (リソース管理)**: Fixed minor disposal logic issues in controller test suites. (コントローラーのテストスイートにおけるリソース破棄ロジックの軽微な問題を修正しました。)

---

## [1.0.0-preview.1] - 2026-04-20

### Added (追加)
- **Initial Framework Bootstrap (フレームワークの初期構築)**: Established the core PosSharp architecture including the Mediator pattern and Reactive state synchronization. (メディエーターパターンとリアクティブな状態同期を含む、PosSharp のコアアーキテクチャを確立しました。)
- **UPOS Core Abstractions (UPOS コア抽象化)**: Implemented standard UPOS interfaces and base classes (`IUposDevice`, `UposDeviceBase`). (標準的な UPOS インターフェースと基底クラス `IUposDevice`, `UposDeviceBase` を実装しました。)
- **Reactive Properties (リアクティブプロパティ)**: Integrated R3 for real-time property monitoring (State, PowerState, etc.). (R3 を統合し、状態や電源状態などのリアルタイム監視を可能にしました。)
