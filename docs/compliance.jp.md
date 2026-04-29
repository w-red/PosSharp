# UPOS 貅匁侠諤ｧ繝槭ヨ繝ｪ繧ｯ繧ｹ

[English](compliance.md) | [日本語](compliance.jp.md)

---


譛ｬ譖ｸ縺ｯ縲∝・騾壹・繝ｭ繝代ユ繧｣縺翫ｈ縺ｳ繝｡繧ｽ繝・ラ縺ｫ縺翫￠繧・**UnifiedPOS (UPOS) v1.16** 莉墓ｧ倥∈縺ｮ貅匁侠迥ｶ豕√ｒ縺ｾ縺ｨ繧√◆繧ゅ・縺ｧ縺吶・
## 蜈ｱ騾壹・繝ｭ繝代ユ繧｣

| 繝励Ο繝代ユ繧｣ | 繧ｵ繝昴・繝・| 蝙・| 繝ｪ繧｢繧ｯ繝・ぅ繝・(R3) | 隱ｬ譏・|
| ---------- | -------- | -- | ----------------- | ---- |
| `State` | 笨・| `ControlState` | `ReadOnlyReactiveProperty` | `Closed`, `Idle`, `Busy` 迥ｶ諷・|
| `DeviceEnabled` | 笨・| `bool` | `ReadOnlyReactiveProperty` | 繝・ヰ繧､繧ｹ縺ｮ譛牙柑蛹也憾諷・|
| `Claimed` | 笨・| `bool` | `ReadOnlyReactiveProperty` | 謗剃ｻ門頃譛臥憾諷・|
| `DataCount` | 笨・| `int` | `ReadOnlyReactiveProperty` | 繧ｭ繝･繝ｼ縺ｫ縺ゅｋ繧､繝吶Φ繝域焚 |
| `DataEventEnabled` | 笨・| `bool` | `ReactiveProperty` | 繧､繝吶Φ繝磯・菫｡縺ｮ譛牙柑/辟｡蜉ｹ |
| `PowerNotify` | 笨・| `PowerNotify` | `ReactiveProperty` | 髮ｻ貅宣夂衍縺ｮ險ｭ螳・|
| `PowerState` | 笨・| `PowerState` | `ReadOnlyReactiveProperty` | 迴ｾ蝨ｨ縺ｮ髮ｻ貅千憾諷・|
| `ResultCode` | 笨・| `UposErrorCode` | `ReadOnlyReactiveProperty` | 逶ｴ霑代・謫堺ｽ懃ｵ先棡 |
| `ResultCodeExtended` | 笨・| `int` | `ReadOnlyReactiveProperty` | 繧ｨ繝ｩ繝ｼ縺ｮ隧ｳ邏ｰ繧ｳ繝ｼ繝・|
| `CapPowerReporting` | 笨・| `PowerReporting` | 螳壽焚 | 髮ｻ貅仙ｱ蜻願・蜉・|
| `CheckHealthText` | 笨・| `string` | `ReadOnlyReactiveProperty` | 繝倥Ν繧ｹ繝√ぉ繝・け縺ｮ邨先棡譁・ｭ怜・ |

## 蜈ｱ騾壹Γ繧ｽ繝・ラ

| 繝｡繧ｽ繝・ラ | 繧ｵ繝昴・繝・| 髱槫酔譛溷ｯｾ蠢・| 隱ｬ譏・|
| -------- | -------- | ---------- | ---- |
| `OpenAsync` | 笨・| 貂医∩ | 繝・ヰ繧､繧ｹ縺ｮ蛻晄悄蛹悶→繧ｪ繝ｼ繝励Φ |
| `CloseAsync` | 笨・| 貂医∩ | 繝・ヰ繧､繧ｹ縺ｮ繧ｯ繝ｭ繝ｼ繧ｺ |
| `ClaimAsync` | 笨・| 貂医∩ | 繝・ヰ繧､繧ｹ縺ｮ謗剃ｻ門頃譛・|
| `ReleaseAsync` | 笨・| 貂医∩ | 謗剃ｻ門頃譛峨・隗｣髯､ |
| `CheckHealthAsync`| 笨・| 貂医∩ | 蜍穂ｽ懃｢ｺ隱阪・螳溯｡・|
| `ClearInputAsync` | 笨・| 貂医∩ | 蜈･蜉帙う繝吶Φ繝医く繝･繝ｼ縺ｮ繧ｯ繝ｪ繧｢ |
| `ClearOutputAsync`| 笨・| 貂医∩ | 螳溯｡悟ｾ・■蜃ｺ蜉帙・繧ｯ繝ｪ繧｢ |
| `DirectIOAsync` | 笨・| 貂医∩ | 繝・ヰ繧､繧ｹ蝗ｺ譛峨さ繝槭Φ繝峨・螳溯｡・|

## 蜈ｱ騾壹う繝吶Φ繝・
| 繧､繝吶Φ繝・| 繧ｵ繝昴・繝・| 蠑墓焚蝙・| 繝ｪ繧｢繧ｯ繝・ぅ繝・(R3) |
| -------- | -------- | ------ | ----------------- |
| `DataEvent` | 笨・| `UposDataEventArgs` | `Observable<UposDataEventArgs>` |
| `DirectIOEvent` | 笨・| `UposDirectIOEventArgs` | `Observable<UposDirectIOEventArgs>` |
| `ErrorEvent` | 笨・| `UposErrorEventArgs` | `Observable<UposErrorEventArgs>` |
| `OutputComplete` | 笨・| `UposOutputCompleteEventArgs` | `Observable<UposOutputCompleteEventArgs>` |
| `StatusUpdate` | 笨・| `UposStatusUpdateEventArgs` | `Observable<UposStatusUpdateEventArgs>` |
