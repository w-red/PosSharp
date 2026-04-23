# JavaPOS (JPOS) 対応表

[English (英語)](jpos.md)

本書は、**JavaPOS (JPOS)** 規格と **PosSharp** における対応関係をまとめたものです。Java エコシステムから移行する開発者のためのガイドです。

## 外部リファレンス

- [JavaPOS 公式サイト](http://www.javapos.com/)
- [UnifiedPOS 公式仕様書 (OMG)](https://www.omg.org/spec/UPOS/)

## メソッドの対応

| JPOS のメソッド | PosSharp での対応 | 備考 |
| --------------- | ----------------- | ---- |
| `open(String logicalName)` | `OpenAsync(ct)` | デバイス名はインスタンス生成時や依存注入 (DI) で指定されるため、引数は不要です。 |
| `close()` | `CloseAsync(ct)` | |
| `claim(int timeout)` | `ClaimAsync(timeout, ct)` | |
| `release()` | `ReleaseAsync(ct)` | |
| `setDeviceEnabled(boolean)` | `SetEnabledAsync(bool, ct)` | JPOS はセッターメソッドですが、PosSharp は Task を返します。 |
| `checkHealth(int level)` | `CheckHealthAsync(level, ct)` | |
| `directIO(int, int[], Object)`| `DirectIOAsync(int, int, object, ct)` | |

## イベントの対応

JPOS は `EventListener` パターンを使用しますが、PosSharp は `リアクティブストリーム (R3)` を使用します。

| JPOS のイベントリスナー | PosSharp のリアクティブストリーム |
| ---------------------- | --------------------------------- |
| `DataListener` | `device.DataEvents` |
| `DirectIOListener` | `device.DirectIOEvents` |
| `ErrorListener` | `device.ErrorEvents` |
| `OutputCompleteListener` | `device.OutputCompleteEvents` |
| `StatusUpdateListener` | `device.StatusUpdateEvents` |

## プロパティの対応

| JPOS の Getter/Setter | PosSharp のプロパティ | リアクティブ型 |
| --------------------- | --------------------- | -------------- |
| `getState()` | `State` | `ReadOnlyReactiveProperty<ControlState>` |
| `getClaimed()` | `Claimed` | `bool` |
| `getDeviceEnabled()` | `DeviceEnabled` | `ReadOnlyReactiveProperty<bool>` |
| `getResultCode()` | `ResultCode` | `ReadOnlyReactiveProperty<UposErrorCode>` |
| `getResultCodeExtended()` | `ResultCodeExtended` | `int` |
| `getCheckHealthText()` | `CheckHealthText` | `string` |
| `getDataCount()` | `DataCount` | `int` |
| `getDataEventEnabled()` | `DataEventEnabled` | `bool` (Mutable) |
| `getAutoDisable()` | `AutoDisable` | `bool` |
| `getPowerNotify()` | `PowerNotify` | `PowerNotify` (Mutable) |
| `getPowerState()` | `PowerState` | `ReadOnlyReactiveProperty<PowerState>` |
| `getDeviceDescription()` | `DeviceDescription` | `string` |
| `getDeviceName()` | `DeviceName` | `string` |
| `getServiceObjectDescription()` | `ServiceObjectDescription` | `string` |
| `getServiceObjectVersion()` | `ServiceObjectVersion` | `string` |

## 移行のポイント

- **Getter/Setter からリアクティブへ**: Java の `getXXX()` / `setXXX()` メソッドによるポーリングや状態確認は、PosSharp では R3 プロパティへの購読 (Subscribe) に置き換わります。
- **検査例外から Task へ**: JPOS では `JposException` の catch が必須でしたが、PosSharp では標準的な `Task` のエラーハンドリングおよび `UposStateException` を使用します。
- **スレッド管理**: JPOS ではコールバックから UI を更新する際にスレッド管理に注意が必要でしたが、
  PosSharp (R3) ではスケジューラによってこれを簡潔に記述できます。
- **デバイス名の指定**: JPOS では `open` メソッドで論理名を指定していましたが、
  PosSharp ではインスタンス生成時や依存注入 (DI) のタイミングで事前にデバイスを特定します。
