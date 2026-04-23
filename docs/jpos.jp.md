# JavaPOS (JPOS) 対応表

[English (英語)](jpos.md)

本書は、**JavaPOS (JPOS)** 規格と **PosSharp** における対応関係をまとめたものです。Java エコシステムから移行する開発者のためのガイドです。

## メソッドの対応

| JPOS のメソッド | PosSharp での対応 | 備考 |
| --------------- | ----------------- | ---- |
| `open(String logicalName)` | `OpenAsync(ct)` | 名前がパスカルケースになり、非同期化されています。 |
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
| `getClaimed()` | `Claimed` | `ReadOnlyReactiveProperty<bool>` |
| `getDeviceEnabled()` | `DeviceEnabled` | `ReadOnlyReactiveProperty<bool>` |
| `getResultCode()` | `ResultCode` | `ReadOnlyReactiveProperty<UposErrorCode>` |
| `getDataCount()` | `DataCount` | `ReadOnlyReactiveProperty<int>` |

## 移行のポイント

- **Getter/Setter からリアクティブへ**: Java の `getXXX()` / `setXXX()` メソッドによるポーリングや状態確認は、PosSharp では R3 プロパティへの購読（Subscribe）に置き換わります。
- **検査例外から Task へ**: JPOS では `JposException` の catch が必須でしたが、PosSharp では標準的な `Task` のエラーハンドリングおよび `UposStateException` を使用します。
- **スレッド管理**: JPOS ではコールバックから UI を更新する際にスレッド管理に注意が必要でしたが、PosSharp (R3) ではスケジューラによってこれを簡潔に記述できます。
