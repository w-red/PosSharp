# POS for .NET Compatibility Matrix

[Japanese (日本語)](pos_for_dotnet.jp.md)

This document provides a mapping between **Microsoft POS for .NET SDK** concepts
and **PosSharp** equivalents.

## External References

- [Microsoft POS for .NET SDK (Microsoft Learn)](https://learn.microsoft.com/en-us/previous-versions/dotnet/pos-for-net/ms828062(v=msdn.10))
- [UnifiedPOS Specification (OMG)](https://www.omg.org/spec/UPOS/)

## Class Mapping

| POS for .NET Class | PosSharp Equivalent | Description |
| ------------------ | ------------------- | ----------- |
| `PosCommon` | `UposDeviceBase` | Base class for all UPOS devices. |
| `PosControl` | `IUposDevice` | Core interface for device interaction. |
| `DeviceService` | `UposDeviceBase` (inherited) | Implementation logic. |
| `CashChanger` | `MyCashChanger : UposDeviceBase` | Specific device type implementation. |

## Property & Method Mapping

| Concept | POS for .NET (Legacy) | PosSharp (Modern) |
| ------- | --------------------- | ----------------- |
| **Open** | `void Open()` | `Task OpenAsync(ct)` |
| **Claim** | `void Claim(timeout)` | `Task ClaimAsync(timeout, ct)` |
| **Enable** | `bool DeviceEnabled { get; set; }` | `Task SetEnabledAsync(bool, ct)` |
| **State** | `ControlState State { get; }` | `ReadOnlyReactiveProperty<ControlState> State` |
| **Result Code** | `int ResultCode { get; }` | `ReadOnlyReactiveProperty<UposErrorCode> ResultCode` |
| **Extended Code** | `int ResultCodeExtended { get; }` | `int ResultCodeExtended` |
| **Busy State** | (Limited direct access in POS for .NET) | `ReadOnlyReactiveProperty<bool> IsBusy` |
| **Health Check** | `void CheckHealth(level)` | `Task CheckHealthAsync(level, ct)` |
| **Health Text** | `string CheckHealthText { get; }` | `string CheckHealthText` |
| **DirectIO** | `DirectIO(command, data, obj)` | `Task DirectIOAsync(command, data, obj, ct)` |
| **Clear Input** | `void ClearInput()` | `Task ClearInputAsync(ct)` |
| **Clear Output** | `void ClearOutput()` | `Task ClearOutputAsync(ct)` |
| **Data Count** | `int DataCount { get; }` | `int DataCount` |
| **Auto Disable** | `bool AutoDisable { get; set; }` | `bool AutoDisable` |
| **Power State** | `PowerState PowerState { get; }` | `ReadOnlyReactiveProperty<PowerState> PowerState` |

## Event Mapping

| Event | POS for .NET (Delegate) | PosSharp (Reactive) |
| ----- | ----------------------- | ------------------- |
| **Data** | `event DataEventHandler DataEvent` | `device.DataEvents.Subscribe(e => ...)` |
| **Error** | `event DeviceErrorEventHandler ErrorEvent` | `device.ErrorEvents.Subscribe(e => ...)` |
| **Status** | `event StatusUpdateEventHandler StatusUpdateEvent` | `device.StatusUpdateEvents.Subscribe(e => ...)` |

## Key Differences

1. **Fully Asynchronous**: While POS for .NET primarily used blocking calls, PosSharp
   is built on `Task`-based asynchronous patterns with full `CancellationToken` support.
2. **Reactive State**: No need for polling. Use R3's `Subscribe` to react to state
   changes in real-time and in a declarative manner.
3. **Mediator-Driven Integration**: PosSharp uses `UposMediator` to coordinate state,
   drastically reducing the need for manual synchronization or state consistency
   management within the Service Object.
