# POS for .NET 蟇ｾ蠢懆｡ｨ

[English](pos_for_dotnet.md) | [日本語](pos_for_dotnet.jp.md)

---


譛ｬ譖ｸ縺ｯ縲・*Microsoft POS for .NET SDK** 縺ｮ讎ょｿｵ縺ｨ **PosSharp** 縺ｫ縺翫￠繧句ｯｾ蠢憺未菫ゅｒ縺ｾ縺ｨ繧√◆繧ゅ・縺ｧ縺吶・
## 螟夜Κ繝ｪ繝輔ぃ繝ｬ繝ｳ繧ｹ

- [Microsoft POS for .NET SDK (Microsoft Learn)](https://learn.microsoft.com/en-us/previous-versions/dotnet/pos-for-net/ms828062(v=msdn.10))
- [UnifiedPOS 蜈ｬ蠑丈ｻ墓ｧ俶嶌 (OMG)](https://www.omg.org/spec/UPOS/)

## 繧ｯ繝ｩ繧ｹ繝ｻ讎ょｿｵ縺ｮ蟇ｾ蠢・
| POS for .NET 縺ｮ繧ｯ繝ｩ繧ｹ | PosSharp 縺ｧ縺ｮ蟇ｾ蠢・| 隱ｬ譏・|
| --------------------- | ----------------- | ---- |
| `PosCommon` | `UposDeviceBase` | 縺吶∋縺ｦ縺ｮ UPOS 繝・ヰ繧､繧ｹ縺ｮ蝓ｺ蠎輔け繝ｩ繧ｹ縲・|
| `PosControl` | `IUposDevice` | 繝・ヰ繧､繧ｹ謫堺ｽ懊・縺溘ａ縺ｮ荳ｻ隕√う繝ｳ繧ｿ繝ｼ繝輔ぉ繝ｼ繧ｹ縲・|
| `DeviceService` | `UposDeviceBase` (邯呎価) | 繝・ヰ繧､繧ｹ縺ｮ蜈ｷ菴鍋噪縺ｪ螳溯｣・Ο繧ｸ繝・け縲・|
| `CashChanger` | `MyCashChanger : UposDeviceBase` | 迚ｹ螳壹・繝・ヰ繧､繧ｹ遞ｮ蛻･縺ｮ螳溯｣・・|

## 繝励Ο繝代ユ繧｣繝ｻ繝｡繧ｽ繝・ラ縺ｮ蟇ｾ蠢・
| 謫堺ｽ懷・螳ｹ | POS for .NET (繝ｬ繧ｬ繧ｷ繝ｼ) | PosSharp (繝｢繝繝ｳ) |
| -------- | ----------------------- | ----------------- |
| **繧ｪ繝ｼ繝励Φ** | `void Open()` | `Task OpenAsync(ct)` |
| **蜊譛・(Claim)** | `void Claim(timeout)` | `Task ClaimAsync(timeout, ct)` |
| **譛牙柑蛹・* | `bool DeviceEnabled { get; set; }` | `Task SetEnabledAsync(bool, ct)` |
| **迥ｶ諷狗｢ｺ隱・* | `ControlState State { get; }` | `ReadOnlyReactiveProperty<ControlState> State` |
| **邨先棡繧ｳ繝ｼ繝・* | `int ResultCode { get; }` | `ReadOnlyReactiveProperty<UposErrorCode> ResultCode` |
| **諡｡蠑ｵ繧ｳ繝ｼ繝・* | `int ResultCodeExtended { get; }` | `int ResultCodeExtended` |
| **Busy迥ｶ諷・* | (Busy 迥ｶ諷九・逶ｴ謗･遒ｺ隱阪・髯仙ｮ夂噪) | `ReadOnlyReactiveProperty<bool> IsBusy` |
| **蛛･蠎ｷ險ｺ譁ｭ** | `void CheckHealth(level)` | `Task CheckHealthAsync(level, ct)` |
| **險ｺ譁ｭ邨先棡** | `string CheckHealthText { get; }` | `string CheckHealthText` |
| **DirectIO** | `DirectIO(command, data, obj)` | `Task DirectIOAsync(command, data, obj, ct)` |
| **蜈･蜉帙け繝ｪ繧｢** | `void ClearInput()` | `Task ClearInputAsync(ct)` |
| **蜃ｺ蜉帙け繝ｪ繧｢** | `void ClearOutput()` | `Task ClearOutputAsync(ct)` |
| **繝・・繧ｿ謨ｰ** | `int DataCount { get; }` | `int DataCount` |
| **閾ｪ蜍慕┌蜉ｹ蛹・* | `bool AutoDisable { get; set; }` | `bool AutoDisable` |
| **髮ｻ貅千憾諷・* | `PowerState PowerState { get; }` | `ReadOnlyReactiveProperty<PowerState> PowerState` |

## 繧､繝吶Φ繝医・蟇ｾ蠢・
| 繧､繝吶Φ繝・| POS for .NET (繝・Μ繧ｲ繝ｼ繝・ | PosSharp (繝ｪ繧｢繧ｯ繝・ぅ繝・ |
| -------- | ------------------------- | ----------------------- |
| **繝・・繧ｿ** | `event DataEventHandler DataEvent` | `device.DataEvents.Subscribe(e => ...)` |
| **繧ｨ繝ｩ繝ｼ** | `event DeviceErrorEventHandler ErrorEvent` | `device.ErrorEvents.Subscribe(e => ...)` |
| **繧ｹ繝・・繧ｿ繧ｹ** | `event StatusUpdateEventHandler StatusUpdateEvent` | `device.StatusUpdateEvents.Subscribe(e => ...)` |

## 荳ｻ縺ｪ驕輔＞縺ｨ繝｡繝ｪ繝・ヨ

1. **螳悟・髱槫酔譛・*: POS for .NET 縺ｧ縺ｯ繝悶Ο繝・く繝ｳ繧ｰ (蜷梧悄) 蜃ｦ逅・′荳ｻ豬√〒縺励◆縺後・   PosSharp 縺ｧ縺ｯ縺吶∋縺ｦ `Task` 繝吶・繧ｹ縺ｮ髱槫酔譛溷・逅・〒縺ゅｊ縲・   `CancellationToken` 縺ｫ繧医ｋ繧ｭ繝｣繝ｳ繧ｻ繝ｫ繧ょ庄閭ｽ縺ｧ縺吶・2. **繝ｪ繧｢繧ｯ繝・ぅ繝悶↑迥ｶ諷・*: 繝励Ο繝代ユ繧｣縺ｮ螟画峩繧偵・繝ｼ繝ｪ繝ｳ繧ｰ縺吶ｋ蠢・ｦ√・縺ゅｊ縺ｾ縺帙ｓ縲・   R3 縺ｮ `Subscribe` 繧剃ｽｿ逕ｨ縺励※縲∫憾諷句､牙喧繧偵Μ繧｢繝ｫ繧ｿ繧､繝縺九▽螳｣險逧・↓繝上Φ繝峨Μ繝ｳ繧ｰ縺ｧ縺阪∪縺吶・3. **繝｡繝・ぅ繧ｨ繝ｼ繧ｿ繝ｼ縺ｫ繧医ｋ邨ｱ蜷・*: PosSharp 縺ｯ `UposMediator` 繧剃ｸｭ蠢・→縺励◆迥ｶ諷狗ｮ｡逅・ｒ陦後≧縺溘ａ縲・   Service Object 蛛ｴ縺ｧ隍・尅縺ｪ謗剃ｻ門宛蠕｡繧・憾諷区紛蜷域ｧ繧呈焔蜍輔〒邂｡逅・☆繧区焔髢薙′螟ｧ蟷・↓蜑頑ｸ帙＆繧後∪縺吶・
