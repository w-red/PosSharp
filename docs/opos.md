# OPOS (OLE for Retail POS) Mapping Matrix

This document provides a translation guide for developers migrating from legacy **OPOS (ActiveX/OCX)** controls to **PosSharp**.

## Method Mapping

| OPOS Method | PosSharp Equivalent | Note |
| ----------- | ------------------- | ---- |
| `Open(device)` | `OpenAsync(ct)` | `device` name is managed via implementation or config. |
| `Close()` | `CloseAsync(ct)` | |
| `ClaimDevice(timeout)` | `ClaimAsync(timeout, ct)` | |
| `ReleaseDevice()` | `ReleaseAsync(ct)` | |
| `CheckHealth(level)` | `CheckHealthAsync(level, ct)` | |
| `ClearInput()` | `ClearInputAsync(ct)` | |
| `ClearOutput()` | `ClearOutputAsync(ct)` | |
| `DirectIO(command, data, string)` | `DirectIOAsync(cmd, data, obj, ct)` | `obj` can be a string or any object. |

## Property Mapping

| OPOS Property | PosSharp Equivalent | Reactive Type |
| ------------- | ------------------- | ------------- |
| `DeviceEnabled` | `SetEnabledAsync(bool)` | `ReadOnlyReactiveProperty<bool>` |
| `State` | `State` | `ReadOnlyReactiveProperty<ControlState>` |
| `ResultCode` | `ResultCode` | `ReadOnlyReactiveProperty<UposErrorCode>` |
| `ResultCodeExtended` | `ResultCodeExtended` | `ReadOnlyReactiveProperty<int>` |
| `DataCount` | `DataCount` | `ReadOnlyReactiveProperty<int>` |
| `DataEventEnabled` | `DataEventEnabled` | `ReactiveProperty<bool>` |
| `PowerNotify` | `PowerNotify` | `ReactiveProperty<PowerNotify>` |
| `PowerState` | `PowerState` | `ReadOnlyReactiveProperty<PowerState>` |

## Event Mapping

| OPOS Event | PosSharp Reactive Stream |
| ---------- | ------------------------ |
| `DataEvent` | `device.DataEvents` |
| `DirectIOEvent` | `device.DirectIOEvents` |
| `ErrorEvent` | `device.ErrorEvents` |
| `OutputCompleteEvent` | `device.OutputCompleteEvents` |
| `StatusUpdateEvent` | `device.StatusUpdateEvents` |

## Migration Notes

- **COM to .NET**: PosSharp is a pure .NET 10.0 implementation. There is no need for COM Interop or `regsvr32` for the core logic.
- **Synchronous to Asynchronous**: OPOS was strictly synchronous (blocking). PosSharp is asynchronous, allowing for responsive UIs and better resource management.
- **Events**: Instead of responding to ActiveX event fires, you "subscribe" to event streams.
