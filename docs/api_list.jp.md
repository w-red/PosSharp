# API 繝ｪ繝輔ぃ繝ｬ繝ｳ繧ｹ (讎りｦ・

[English](api_list.md) | [日本語](api_list.jp.md)

---


譛ｬ譖ｸ縺ｯ縲・*PosSharp** 繝輔Ξ繝ｼ繝繝ｯ繝ｼ繧ｯ縺ｧ蜈ｬ髢九＆繧後※縺・ｋ繝代ヶ繝ｪ繝・け API 縺ｮ繝ｪ繝輔ぃ繝ｬ繝ｳ繧ｹ縺ｧ縺吶・
## PosSharp.Abstractions

螳溯｣・↓萓晏ｭ倥＠縺ｪ縺・√さ繧｢縺ｨ縺ｪ繧九う繝ｳ繧ｿ繝ｼ繝輔ぉ繝ｼ繧ｹ縺翫ｈ縺ｳ蝙句ｮ夂ｾｩ鄒､縺ｧ縺吶・
### 繧､繝ｳ繧ｿ繝ｼ繝輔ぉ繝ｼ繧ｹ

- **[`IUposDevice`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice)**: 縺吶∋縺ｦ縺ｮ UPOS 繝・ヰ繧､繧ｹ縺ｮ荳ｻ隕√↑繧､繝ｳ繧ｿ繝ｼ繝輔ぉ繝ｼ繧ｹ縲ゅΜ繧｢繧ｯ繝・ぅ繝悶↑繝励Ο繝代ユ繧｣ (`State`, `Claimed` 遲・ 縺ｨ髱槫酔譛溘Γ繧ｽ繝・ラ ([`OpenAsync`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.OpenAsync(System.Threading.CancellationToken)), [`ClaimAsync`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.ClaimAsync(int,System.Threading.CancellationToken)) 遲・ 繧貞性縺ｿ縺ｾ縺吶・- **[`IUposMediator`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposMediator)**: 蜷梧悄縺ｫ菴ｿ逕ｨ縺輔ｌ繧句・驛ｨ迥ｶ諷狗ｮ｡逅・う繝ｳ繧ｿ繝ｼ繝輔ぉ繝ｼ繧ｹ縲・- **[`UposLifecycleManager`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.Lifecycle.UposLifecycleManager)**: 繝・ヰ繧､繧ｹ縺ｮ迥ｶ諷矩・遘ｻ繧堤ｮ｡逅・☆繧九け繝ｩ繧ｹ縲・- **[`IUposEventSink`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposEventSink)**: UPOS 繧､繝吶Φ繝医ｒ蜿嶺ｿ｡縺励※蜃ｦ逅・〒縺阪ｋ繝・ヰ繧､繧ｹ逕ｨ縺ｮ繧､繝ｳ繧ｿ繝ｼ繝輔ぉ繝ｼ繧ｹ縲・
### 繧､繝吶Φ繝・(繝ｪ繧｢繧ｯ繝・ぅ繝悶せ繝医Μ繝ｼ繝)

[`IUposDevice`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice) 縺ｧ謠蝉ｾ帙＆繧後ｋ讓呎ｺ也噪縺ｪ UPOS 繧､繝吶Φ繝医・繧ｹ繝医Μ繝ｼ繝縺ｨ縲√◎繧後↓蟇ｾ蠢懊☆繧句ｼ墓焚縺ｮ蝙九〒縺吶・
| 繧､繝吶Φ繝医せ繝医Μ繝ｼ繝 | 蟇ｾ蠢懊☆繧九う繝吶Φ繝亥ｼ墓焚 | 隱ｬ譏・|
| :--- | :--- | :--- |
| [`DataEvents`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.DataEvents) | [`UposDataEventArgs`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposDataEventArgs) | 繝・ヰ繧､繧ｹ縺九ｉ蜈･蜉帙ョ繝ｼ繧ｿ繧貞女菫｡縺励◆縺ｨ縺阪↓逋ｺ轣ｫ縺励∪縺吶・|
| [`ErrorEvents`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.ErrorEvents) | [`UposErrorEventArgs`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposDataEventArgs) | 髱槫酔譛溷・逅・ｸｭ縺ｪ縺ｩ縺ｫ繧ｨ繝ｩ繝ｼ縺檎匱逕溘＠縺溘→縺阪↓逋ｺ轣ｫ縺励∪縺吶・|
| [`StatusUpdateEvents`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.StatusUpdateEvents) | [`UposStatusUpdateEventArgs`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposStatusUpdateEventArgs) | 繝・ヰ繧､繧ｹ縺ｮ迥ｶ諷・(髮ｻ貅千憾諷九↑縺ｩ) 縺悟､牙喧縺励◆縺ｨ縺阪↓逋ｺ轣ｫ縺励∪縺吶・|
| [`DirectIoEvents`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.DirectIoEvents) | [`UposDirectIoEventArgs`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposDirectIoEventArgs) | 繝・ヰ繧､繧ｹ蝗ｺ譛峨・ DirectIO 繧､繝吶Φ繝医′逋ｺ逕溘＠縺溘→縺阪↓逋ｺ轣ｫ縺励∪縺吶・|
| [`OutputCompleteEvents`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.OutputCompleteEvents) | [`UposOutputCompleteEventArgs`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposOutputCompleteEventArgs) | 髱槫酔譛溘・蜃ｺ蜉帶桃菴懊′螳御ｺ・＠縺溘→縺阪↓逋ｺ轣ｫ縺励∪縺吶・|

### 繝励Ο繝代ユ繧｣ (Properties)

[`IUposDevice`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice) 縺ｧ謠蝉ｾ帙＆繧後ｋ荳ｻ隕√↑繝励Ο繝代ユ繧｣縺ｧ縺吶ょ､壹￥縺ｮ繝励Ο繝代ユ繧｣縺ｯ [R3](https://github.com/Cysharp/R3) 縺ｮ繝ｪ繧｢繧ｯ繝・ぅ繝悶・繝ｭ繝代ユ繧｣縺ｨ縺励※謠蝉ｾ帙＆繧後※縺翫ｊ縲√Μ繧｢繝ｫ繧ｿ繧､繝縺ｪ逶｣隕悶′蜿ｯ閭ｽ縺ｧ縺吶・
#### 迥ｶ諷九・蛻ｶ蠕｡

| 繝励Ο繝代ユ繧｣蜷・| 蝙・| 隱ｬ譏・|
| :--- | :--- | :--- |
| `State` | ReadOnlyReactiveProperty<[ControlState](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.ControlState)> | 繝・ヰ繧､繧ｹ縺ｮ迴ｾ蝨ｨ縺ｮ隲也炊迥ｶ諷・(Closed, Idle, Busy)縲・|
| `IsBusy` | ReadOnlyReactiveProperty<bool> | 繝・ヰ繧､繧ｹ縺檎樟蝨ｨ謫堺ｽ懊ｒ螳溯｡御ｸｭ縺九←縺・°縲・|
| `LastError` | ReadOnlyReactiveProperty<[UposErrorCode](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode)> | 譛蠕後↓螳溯｡後＆繧後◆謫堺ｽ懊・邨先棡繧ｳ繝ｼ繝峨・|
| `ResultCodeExtended` | `int` | 譛蠕後↓螳溯｡後＆繧後◆謫堺ｽ懊・諡｡蠑ｵ邨先棡繧ｳ繝ｼ繝峨・|
| `IsOpen` | `bool` | 繝・ヰ繧､繧ｹ縺後が繝ｼ繝励Φ縺輔ｌ縺ｦ縺・ｋ縺九・|
| `IsClaimed` | `bool` | 繝・ヰ繧､繧ｹ縺梧賜莉門頃譛峨＆繧後※縺・ｋ縺九・|
| `IsEnabled` | `bool` | 繝・ヰ繧､繧ｹ縺梧怏蜉ｹ蛹・(DeviceEnabled) 縺輔ｌ縺ｦ縺・ｋ縺九・|

#### 繝・・繧ｿ繝ｻ險ｭ螳・
| 繝励Ο繝代ユ繧｣蜷・| 蝙・| 隱ｬ譏・|
| :--- | :--- | :--- |
| `DataEventEnabled` | `bool` | 繝・・繧ｿ繧､繝吶Φ繝医・騾夂衍縺梧怏蜉ｹ縺九←縺・°縲・|
| `DataCount` | `int` | 迴ｾ蝨ｨ繧ｭ繝･繝ｼ縺ｫ貅懊∪縺｣縺ｦ縺・ｋ繝・・繧ｿ繧､繝吶Φ繝医・謨ｰ縲・|
| `AutoDisable` | `bool` | 繧､繝吶Φ繝育匱轣ｫ蠕後↓閾ｪ蜍輔〒 `DataEventEnabled` 繧・false 縺ｫ縺吶ｋ縺九・|
| `CheckHealthText` | `string` | [`CheckHealthAsync`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.CheckHealthAsync(PosSharp.Abstractions.HealthCheckLevel,System.Threading.CancellationToken)) 螳溯｡悟ｾ後・險ｺ譁ｭ邨先棡繝・く繧ｹ繝医・|

#### 髮ｻ貅舌・諠・ｱ

| 繝励Ο繝代ユ繧｣蜷・| 蝙・| 隱ｬ譏・|
| :--- | :--- | :--- |
| `PowerState` | ReadOnlyReactiveProperty<[PowerState](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.PowerState)> | 繝・ヰ繧､繧ｹ縺ｮ迴ｾ蝨ｨ縺ｮ髮ｻ貅千憾諷九・|
| `PowerNotify` | [`PowerNotify`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.PowerNotify) | 髮ｻ貅千憾諷九・騾夂衍繝｢繝ｼ繝・(Disabled/Enabled)縲・|
| `CapPowerReporting` | [`PowerReporting`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.PowerReporting) | 繝・ヰ繧､繧ｹ縺ｮ髮ｻ貅仙ｱ蜻願・蜉帙・|
| `DeviceName` | `string` | 繝・ヰ繧､繧ｹ縺ｮ隲也炊蜷阪・|
| `DeviceDescription` | `string` | 繝・ヰ繧､繧ｹ縺ｮ隱ｬ譏弱・|
| `Capabilities` | [`UposCapabilities`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposCapabilities) | 繝・ヰ繧､繧ｹ縺ｮ蝗ｺ螳夂噪縺ｪ讖溯・螳夂ｾｩ縲ＡAsString`, `AsInt`, `AsBool`, `As<T>` 繝｡繧ｽ繝・ラ縺ｫ繧医ｊ縲∝梛螳牙・縺ｫ蛟､繧貞叙蠕怜庄閭ｽ縺ｧ縺吶・|
| `ServiceObjectDescription` | `string` | 繧ｵ繝ｼ繝薙せ繧ｪ繝悶ず繧ｧ繧ｯ繝医・隱ｬ譏弱・|
| `ServiceObjectVersion` | `string` | 繧ｵ繝ｼ繝薙せ繧ｪ繝悶ず繧ｧ繧ｯ繝医・繝舌・繧ｸ繝ｧ繝ｳ縲・|

### 蛻玲嫌蝙・(Enums)

- **[`ControlState`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.ControlState)**: 繝・ヰ繧､繧ｹ縺ｮ蝓ｺ譛ｬ迥ｶ諷・(`Closed`, `Idle`, `Busy`)縲・- **[`UposErrorCode`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode)**: 讓呎ｺ・UPOS 繧ｨ繝ｩ繝ｼ繧ｳ繝ｼ繝・(`Success`, `Closed`, `Claimed`, `Enabled`, `Failure` 遲・縲・- **[`PowerState`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.PowerState)**: 髮ｻ貅千憾諷・(`Unknown`, `Online`, `Off`, `Offline`, `OffOffline`)縲・- **[`PowerNotify`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.PowerNotify)**: 髮ｻ貅宣夂衍險ｭ螳・(`Disabled`, `Enabled`)縲・- **[`HealthCheckLevel`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.HealthCheckLevel)**: 繝倥Ν繧ｹ繝√ぉ繝・け繝ｬ繝吶Ν (`Internal`, `External`, `Interactive`)縲・
---

## PosSharp.Core

繝輔Ξ繝ｼ繝繝ｯ繝ｼ繧ｯ縺ｮ讓呎ｺ門ｮ溯｣・け繝ｩ繧ｹ鄒､縺ｧ縺吶・
### 蝓ｺ蠎輔け繝ｩ繧ｹ

- **[`UposDeviceBase`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposDeviceBase)**: UPOS 繝・ヰ繧､繧ｹ繧貞ｮ溯｣・☆繧九◆繧√・蝓ｺ蠎輔→縺ｪ繧区歓雎｡繧ｯ繝ｩ繧ｹ縲ゅ・繝ｭ繝代ユ繧｣縺ｮ閾ｪ蜍募酔譛溘・崕貅千ｮ｡逅・√Λ繧､繝輔し繧､繧ｯ繝ｫ蛻ｶ蠕｡繧呈署萓帙＠縺ｾ縺吶・- **[`UposMediator`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposMediator)**: 迥ｶ諷九Γ繝・ぅ繧ｨ繝ｼ繧ｿ繝ｼ縺ｮ讓呎ｺ門ｮ溯｣・ＡAtomicState<T>` 繧貞茜逕ｨ縺励◆繝ｭ繝・け繝輔Μ繝ｼ縺ｧ繧ｹ繝ｬ繝・ラ繧ｻ繝ｼ繝輔↑迥ｶ諷区峩譁ｰ繧貞ｮ溽樟縺励※縺・∪縺吶・- **[`UposLifecycleManager`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.Lifecycle.UposLifecycleManager)**: 繝ｩ繧､繝輔し繧､繧ｯ繝ｫ繧ｳ繝ｼ繝・ぅ繝阪・繧ｿ繝ｼ縺ｮ讓呎ｺ門ｮ溯｣・・
### 繝ｦ繝ｼ繝・ぅ繝ｪ繝・ぅ繝ｻ蝓ｺ逶､

- **[`AtomicState<T>`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.AtomicState_1)**: Lock 繧剃ｽｿ逕ｨ縺励↑縺・，AS (Compare-And-Swap) 繝吶・繧ｹ縺ｮ繧｢繝医Α繝・け縺ｪ迥ｶ諷狗ｮ｡逅・さ繝ｳ繝昴・繝阪Φ繝医・- **[`MediatorSnapshot`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.MediatorSnapshot)**: `UposMediator` 縺ｮ迥ｶ諷具ｼ・tate, IsBusy, LastError 遲会ｼ峨ｒ荳諡ｬ縺ｧ菫晄戟縺吶ｋ荳榊､峨・繝ｬ繧ｳ繝ｼ繝峨・
### 繝ｩ繧､繝輔し繧､繧ｯ繝ｫ繝上Φ繝峨Λ繝ｼ

- **[`StandardLifecycleHandler`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.Lifecycle.StandardLifecycleHandler)**: 讓呎ｺ也噪縺ｪ UPOS 繝・ヰ繧､繧ｹ逕ｨ縺ｮ驕ｷ遘ｻ繝ｭ繧ｸ繝・け螳溯｣・・
### 萓句､・(Exceptions)

- **[`UposException`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposException)**: UPOS 謫堺ｽ懊′螟ｱ謨励＠縺滄圀縺ｫ繧ｹ繝ｭ繝ｼ縺輔ｌ繧句渕蠎穂ｾ句､悶〒縺吶ＡErrorCode` (讓呎ｺ悶お繝ｩ繝ｼ) 縺翫ｈ縺ｳ `ExtendedErrorCode` (諡｡蠑ｵ繧ｨ繝ｩ繝ｼ) 繧剃ｿ晄戟縺励∪縺吶・- **[`UposStateException`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposStateException)**: 荳肴ｭ｣縺ｪ繝・ヰ繧､繧ｹ迥ｶ諷・(萓具ｼ啻OpenAsync` 蜑阪↓ `ClaimAsync` 繧貞他縺ｶ縺ｪ縺ｩ) 縺ｧ繝｡繧ｽ繝・ラ縺悟ｮ溯｡後＆繧後◆蝣ｴ蜷医↓繧ｹ繝ｭ繝ｼ縺輔ｌ縺ｾ縺吶ＡInvalidOperationException` 繧堤ｶ呎価縺励※縺翫ｊ縲～CurrentState` 繧・`AllowedStates` 繧貞盾辣ｧ縺励※繝・ヰ繝・げ縺悟庄閭ｽ縺ｧ縺吶・- **`OperationCanceledException`**: 髱槫酔譛溘Γ繧ｽ繝・ラ (`OpenAsync` 遲・ 縺ｫ貂｡縺輔ｌ縺・`CancellationToken` 縺後く繝｣繝ｳ繧ｻ繝ｫ縺輔ｌ縺溷ｴ蜷医↓繧ｹ繝ｭ繝ｼ縺輔ｌ縺ｾ縺吶・
---

## 繝・ヰ繧､繧ｹ縺ｮ蛻晄悄蛹悶→蜷榊燕縺ｮ邏蝉ｻ倥￠

PosSharp 縺ｧ縺ｯ縲√ョ繝舌う繧ｹ蜷阪・繝｡繧ｽ繝・ラ蠑墓焚縺ｧ縺ｯ縺ｪ縺上√う繝ｳ繧ｹ繧ｿ繝ｳ繧ｹ逕滓・譎ゅ↓遒ｺ螳壹＆縺帙∪縺吶・
### 1. 謇句虚縺ｧ縺ｮ繧､繝ｳ繧ｹ繧ｿ繝ｳ繧ｹ逕滓・

```csharp
// 繧ｳ繝ｳ繧ｹ繝医Λ繧ｯ繧ｿ縺ｧ蜷榊燕繧・ｨｭ螳壹ｒ貂｡縺・var device = new MyCashChanger("LogicalDevice1");

// 繝｡繧ｽ繝・ラ蜻ｼ縺ｳ蜃ｺ縺玲凾縺ｫ縺ｯ蜷榊燕縺ｮ謖・ｮ壹・荳崎ｦ・await device.OpenAsync();
```

### 2. 萓晏ｭ俶ｳｨ蜈･ (DI) 縺ｫ繧医ｋ隗｣豎ｺ

.NET 縺ｮ讓呎ｺ也噪縺ｪ DI 繧ｳ繝ｳ繝・リ (Keyed Services) 繧貞茜逕ｨ縺励◆萓九〒縺吶・
```csharp
// 繧ｵ繝ｼ繝薙せ逋ｻ骭ｲ譎・services.AddKeyedSingleton<IUposDevice, MyCashChanger>("CashChanger1");

// 蛻ｩ逕ｨ譎・(繧､繝ｳ繧ｸ繧ｧ繧ｯ繧ｷ繝ｧ繝ｳ)
public class PosService([FromKeyedServices("CashChanger1")] IUposDevice device)
{
    public async Task Initialize()
    {
        // 縺吶〒縺ｫ蜷榊燕縺檎ｴ蝉ｻ倥＞縺溘う繝ｳ繧ｹ繧ｿ繝ｳ繧ｹ縺梧ｳｨ蜈･縺輔ｌ繧・        await device.OpenAsync();
    }
}
```

---

## 繝・ヰ繧､繧ｹ讖溯・ (Capabilities) 縺ｮ蛻ｩ逕ｨ

`Capabilities` 繝励Ο繝代ユ繧｣縺九ｉ縺ｯ縲√ョ繝舌う繧ｹ蝗ｺ譛峨・蝗ｺ螳夂噪縺ｪ險ｭ螳壼､繧・・蜉幢ｼ井ｾ具ｼ啻CapPowerReporting` 遲会ｼ峨ｒ蝙句ｮ牙・縺ｫ蜿門ｾ励〒縺阪∪縺吶・
```csharp
// 譁・ｭ怜・縺ｨ縺励※蜿門ｾ・(繝・ヵ繧ｩ繝ｫ繝亥､繧呈欠螳壼庄閭ｽ)
string deviceModel = device.Capabilities.AsString("ModelName", "Generic Device");

// 謨ｰ蛟､縺ｨ縺励※蜿門ｾ・int maxDataLength = device.Capabilities.AsInt("MaxDataLength");

// 逵溷⊃蛟､縺ｨ縺励※蜿門ｾ・bool supportsSpecialFeature = device.Capabilities.AsBool("CapSpecialFeature");

// 豎守畑逧・↑蝙区欠螳壼叙蠕・var complexConfig = device.Capabilities.As<MyConfig>("CustomConfig");
```

---

## 繝ｪ繧｢繧ｯ繝・ぅ繝悶・繝ｭ繝代ユ繧｣縺ｮ蛻ｩ逕ｨ譁ｹ豕・(R3)

PosSharp 縺ｮ繝励Ο繝代ユ繧｣縺ｯ [R3](https://github.com/Cysharp/R3) 繧偵・繝ｼ繧ｹ縺ｫ縺励※縺翫ｊ縲∽ｻ･荳九・譁ｹ豕輔〒蛻ｩ逕ｨ縺ｧ縺阪∪縺吶・
### 1. 迥ｶ諷九・螟牙喧繧定ｳｼ隱ｭ縺吶ｋ (`Subscribe`)

繝・ヰ繧､繧ｹ縺ｮ迥ｶ諷句､牙喧繧偵Μ繧｢繝ｫ繧ｿ繧､繝縺ｫ繝上Φ繝峨Μ繝ｳ繧ｰ縺吶ｋ讓呎ｺ也噪縺ｪ譁ｹ豕輔〒縺吶・
```csharp
// 迥ｶ諷九・螟牙喧 (繝励Ο繝代ユ繧｣) 繧定ｳｼ隱ｭ
device.State.Subscribe(state => 
{
    Console.WriteLine($"迴ｾ蝨ｨ縺ｮ迥ｶ諷・ {state}");
});

// 繝・ヰ繧､繧ｹ繧､繝吶Φ繝・(繧ｹ繝医Μ繝ｼ繝) 繧定ｳｼ隱ｭ
device.DataEvents.Subscribe(e =>
{
    Console.WriteLine($"繝・・繧ｿ蜿嶺ｿ｡: Status={e.Status}");
});
```

### 2. 迴ｾ蝨ｨ縺ｮ蛟､繧堤峩謗･蜿門ｾ励☆繧・(`Value`)

迴ｾ蝨ｨ縺ｮ繧ｹ繝翫ャ繝励す繝ｧ繝・ヨ繧貞叉蠎ｧ縺ｫ蜿門ｾ励＠縺溘＞蝣ｴ蜷医↓菴ｿ逕ｨ縺励∪縺吶・
```csharp
var currentState = device.State.Value;
if (currentState == ControlState.Idle) 
{
    // 蠕・ｩ滉ｸｭ縺ｮ蜃ｦ逅・}
```

---

## 諡｡蠑ｵ繝｡繧ｽ繝・ラ

- **[`UposMediatorExtensions`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposMediatorExtensions)**: [`UposMediator`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposMediator) 蜀・〒縺ｮ迥ｶ諷区､懆ｨｼ繧定｣懷勧縺吶ｋ繝倥Ν繝代・繝｡繧ｽ繝・ラ縲・  - `ValidateOpen()`: 繝・ヰ繧､繧ｹ縺・Open 迥ｶ諷九°遒ｺ隱阪ょ､ｱ謨玲凾縺ｯ [`UposErrorCode`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode).Closed 繧剃ｿ晄戟縺励◆ [`UposStateException`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposStateException) 繧偵せ繝ｭ繝ｼ縲・  - `ValidateClaimed()`: 繝・ヰ繧､繧ｹ縺・Claimed 迥ｶ諷九°遒ｺ隱阪ょ､ｱ謨玲凾縺ｯ [`UposErrorCode`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode).NotClaimed 繧剃ｿ晄戟縺励◆ [`UposStateException`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposStateException) 繧偵せ繝ｭ繝ｼ縲・  - `ValidateEnabled()`: 繝・ヰ繧､繧ｹ縺・Enabled 迥ｶ諷九°遒ｺ隱阪ょ､ｱ謨玲凾縺ｯ [`UposErrorCode`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode).Disabled 繧剃ｿ晄戟縺励◆ [`UposStateException`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposStateException) 繧偵せ繝ｭ繝ｼ縲・  - `ValidateNotBusy()`: 繝・ヰ繧､繧ｹ縺・Busy 迥ｶ諷九〒縺ｪ縺・°遒ｺ隱阪ょ､ｱ謨玲凾縺ｯ [`UposErrorCode`](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode).Busy 繧剃ｿ晄戟縺励◆ [`UposStateException`](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposStateException) 繧偵せ繝ｭ繝ｼ縲・
### 蛻ｩ逕ｨ萓・
繝・ヰ繧､繧ｹ縺ｮ螳溯｣・け繝ｩ繧ｹ蜀・〒縲∵桃菴懊・蜑肴署譚｡莉ｶ繧偵メ繧ｧ繝・け縺吶ｋ髫帙↓菴ｿ逕ｨ縺励∪縺吶・
```csharp
public void PrintReceipt(string data)
{
    try 
    {
        // 1. 謫堺ｽ懷燕縺ｫ迥ｶ諷九ｒ讀懆ｨｼ
        // (BeginOperation 蜀・Κ縺ｧ繧ゅメ繧ｧ繝・け縺輔ｌ縺ｾ縺吶′縲∽ｺ句燕讀懆ｨｼ縺ｨ縺励※譛臥畑)
        mediator.ValidateEnabled();
        mediator.ValidateNotBusy();

        // 2. 謫堺ｽ懊・髢句ｧ・(Busy 繝ｭ繝・け繧貞叙蠕励＠縲∫ｵゆｺ・凾縺ｫ閾ｪ蜍戊ｧ｣謾ｾ)
        using (mediator.BeginOperation())
        {
            // 螳滄圀縺ｮ蜊ｰ蟄怜・逅・        }
    }
    catch (UposStateException ex)
    {
        // 謚輔￡繧峨ｌ縺滉ｾ句､悶°繧・UPOS 讓呎ｺ悶お繝ｩ繝ｼ繧ｳ繝ｼ繝峨ｒ蜿門ｾ励＠縺ｦ蝣ｱ蜻・        mediator.ReportError(ex.ErrorCode);
        throw;
    }
}
```

> [!TIP]
> `BeginOperation()` 縺ｯ蜀・Κ縺ｧ `ValidateEnabled()` 縺ｨ `ValidateNotBusy()` 繧定・蜍慕噪縺ｫ螳溯｡後＠縺ｾ縺吶りｿｽ蜉縺ｮ繝舌Μ繝・・繧ｷ繝ｧ繝ｳ縺御ｸ崎ｦ√↑蝣ｴ蜷医・縲～BeginOperation()` 縺ｮ蜻ｼ縺ｳ蜃ｺ縺励□縺代〒蜊∝・縺ｧ縺吶・
