# API リファレンス (概要)

[English (英語)](api_list.md)

本書は、**PosSharp** フレームワークで公開されているパブリック API のリファレンスです。

## PosSharp.Abstractions

実装に依存しない、コアとなるインターフェースおよび型定義群です。

### インターフェース

- **[`IUposDevice`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice)**: すべての UPOS デバイスの主要なインターフェース。リアクティブなプロパティ (`State`, `Claimed` 等) と非同期メソッド ([`OpenAsync`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.OpenAsync(System.Threading.CancellationToken)), [`ClaimAsync`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.ClaimAsync(int,System.Threading.CancellationToken)) 等) を含みます。
- **[`IUposMediator`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposMediator)**: 同期に使用される内部状態管理インターフェース。
- **[`UposLifecycleManager`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.Lifecycle.UposLifecycleManager)**: デバイスの状態遷移を管理するクラス。
- **[`IUposEventSink`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposEventSink)**: UPOS イベントを受信して処理できるデバイス用のインターフェース。

### イベント (リアクティブストリーム)

`[IUposDevice](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice)` で提供される標準的な UPOS イベントのストリームと、それに対応する引数の型です。

| イベントストリーム | 対応するイベント引数 | 説明 |
| :--- | :--- | :--- |
| [`DataEvents`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.DataEvents) | [`UposDataEventArgs`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposDataEventArgs) | デバイスから入力データを受信したときに発火します。 |
| [`ErrorEvents`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.ErrorEvents) | [`UposErrorEventArgs`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposDataEventArgs) | 非同期処理中などにエラーが発生したときに発火します。 |
| [`StatusUpdateEvents`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.StatusUpdateEvents) | [`UposStatusUpdateEventArgs`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposStatusUpdateEventArgs) | デバイスの状態 (電源状態など) が変化したときに発火します。 |
| [`DirectIoEvents`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.DirectIoEvents) | [`UposDirectIoEventArgs`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposDirectIoEventArgs) | デバイス固有の DirectIO イベントが発生したときに発火します。 |
| [`OutputCompleteEvents`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.OutputCompleteEvents) | [`UposOutputCompleteEventArgs`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposOutputCompleteEventArgs) | 非同期の出力操作が完了したときに発火します。 |

### プロパティ (Properties)

