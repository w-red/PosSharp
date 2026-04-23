# JavaPOS (JPOS) Mapping Matrix

[Japanese (日本語)](jpos.jp.md)

This document provides a mapping between **JavaPOS (JPOS)** standards and **PosSharp** equivalents for developers coming from the Java ecosystem.

## Method Mapping

| JPOS Method | PosSharp Equivalent | Note |
| ----------- | ------------------- | ---- |
| `open(String logicalName)` | `OpenAsync(ct)` | Case change and async-first. |
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
| `getClaimed()` | `Claimed` | `ReadOnlyReactiveProperty<bool>` |
| `getDeviceEnabled()` | `DeviceEnabled` | `ReadOnlyReactiveProperty<bool>` |
| `getResultCode()` | `ResultCode` | `ReadOnlyReactiveProperty<UposErrorCode>` |
| `getDataCount()` | `DataCount` | `ReadOnlyReactiveProperty<int>` |

## Key Transitions

- **Getter/Setter to Reactive**: Java's `getXXX()` / `setXXX()` methods are replaced by PosSharp's R3 properties, allowing for functional chaining and observation.
- **Checked Exceptions to Tasks**: JPOS relies on `JposException`. PosSharp uses `UposStateException` and standard `Task` faulting mechanisms.
- **Threading**: JPOS often requires manual thread management for UI updates from callbacks. PosSharp's R3 integration simplifies this with schedulers.
