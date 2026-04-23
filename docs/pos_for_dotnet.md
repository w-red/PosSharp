# POS for .NET Compatibility Matrix

This document provides a mapping between **Microsoft POS for .NET SDK** concepts and **PosSharp** equivalents.

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
| **Status** | `ControlState State { get; }` | `ReadOnlyReactiveProperty<ControlState> State` |
| **Error** | `int ResultCode { get; }` | `ReadOnlyReactiveProperty<UposErrorCode> ResultCode` |
| **IO** | `DirectIO(command, data, obj)` | `Task DirectIOAsync(command, data, obj, ct)` |

## Event Mapping

| Event | POS for .NET (Delegate) | PosSharp (Reactive) |
| ----- | ----------------------- | ------------------- |
| **Data** | `event DataEventHandler DataEvent` | `device.DataEvents.Subscribe(e => ...)` |
| **Error** | `event DeviceErrorEventHandler ErrorEvent` | `device.ErrorEvents.Subscribe(e => ...)` |
| **Status** | `event StatusUpdateEventHandler StatusUpdateEvent` | `device.StatusUpdateEvents.Subscribe(e => ...)` |

## Key Differences

1. **Async-First**: All blocking operations in POS for .NET are asynchronous tasks in PosSharp.
2. **Reactive State**: Instead of polling or simple getters, use `Subscribe` or `CurrentValue` from R3 reactive properties.
3. **Mediator Pattern**: PosSharp uses a centralized `UposMediator` to ensure thread-safe state synchronization, whereas POS for .NET often relies on manual state management in Service Objects.
