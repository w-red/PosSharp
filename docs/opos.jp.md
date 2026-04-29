# OPOS 蟇ｾ蠢懆｡ｨ

[English](opos.md) | [日本語](opos.jp.md)

---


譛ｬ譖ｸ縺ｯ縲√Ξ繧ｬ繧ｷ繝ｼ縺ｪ **OPOS (ActiveX/OCX)** 繧ｳ繝ｳ繝医Ο繝ｼ繝ｫ縺九ｉ **PosSharp** 縺ｸ遘ｻ陦後☆繧矩幕逋ｺ閠・・縺溘ａ縺ｮ螟画鋤繧ｬ繧､繝峨〒縺吶・
## 螟夜Κ繝ｪ繝輔ぃ繝ｬ繝ｳ繧ｹ

- [OPOS (ActiveX) 蜈ｬ蠑上Μ繧ｽ繝ｼ繧ｹ (Monroe Consulting)](http://www.monroecs.com/opos.htm)
- [UnifiedPOS 蜈ｬ蠑丈ｻ墓ｧ俶嶌 (OMG)](https://www.omg.org/spec/UPOS/)

## 繝｡繧ｽ繝・ラ縺ｮ蟇ｾ蠢・
| OPOS 繝｡繧ｽ繝・ラ | PosSharp 縺ｧ縺ｮ蟇ｾ蠢・| 蛯呵・|
| ------------- | ----------------- | ---- |
| `Open(device)` | `OpenAsync(ct)` | 繝・ヰ繧､繧ｹ蜷阪・繧､繝ｳ繧ｹ繧ｿ繝ｳ繧ｹ逕滓・譎ゅｄ萓晏ｭ俶ｳｨ蜈･ (DI) 縺ｧ謖・ｮ壹＆繧後ｋ縺溘ａ縲∝ｼ墓焚縺ｯ荳崎ｦ√〒縺吶・|
| `Close()` | `CloseAsync(ct)` | |
| `ClaimDevice(timeout)` | `ClaimAsync(timeout, ct)` | |
| `ReleaseDevice()` | `ReleaseAsync(ct)` | |
| `CheckHealth(level)` | `CheckHealthAsync(level, ct)` | |
| `ClearInput()` | `ClearInputAsync(ct)` | |
| `ClearOutput()` | `ClearOutputAsync(ct)` | |
| `DirectIO(command, data, string)` | `DirectIOAsync(cmd, data, obj, ct)` | `obj` 縺ｯ譁・ｭ怜・縺縺代〒縺ｪ縺上が繝悶ず繧ｧ繧ｯ繝医ｂ謖・ｮ壼庄閭ｽ縺ｧ縺吶・|

## 繝励Ο繝代ユ繧｣縺ｮ蟇ｾ蠢・
| OPOS 繝励Ο繝代ユ繧｣ | PosSharp 縺ｧ縺ｮ蟇ｾ蠢・| 繝ｪ繧｢繧ｯ繝・ぅ繝門梛 |
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

## 繧､繝吶Φ繝医・蟇ｾ蠢・
| OPOS 繧､繝吶Φ繝・| PosSharp 繝ｪ繧｢繧ｯ繝・ぅ繝悶せ繝医Μ繝ｼ繝 |
| ------------- | ------------------------------ |
| `DataEvent` | `device.DataEvents` |
| `DirectIOEvent` | `device.DirectIOEvents` |
| `ErrorEvent` | `device.ErrorEvents` |
| `OutputCompleteEvent` | `device.OutputCompleteEvents` |
| `StatusUpdateEvent` | `device.StatusUpdateEvents` |

## 遘ｻ陦後・繝昴う繝ｳ繝・
- **COM 縺九ｉ .NET 10.0 縺ｸ**: PosSharp 縺ｯ邏皮ｲ九↑ .NET 螳溯｣・〒縺吶・OM Interop 繧・  `regsvr32` 縺ｫ繧医ｋ逋ｻ骭ｲ縲・2bit/64bit 縺ｮ遶ｶ蜷医↓謔ｩ縺ｾ縺輔ｌ繧九％縺ｨ縺ｯ縺ゅｊ縺ｾ縺帙ｓ縲・- **蜷梧悄縺九ｉ髱槫酔譛溘∈**: OPOS 縺ｯ蜴ｳ蟇・↓蜷梧悄逧・(繝悶Ο繝・く繝ｳ繧ｰ) 縺ｪ蜍穂ｽ懊〒縺励◆縺後・  PosSharp 縺ｯ髱槫酔譛溘〒縺ゅｋ縺溘ａ縲ゞI 縺ｮ繝輔Μ繝ｼ繧ｺ繧帝亟縺弱√Μ繧ｽ繝ｼ繧ｹ繧貞柑邇・噪縺ｫ蛻ｩ逕ｨ縺ｧ縺阪∪縺吶・- **繧､繝吶Φ繝育ｮ｡逅・*: ActiveX 縺ｮ繧､繝吶Φ繝育匱轣ｫ繧貞ｾ・▽縺ｮ縺ｧ縺ｯ縺ｪ縺上√う繝吶Φ繝医せ繝医Μ繝ｼ繝繧・  縲瑚ｳｼ隱ｭ (Subscribe)縲阪☆繧句ｽ｢蠑上↓螟峨ｏ繧翫∪縺吶ゅ％繧後↓繧医ｊ縲´INQ 逧・↑
  繝輔ぅ繝ｫ繧ｿ繝ｪ繝ｳ繧ｰ繧・粋謌舌′螳ｹ譏薙↓縺ｪ繧翫∪縺吶・- **繝・ヰ繧､繧ｹ蜷阪・謖・ｮ・*: OPOS 縺ｧ縺ｯ `Open` 繝｡繧ｽ繝・ラ縺ｮ蠑墓焚縺ｧ隲也炊蜷阪ｒ謖・ｮ壹＠縺ｦ縺・∪縺励◆縺後・  PosSharp 縺ｧ縺ｯ繧､繝ｳ繧ｹ繧ｿ繝ｳ繧ｹ逕滓・譎ゅｄ萓晏ｭ俶ｳｨ蜈･ (DI) 縺ｮ繧ｿ繧､繝溘Φ繧ｰ縺ｧ莠句燕縺ｫ繝・ヰ繧､繧ｹ繧堤音螳壹＠縺ｾ縺吶・
