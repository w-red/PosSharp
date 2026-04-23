# POS for .NET 対応表

本書は、**Microsoft POS for .NET SDK** の概念と **PosSharp** における対応関係をまとめたものです。

## クラス・概念の対応

| POS for .NET のクラス | PosSharp での対応 | 説明 |
| --------------------- | ----------------- | ---- |
| `PosCommon` | `UposDeviceBase` | すべての UPOS デバイスの基底クラス。 |
| `PosControl` | `IUposDevice` | デバイス操作のための主要インターフェース。 |
| `DeviceService` | `UposDeviceBase` (継承) | デバイスの具体的な実装ロジック。 |
| `CashChanger` | `MyCashChanger : UposDeviceBase` | 特定のデバイス種別の実装。 |

## プロパティ・メソッドの対応

| 操作内容 | POS for .NET (レガシー) | PosSharp (モダン) |
| -------- | ----------------------- | ----------------- |
| **オープン** | `void Open()` | `Task OpenAsync(ct)` |
| **占有 (Claim)** | `void Claim(timeout)` | `Task ClaimAsync(timeout, ct)` |
| **有効化** | `bool DeviceEnabled { get; set; }` | `Task SetEnabledAsync(bool, ct)` |
| **状態確認** | `ControlState State { get; }` | `ReadOnlyReactiveProperty<ControlState> State` |
| **結果コード** | `int ResultCode { get; }` | `ReadOnlyReactiveProperty<UposErrorCode> ResultCode` |
| **DirectIO** | `DirectIO(command, data, obj)` | `Task DirectIOAsync(command, data, obj, ct)` |

## イベントの対応

| イベント | POS for .NET (デリゲート) | PosSharp (リアクティブ) |
| -------- | ------------------------- | ----------------------- |
| **データ** | `event DataEventHandler DataEvent` | `device.DataEvents.Subscribe(e => ...)` |
| **エラー** | `event DeviceErrorEventHandler ErrorEvent` | `device.ErrorEvents.Subscribe(e => ...)` |
| **ステータス** | `event StatusUpdateEventHandler StatusUpdateEvent` | `device.StatusUpdateEvents.Subscribe(e => ...)` |

## 主な違いとメリット

1. **完全非同期**: POS for .NET ではブロッキング（同期）処理が主流でしたが、PosSharp ではすべて `Task` ベースの非同期処理であり、`CancellationToken` によるキャンセルも可能です。
2. **リアクティブな状態**: プロパティの変更をポーリングする必要はありません。R3 の `Subscribe` を使用して、状態変化をリアルタイムかつ宣言的にハンドリングできます。
3. **メディエーターによる統合**: PosSharp は `UposMediator` を中心とした状態管理を行うため、Service Object 側で複雑な排他制御や状態整合性を手動で管理する手間が大幅に削減されます。
