# API リファレンス (全一覧)

[English (英語)](api_list.md)

**PosSharp** フレームワークで公開されているパブリック API の網羅的な一覧です。

## PosSharp.Abstractions

実装に依存しない、コアとなるインターフェースおよび型定義です。

### インターフェース
- **`IUposDevice`**: すべての UPOS デバイスの主要インターフェース。リアクティブなプロパティ (`State`, `Claimed` 等) と非同期メソッド (`OpenAsync`, `ClaimAsync` 等) を含みます。
- **`IUposMediator`**: 状態同期に使用される内部メディエーターインターフェース。
- **`IUposLifecycleManager`**: デバイスの状態遷移を統括するインターフェース。
- **`IUposEventSink`**: UPOS イベントを受信・処理可能なデバイスを示すインターフェース。

### 列挙型 (Enums)
- **`ControlState`**: デバイスの基本状態 (`Closed`, `Idle`, `Busy`)。
- **`UposErrorCode`**: 標準 UPOS エラーコード (`Success`, `Closed`, `Claimed`, `Enabled`, `Failure` 等)。
- **`PowerState`**: 電源状態 (`Unknown`, `Online`, `Off`, `Offline`, `Offline`)。
- **`PowerNotify`**: 電源通知設定 (`Disabled`, `Enabled`)。
- **`HealthCheckLevel`**: ヘルスチェックレベル (`Internal`, `External`, `Interactive`)。

### イベント引数
- `UposDataEventArgs`
- `UposDirectIOEventArgs`
- `UposErrorEventArgs`
- `UposOutputCompleteEventArgs`
- `UposStatusUpdateEventArgs`

---

## PosSharp.Core

フレームワークの標準実装クラス群です。

### 基底クラス
- **`UposDeviceBase`**: UPOS デバイス実装の基底となる抽象クラス。プロパティの自動同期、電源管理、ライフサイクル制御を提供します。
- **`UposMediator`**: 状態メディエーターの標準実装。
- **`UposLifecycleManager`**: ライフサイクルコーディネーターの標準実装。

### ライフサイクルハンドラー
- **`StandardLifecycleHandler`**: 標準的な UPOS デバイス用の遷移ロジック実装。

### 例外
- **`UposStateException`**: 不正なデバイス状態（例：`OpenAsync` 前に `ClaimAsync` を呼ぶなど）でメソッドが実行された場合にスローされます。

---

## 拡張メソッド
- **`UposMediatorExtensions`**: メディエーター内での状態検証を補助するヘルパーメソッド。
