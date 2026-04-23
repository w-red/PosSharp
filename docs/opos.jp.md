# OPOS 対応表

[English (英語)](opos.md)

本書は、レガシーな **OPOS (ActiveX/OCX)** コントロールから **PosSharp** へ移行する開発者のための変換ガイドです。

## メソッドの対応

| OPOS メソッド | PosSharp での対応 | 備考 |
| ------------- | ----------------- | ---- |
| `Open(device)` | `OpenAsync(ct)` | デバイス名は実装または設定により管理されます。 |
| `Close()` | `CloseAsync(ct)` | |
| `ClaimDevice(timeout)` | `ClaimAsync(timeout, ct)` | |
| `ReleaseDevice()` | `ReleaseAsync(ct)` | |
| `CheckHealth(level)` | `CheckHealthAsync(level, ct)` | |
| `ClearInput()` | `ClearInputAsync(ct)` | |
| `ClearOutput()` | `ClearOutputAsync(ct)` | |
| `DirectIO(command, data, string)` | `DirectIOAsync(cmd, data, obj, ct)` | `obj` は文字列だけでなくオブジェクトも指定可能です。 |

## プロパティの対応

| OPOS プロパティ | PosSharp での対応 | リアクティブ型 |
| --------------- | ----------------- | -------------- |
| `DeviceEnabled` | `SetEnabledAsync(bool)` | `ReadOnlyReactiveProperty<bool>` |
| `State` | `State` | `ReadOnlyReactiveProperty<ControlState>` |
| `ResultCode` | `ResultCode` | `ReadOnlyReactiveProperty<UposErrorCode>` |
| `ResultCodeExtended` | `ResultCodeExtended` | `ReadOnlyReactiveProperty<int>` |
| `DataCount` | `DataCount` | `ReadOnlyReactiveProperty<int>` |
| `DataEventEnabled` | `DataEventEnabled` | `ReactiveProperty<bool>` |
| `PowerNotify` | `PowerNotify` | `ReactiveProperty<PowerNotify>` |
| `PowerState` | `PowerState` | `ReadOnlyReactiveProperty<PowerState>` |

## イベントの対応

| OPOS イベント | PosSharp リアクティブストリーム |
| ------------- | ------------------------------ |
| `DataEvent` | `device.DataEvents` |
| `DirectIOEvent` | `device.DirectIOEvents` |
| `ErrorEvent` | `device.ErrorEvents` |
| `OutputCompleteEvent` | `device.OutputCompleteEvents` |
| `StatusUpdateEvent` | `device.StatusUpdateEvents` |

## 移行のポイント

- **COM から .NET 10.0 へ**: PosSharp は純粋な .NET 実装です。COM Interop や `regsvr32` による登録、32bit/64bit の競合に悩まされることはありません。
- **同期から非同期へ**: OPOS は厳密に同期的（ブロッキング）な動作でしたが、PosSharp は非同期であるため、UI のフリーズを防ぎ、リソースを効率的に利用できます。
- **イベント管理**: ActiveX のイベント発火を待つのではなく、イベントストリームを「購読（Subscribe）」する形式に変わります。これにより、LINQ 的なフィルタリングや合成が容易になります。
