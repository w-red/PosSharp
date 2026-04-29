# JavaPOS (JPOS) 蟇ｾ蠢懆｡ｨ

[English](jpos.md) | [日本語](jpos.jp.md)

---


譛ｬ譖ｸ縺ｯ縲・*JavaPOS (JPOS)** 隕乗ｼ縺ｨ **PosSharp** 縺ｫ縺翫￠繧句ｯｾ蠢憺未菫ゅｒ縺ｾ縺ｨ繧√◆繧ゅ・縺ｧ縺吶・ava 繧ｨ繧ｳ繧ｷ繧ｹ繝・Β縺九ｉ遘ｻ陦後☆繧矩幕逋ｺ閠・・縺溘ａ縺ｮ繧ｬ繧､繝峨〒縺吶・
## 螟夜Κ繝ｪ繝輔ぃ繝ｬ繝ｳ繧ｹ

- [JavaPOS 蜈ｬ蠑上し繧､繝・(http://www.javapos.com/)
- [UnifiedPOS 蜈ｬ蠑丈ｻ墓ｧ俶嶌 (OMG)](https://www.omg.org/spec/UPOS/)

## 繝｡繧ｽ繝・ラ縺ｮ蟇ｾ蠢・
| JPOS 縺ｮ繝｡繧ｽ繝・ラ | PosSharp 縺ｧ縺ｮ蟇ｾ蠢・| 蛯呵・|
| --------------- | ----------------- | ---- |
| `open(String logicalName)` | `OpenAsync(ct)` | 繝・ヰ繧､繧ｹ蜷阪・繧､繝ｳ繧ｹ繧ｿ繝ｳ繧ｹ逕滓・譎ゅｄ萓晏ｭ俶ｳｨ蜈･ (DI) 縺ｧ謖・ｮ壹＆繧後ｋ縺溘ａ縲∝ｼ墓焚縺ｯ荳崎ｦ√〒縺吶・|
| `close()` | `CloseAsync(ct)` | |
| `claim(int timeout)` | `ClaimAsync(timeout, ct)` | |
| `release()` | `ReleaseAsync(ct)` | |
| `setDeviceEnabled(boolean)` | `SetEnabledAsync(bool, ct)` | JPOS 縺ｯ繧ｻ繝・ち繝ｼ繝｡繧ｽ繝・ラ縺ｧ縺吶′縲￣osSharp 縺ｯ Task 繧定ｿ斐＠縺ｾ縺吶・|
| `checkHealth(int level)` | `CheckHealthAsync(level, ct)` | |
| `directIO(int, int[], Object)`| `DirectIOAsync(int, int, object, ct)` | |

## 繧､繝吶Φ繝医・蟇ｾ蠢・
JPOS 縺ｯ `EventListener` 繝代ち繝ｼ繝ｳ繧剃ｽｿ逕ｨ縺励∪縺吶′縲￣osSharp 縺ｯ `繝ｪ繧｢繧ｯ繝・ぅ繝悶せ繝医Μ繝ｼ繝 (R3)` 繧剃ｽｿ逕ｨ縺励∪縺吶・
| JPOS 縺ｮ繧､繝吶Φ繝医Μ繧ｹ繝翫・ | PosSharp 縺ｮ繝ｪ繧｢繧ｯ繝・ぅ繝悶せ繝医Μ繝ｼ繝 |
| ---------------------- | --------------------------------- |
| `DataListener` | `device.DataEvents` |
| `DirectIOListener` | `device.DirectIOEvents` |
| `ErrorListener` | `device.ErrorEvents` |
| `OutputCompleteListener` | `device.OutputCompleteEvents` |
| `StatusUpdateListener` | `device.StatusUpdateEvents` |

## 繝励Ο繝代ユ繧｣縺ｮ蟇ｾ蠢・
| JPOS 縺ｮ Getter/Setter | PosSharp 縺ｮ繝励Ο繝代ユ繧｣ | 繝ｪ繧｢繧ｯ繝・ぅ繝門梛 |
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

## 遘ｻ陦後・繝昴う繝ｳ繝・
- **Getter/Setter 縺九ｉ繝ｪ繧｢繧ｯ繝・ぅ繝悶∈**: Java 縺ｮ `getXXX()` / `setXXX()` 繝｡繧ｽ繝・ラ縺ｫ繧医ｋ繝昴・繝ｪ繝ｳ繧ｰ繧・憾諷狗｢ｺ隱阪・縲￣osSharp 縺ｧ縺ｯ R3 繝励Ο繝代ユ繧｣縺ｸ縺ｮ雉ｼ隱ｭ (Subscribe) 縺ｫ鄂ｮ縺肴鋤繧上ｊ縺ｾ縺吶・- **讀懈渊萓句､悶°繧・Task 縺ｸ**: JPOS 縺ｧ縺ｯ `JposException` 縺ｮ catch 縺悟ｿ・医〒縺励◆縺後￣osSharp 縺ｧ縺ｯ讓呎ｺ也噪縺ｪ `Task` 縺ｮ繧ｨ繝ｩ繝ｼ繝上Φ繝峨Μ繝ｳ繧ｰ縺翫ｈ縺ｳ `UposStateException` 繧剃ｽｿ逕ｨ縺励∪縺吶・- **繧ｹ繝ｬ繝・ラ邂｡逅・*: JPOS 縺ｧ縺ｯ繧ｳ繝ｼ繝ｫ繝舌ャ繧ｯ縺九ｉ UI 繧呈峩譁ｰ縺吶ｋ髫帙↓繧ｹ繝ｬ繝・ラ邂｡逅・↓豕ｨ諢上′蠢・ｦ√〒縺励◆縺後・  PosSharp (R3) 縺ｧ縺ｯ繧ｹ繧ｱ繧ｸ繝･繝ｼ繝ｩ縺ｫ繧医▲縺ｦ縺薙ｌ繧堤ｰ｡貎斐↓險倩ｿｰ縺ｧ縺阪∪縺吶・- **繝・ヰ繧､繧ｹ蜷阪・謖・ｮ・*: JPOS 縺ｧ縺ｯ `open` 繝｡繧ｽ繝・ラ縺ｧ隲也炊蜷阪ｒ謖・ｮ壹＠縺ｦ縺・∪縺励◆縺後・  PosSharp 縺ｧ縺ｯ繧､繝ｳ繧ｹ繧ｿ繝ｳ繧ｹ逕滓・譎ゅｄ萓晏ｭ俶ｳｨ蜈･ (DI) 縺ｮ繧ｿ繧､繝溘Φ繧ｰ縺ｧ莠句燕縺ｫ繝・ヰ繧､繧ｹ繧堤音螳壹＠縺ｾ縺吶・
