# OPOS 対応表

[English (英語)](opos.md)

本書は、レガシーな **OPOS (ActiveX/OCX)** コントロールから **PosSharp** へ移行する開発者のための変換ガイドです。

## 外部リファレンス

- [OPOS (ActiveX) 公式リソース (Monroe Consulting)](http://www.monroecs.com/opos.htm)
- [UnifiedPOS 公式仕様書 (OMG)](https://www.omg.org/spec/UPOS/)

## メソッドの対応

| OPOS メソッド | PosSharp での対応 | 備考 |
| ------------- | ----------------- | ---- |
| `Open(device)` | `OpenAsync(ct)` | デバイス名はインスタンス生成時や依存注入 (DI) で指定されるため、引数は不要です。 |
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
| `Claimed` | `IsClaimed` | `bool` |
| `ResultCode` | `ResultCode` | `ReadOnlyReactiveProperty<UposErrorCode>` |
| `ResultCodeExtended` | `ResultCodeExtended` | `ReadOnlyReactiveProperty<int>` |
| `CheckHealthText` | `CheckHealthText` | `string` |
| `DataCount` | `DataCount` | `ReadOnlyReactiveProperty<int>` |
| `DataEventEnabled` | `DataEventEnabled` | `ReactiveProperty<bool>` |
| `AutoDisable` | `AutoDisable` | `bool` |
| `PowerNotify` | `PowerNotify` | `ReactiveProperty<PowerNotify>` |
| `PowerState` | `PowerState` | `ReadOnlyReactiveProperty<PowerState>` |
| `DeviceDescription` | `DeviceDescription` | `string` |
| `DeviceName` | `DeviceName` | `string` |
| `ServiceObjectDescription` | `ServiceObjectDescription` | `string` |
| `ServiceObjectVersion` | `ServiceObjectVersion` | `string` |

## イベントの対応

| OPOS イベント | PosSharp リアクティブストリーム |
| ------------- | ------------------------------ |
| `DataEvent` | `device.DataEvents` |
| `DirectIOEvent` | `device.DirectIOEvents` |
| `ErrorEvent` | `device.ErrorEvents` |
| `OutputCompleteEvent` | `device.OutputCompleteEvents` |
| `StatusUpdateEvent` | `device.StatusUpdateEvents` |

## 移行のポイント

- **COM から .NET 10.0 へ**: PosSharp は純粋な .NET 実装です。COM Interop や
  `regsvr32` による登録、32bit/64bit の競合に悩まされることはありません。
- **同期から非同期へ**: OPOS は厳密に同期的 (ブロッキング) な動作でしたが、
  PosSharp は非同期であるため、UI のフリーズを防ぎ、リソースを効率的に利用できます。
- **イベント管理**: ActiveX のイベント発火を待つのではなく、イベントストリームを
  「購読 (Subscribe)」する形式に変わります。これにより、LINQ 的な
  フィルタリングや合成が容易になります。
- **デバイス名の指定**: OPOS では `Open` メソッドの引数で論理名を指定していましたが、
  PosSharp ではインスタンス生成時や依存注入 (DI) のタイミングで事前にデバイスを特定します。