`[IUposDevice](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice)` で提供される主要なプロパティです。多くのプロパティは [R3](https://github.com/Cysharp/R3) のリアクティブプロパティとして提供されており、リアルタイムな監視が可能です。

#### 状態・制御

| プロパティ名 | 型 | 説明 |
| :--- | :--- | :--- |
| `State` | `ReadOnlyReactiveProperty<[ControlState](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.ControlState)>` | デバイスの現在の論理状態 (Closed, Idle, Busy)。 |
| `IsBusy` | `ReadOnlyReactiveProperty<bool>` | デバイスが現在操作を実行中かどうか。 |
| `LastError` | `ReadOnlyReactiveProperty<[UposErrorCode](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode)>` | 最後に実行された操作の結果コード。 |
| `ResultCodeExtended` | `int` | 最後に実行された操作の拡張結果コード。 |
| `IsOpen` | `bool` | デバイスがオープンされているか。 |
| `IsClaimed` | `bool` | デバイスが排他占有されているか。 |
| `IsEnabled` | `bool` | デバイスが有効化 (DeviceEnabled) されているか。 |

#### データ・設定

| プロパティ名 | 型 | 説明 |
| :--- | :--- | :--- |
| `DataEventEnabled` | `bool` | データイベントの通知が有効かどうか。 |
| `DataCount` | `int` | 現在キューに溜まっているデータイベントの数。 |
| `AutoDisable` | `bool` | イベント発火後に自動で `DataEventEnabled` を false にするか。 |
| `CheckHealthText` | `string` | `[CheckHealthAsync](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.CheckHealthAsync(PosSharp.Abstractions.HealthCheckLevel,System.Threading.CancellationToken))` 実行後の診断結果テキスト。 |

#### 電源・情報

| プロパティ名 | 型 | 説明 |
| :--- | :--- | :--- |
| `PowerState` | `ReadOnlyReactiveProperty<[PowerState](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.PowerState)>` | デバイスの現在の電源状態。 |
| `PowerNotify` | `[PowerNotify](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.PowerNotify)` | 電源状態の通知モード (Disabled/Enabled)。 |
| `CapPowerReporting` | `[PowerReporting](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.PowerReporting)` | デバイスの電源報告能力。 |
| `DeviceName` | `string` | デバイスの論理名。 |
| `DeviceDescription` | `string` | デバイスの説明。 |
| `Capabilities` | `[UposCapabilities](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposCapabilities)` | デバイスの固定的な機能定義。 |
| `ServiceObjectDescription` | `string` | サービスオブジェクトの説明。 |
| `ServiceObjectVersion` | `string` | サービスオブジェクトのバージョン。 |

### 列挙型 (Enums)

- **[`ControlState`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.ControlState)**: デバイスの基本状態 (`Closed`, `Idle`, `Busy`)。
- **[`UposErrorCode`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode)**: 標準 UPOS エラーコード (`Success`, `Closed`, `Claimed`, `Enabled`, `Failure` 等)。
- **[`PowerState`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.PowerState)**: 電源状態 (`Unknown`, `Online`, `Off`, `Offline`, `OffOffline`)。
- **[`PowerNotify`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.PowerNotify)**: 電源通知設定 (`Disabled`, `Enabled`)。
- **[`HealthCheckLevel`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.HealthCheckLevel)**: ヘルスチェックレベル (`Internal`, `External`, `Interactive`)。

---

## PosSharp.Core

フレームワークの標準実装クラス群です。

### 基底クラス

- **[`UposDeviceBase`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposDeviceBase)**: UPOS デバイスを実装するための基底となる抽象クラス。プロパティの自動同期、電源管理、ライフサイクル制御を提供します。
- **[`UposMediator`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposMediator)**: 状態メディエーターの標準実装。
- **[`UposLifecycleManager`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.Lifecycle.UposLifecycleManager)**: ライフサイクルコーディネーターの標準実装。

### ライフサイクルハンドラー

- **[`StandardLifecycleHandler`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.Lifecycle.StandardLifecycleHandler)**: 標準的な UPOS デバイス用の遷移ロジック実装。

### 例外 (Exceptions)

- **[`UposException`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposException)**: UPOS 操作が失敗した際にスローされる基底例外です。`ErrorCode` (標準エラー) および `ExtendedErrorCode` (拡張エラー) を保持します。
- **[`UposStateException`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposStateException)**: 不正なデバイス状態 (例：`OpenAsync` 前に `ClaimAsync` を呼ぶなど) でメソッドが実行された場合にスローされます。`InvalidOperationException` を継承しており、`CurrentState` や `AllowedStates` を参照してデバッグが可能です。
- **`OperationCanceledException`**: 非同期メソッド (`OpenAsync` 等) に渡された `CancellationToken` がキャンセルされた場合にスローされます。

---

## デバイスの初期化と名前の紐付け

PosSharp では、デバイス名はメソッド引数ではなく、インスタンス生成時に確定させます。

### 1. 手動でのインスタンス生成

```csharp
// コンストラクタで名前や設定を渡す
var device = new MyCashChanger("LogicalDevice1");

// メソッド呼び出し時には名前の指定は不要
await device.OpenAsync();
```

### 2. 依存注入 (DI) による解決

.NET の標準的な DI コンテナ (Keyed Services) を利用した例です。

```csharp
// サービス登録時
services.AddKeyedSingleton<IUposDevice, MyCashChanger>("CashChanger1");

// 利用時 (インジェクション)
public class PosService([FromKeyedServices("CashChanger1")] IUposDevice device)
{
    public async Task Initialize()
    {
        // すでに名前が紐付いたインスタンスが注入される
        await device.OpenAsync();
    }
}
```

---

## リアクティブプロパティの利用方法 (R3)

PosSharp のプロパティは [R3](https://github.com/Cysharp/R3) をベースにしており、以下の方法で利用できます。

### 1. 状態の変化を購読する (`Subscribe`)

デバイスの状態変化をリアルタイムにハンドリングする標準的な方法です。

```csharp
// 状態の変化 (プロパティ) を購読
device.State.Subscribe(state => 
{
    Console.WriteLine($"現在の状態: {state}");
});

// デバイスイベント (ストリーム) を購読
device.DataEvents.Subscribe(e =>
{
    Console.WriteLine($"データ受信: Status={e.Status}");
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

- **[`UposMediatorExtensions`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposMediatorExtensions)**: [`UposMediator`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposMediator) 内での状態検証を補助するヘルパーメソッド。
  - `ValidateOpen()`: デバイスが Open 状態か確認。失敗時は [`UposErrorCode`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode).Closed を保持した [`UposStateException`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposStateException) をスロー。
  - `ValidateClaimed()`: デバイスが Claimed 状態か確認。失敗時は [`UposErrorCode`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode).NotClaimed を保持した [`UposStateException`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposStateException) をスロー。
  - `ValidateEnabled()`: デバイスが Enabled 状態か確認。失敗時は [`UposErrorCode`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode).Disabled を保持した [`UposStateException`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposStateException) をスロー。
  - `ValidateNotBusy()`: デバイスが Busy 状態でないか確認。失敗時は [`UposErrorCode`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode).Busy を保持した [`UposStateException`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposStateException) をスロー。

### 利用例

デバイスの実装クラス内で、操作の前提条件をチェックする際に使用します。

```csharp
public void PrintReceipt(string data)
{
    try 
    {
        // 1. 操作前に状態を検証
        // (BeginOperation 内部でもチェックされますが、事前検証として有用)
        mediator.ValidateEnabled();
        mediator.ValidateNotBusy();

        // 2. 操作の開始 (Busy ロックを取得し、終了時に自動解放)
        using (mediator.BeginOperation())
        {
            // 実際の印字処理
        }
    }
    catch (UposStateException ex)
    {
        // 投げられた例外から UPOS 標準エラーコードを取得して報告
        mediator.ReportError(ex.ErrorCode);
        throw;
    }
}
```

> [!TIP]
> `BeginOperation()` は内部で `ValidateEnabled()` と `ValidateNotBusy()` を自動的に実行します。追加のバリデーションが不要な場合は、`BeginOperation()` の呼び出しだけで十分です。
