# API リファレンス (全一覧)

[English (英語)](api_list.md)

本書は、**PosSharp** フレームワークで公開されているパブリック API のリファレンスです。

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
- **`PowerState`**: 電源状態 (`Unknown`, `Online`, `Off`, `Offline`, `OffOffline`)。
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

## リアクティブプロパティの利用方法 (R3)

PosSharp のプロパティは [R3](https://github.com/Cysharp/R3) をベースにしており、以下の方法で利用できます。

### 1. 状態の変化を購読する (`Subscribe`)
デバイスの状態変化をリアルタイムにハンドリングする標準的な方法です。

```csharp
// 状態が変化するたびに実行される
device.State.Subscribe(state => 
{
    Console.WriteLine($"現在の状態: {state}");
});
```

### 2. 現在の値を直接取得する (`Value`)
現在のスナップショットを即座に取得したい場合に使用します。

```csharp
var currentState = device.State.Value;
if (currentState == ControlState.Idle) 
{
    // 待機中の処理
}
```

---

## 拡張メソッド
- **`UposMediatorExtensions`**: メディエーター内での状態検証を補助するヘルパーメソッド。
  - `ValidateOpen()`: デバイスが Open 状態か確認。失敗時は `UposErrorCode.Closed` をスロー。
  - `ValidateClaimed()`: デバイスが Claimed 状態か確認。失敗時は `UposErrorCode.NotClaimed` をスロー。
  - `ValidateEnabled()`: デバイスが Enabled 状態か確認。失敗時は `UposErrorCode.Disabled` をスロー。
  - `ValidateNotBusy()`: デバイスが Busy 状態でないか確認。失敗時は `UposErrorCode.Busy` をスロー。

### 利用例
デバイスの実装クラス内で、操作の前提条件をチェックする際に使用します。

```csharp
public void PrintReceipt(string data)
{
    // デバイスが Enabled かつ Busy でないことを一括チェック
    mediator.ValidateEnabled();
    mediator.ValidateNotBusy();

    using (mediator.BeginOperation())
    {
        // 実際の印字処理
    }
}
```
