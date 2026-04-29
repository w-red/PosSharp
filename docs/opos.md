# OPOS (OLE for Retail POS) Mapping Table

[Japanese (日本誁E](opos.jp.md)

Official migration guide for developers moving from legacy **OPOS (ActiveX/OCX)** controls to **PosSharp**.

## External References

- [OPOS (ActiveX) Official Resources (Monroe Consulting)](http://www.monroecs.com/opos.htm)
- [UnifiedPOS Specification (OMG)](https://www.omg.org/spec/UPOS/)

## Method Mapping

| OPOS Method | PosSharp Equivalent | Note |
| ----------- | ------------------- | ---- |
| `Open(device)` | `OpenAsync(ct)` | Device names are handled during instantiation (e.g., via DI), so no argument is needed here. |
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
| `Claimed` | `IsClaimed` | `bool` |
| `ResultCode` | `ResultCode` | `ReadOnlyReactiveProperty<UposErrorCode>` |
| `ResultCodeExtended` | `ResultCodeExtended` | `int` |
| `CheckHealthText` | `CheckHealthText` | `string` |
| `DataCount` | `DataCount` | `int` |
| `DataEventEnabled` | `DataEventEnabled` | `bool` (Mutable) |
| `AutoDisable` | `AutoDisable` | `bool` |
| `PowerNotify` | `PowerNotify` | `PowerNotify` (Mutable) |
| `PowerState` | `PowerState` | `ReadOnlyReactiveProperty<PowerState>` |
| `DeviceDescription` | `DeviceDescription` | `string` |
| `DeviceName` | `DeviceName` | `string` |
| `ServiceObjectDescription` | `ServiceObjectDescription` | `string` |
| `ServiceObjectVersion` | `ServiceObjectVersion` | `string` |

## Event Mapping

| OPOS Event | PosSharp Reactive Stream |
| ---------- | ------------------------ |
| `DataEvent` | `device.DataEvents` |
| `DirectIOEvent` | `device.DirectIOEvents` |
| `ErrorEvent` | `device.ErrorEvents` |
| `OutputCompleteEvent` | `device.OutputCompleteEvents` |
| `StatusUpdateEvent` | `device.StatusUpdateEvents` |

## Migration Notes

- **COM to .NET**: PosSharp is a pure .NET 10.0 implementation.
  There is no need for COM Interop or `regsvr32` for the core logic.
- **Synchronous to Asynchronous**: OPOS was strictly synchronous (blocking).
  PosSharp is asynchronous, allowing for responsive UIs and better resource management.
- **Events**: Instead of responding to ActiveX event fires, you "subscribe" to event streams.
- **Device Naming**: While OPOS required a logical name in the `Open` method, PosSharp
  associates the device identity during instantiation or via Dependency Injection (DI).
