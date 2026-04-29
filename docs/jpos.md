# JavaPOS (JPOS) Mapping Matrix

[Japanese (日本誁E](jpos.jp.md)

This document provides a mapping between **JavaPOS (JPOS)** standards and **PosSharp** equivalents for developers coming from the Java ecosystem.

## External References

- [JavaPOS Official Site](http://www.javapos.com/)
- [UnifiedPOS Specification (OMG)](https://www.omg.org/spec/UPOS/)

## Method Mapping

| JPOS Method | PosSharp Equivalent | Note |
| ----------- | ------------------- | ---- |
| `open(String logicalName)` | `OpenAsync(ct)` | Device names are handled during instantiation (e.g., via DI), so no argument is needed here. |
| `close()` | `CloseAsync(ct)` | |
| `claim(int timeout)` | `ClaimAsync(timeout, ct)` | |
| `release()` | `ReleaseAsync(ct)` | |
| `setDeviceEnabled(boolean)` | `SetEnabledAsync(bool, ct)` | JPOS uses setter methods; PosSharp uses async tasks. |
| `checkHealth(int level)` | `CheckHealthAsync(level, ct)` | |
| `directIO(int, int[], Object)`| `DirectIOAsync(int, int, object, ct)` | |

## Event Mapping

JPOS uses the `EventListener` pattern, while PosSharp uses `Reactive Streams (R3)`.

| JPOS Event Listener | PosSharp Reactive Stream |
| ------------------- | ------------------------ |
| `DataListener` | `device.DataEvents` |
| `DirectIOListener` | `device.DirectIOEvents` |
| `ErrorListener` | `device.ErrorEvents` |
| `OutputCompleteListener` | `device.OutputCompleteEvents` |
| `StatusUpdateListener` | `device.StatusUpdateEvents` |

## Property Mapping

| JPOS Getter/Setter | PosSharp Property | Reactive Type |
| ------------------ | ----------------- | ------------- |
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

## Key Transitions

- **Getter/Setter to Reactive**: Java's `getXXX()` / `setXXX()` methods are replaced by PosSharp's R3 properties, allowing for functional chaining and observation.
- **Checked Exceptions to Tasks**: JPOS relies on `JposException`. PosSharp uses `UposStateException` and standard `Task` faulting mechanisms.
- **Threading**: JPOS often requires manual thread management for UI updates from callbacks. PosSharp's R3 integration simplifies this with schedulers.
- **Device Naming**: While JPOS required a logical name in the `open` method, PosSharp associates
  the device identity during instantiation or via Dependency Injection (DI).
