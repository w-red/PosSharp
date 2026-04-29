# UPOS 準拠性マトリクス

[English](compliance.md)

本書は、共通プロパティおよびメソッドにおける **UnifiedPOS (UPOS) v1.16** 仕様への準拠状況をまとめたものです。

## 共通プロパティ

| プロパティ | サポート | 型 | リアクティブ (R3) | 説明 |
| ---------- | -------- | -- | ----------------- | ---- |
| `State` | ✅ | `ControlState` | `ReadOnlyReactiveProperty` | `Closed`, `Idle`, `Busy` 状態 |
| `DeviceEnabled` | ✅ | `bool` | `ReadOnlyReactiveProperty` | デバイスの有効化状態 |
| `Claimed` | ✅ | `bool` | `ReadOnlyReactiveProperty` | 排他占有状態 |
| `DataCount` | ✅ | `int` | `ReadOnlyReactiveProperty` | キューにあるイベント数 |
| `DataEventEnabled` | ✅ | `bool` | `ReactiveProperty` | イベント配信の有効/無効 |
| `PowerNotify` | ✅ | `PowerNotify` | `ReactiveProperty` | 電源通知の設定 |
| `PowerState` | ✅ | `PowerState` | `ReadOnlyReactiveProperty` | 現在の電源状態 |
| `ResultCode` | ✅ | `UposErrorCode` | `ReadOnlyReactiveProperty` | 直近の操作結果 |
| `ResultCodeExtended` | ✅ | `int` | `ReadOnlyReactiveProperty` | エラーの詳細コード |
| `CapPowerReporting` | ✅ | `PowerReporting` | 定数 | 電源報告能力 |
| `CheckHealthText` | ✅ | `string` | `ReadOnlyReactiveProperty` | ヘルスチェックの結果文字列 |

## 共通メソッド

| メソッド | サポート | 非同期対応 | 説明 |
| -------- | -------- | ---------- | ---- |
| `OpenAsync` | ✅ | 済み | デバイスの初期化とオープン |
| `CloseAsync` | ✅ | 済み | デバイスのクローズ |
| `ClaimAsync` | ✅ | 済み | デバイスの排他占有 |
| `ReleaseAsync` | ✅ | 済み | 排他占有の解除 |
| `CheckHealthAsync`| ✅ | 済み | 動作確認の実行 |
| `ClearInputAsync` | ✅ | 済み | 入力イベントキューのクリア |
| `ClearOutputAsync`| ✅ | 済み | 実行待ち出力のクリア |
| `DirectIOAsync` | ✅ | 済み | デバイス固有コマンドの実行 |

## 共通イベント

| イベント | サポート | 引数型 | リアクティブ (R3) |
| -------- | -------- | ------ | ----------------- |
| `DataEvent` | ✅ | `UposDataEventArgs` | `Observable<UposDataEventArgs>` |
| `DirectIOEvent` | ✅ | `UposDirectIOEventArgs` | `Observable<UposDirectIOEventArgs>` |
| `ErrorEvent` | ✅ | `UposErrorEventArgs` | `Observable<UposErrorEventArgs>` |
| `OutputComplete` | ✅ | `UposOutputCompleteEventArgs` | `Observable<UposOutputCompleteEventArgs>` |
| `StatusUpdate` | ✅ | `UposStatusUpdateEventArgs` | `Observable<UposStatusUpdateEventArgs>` |
