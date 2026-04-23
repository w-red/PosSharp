# UPOS Compliance Matrix

[Japanese (日本語)](compliance.jp.md)

This document outlines the level of compliance with the **UnifiedPOS (UPOS) v1.16** specification for common properties and methods.

## Common Properties

| Property | Support | Type | Reactive (R3) | Description |
| -------- | ------- | ---- | ------------- | ----------- |
| `State` | ✅ | `ControlState` | `ReadOnlyReactiveProperty` | `Closed`, `Idle`, `Busy` |
| `DeviceEnabled` | ✅ | `bool` | `ReadOnlyReactiveProperty` | Device operational state |
| `Claimed` | ✅ | `bool` | `ReadOnlyReactiveProperty` | Exclusive access status |
| `DataCount` | ✅ | `int` | `ReadOnlyReactiveProperty` | Number of queued data events |
| `DataEventEnabled` | ✅ | `bool` | `ReactiveProperty` | Event delivery toggle |
| `PowerNotify` | ✅ | `PowerNotify` | `ReactiveProperty` | Power notification setting |
| `PowerState` | ✅ | `PowerState` | `ReadOnlyReactiveProperty` | Current power condition |
| `ResultCode` | ✅ | `UposErrorCode` | `ReadOnlyReactiveProperty` | Last operation result |
| `ResultCodeExtended` | ✅ | `int` | `ReadOnlyReactiveProperty` | Specific error detail |
| `CapPowerReporting` | ✅ | `PowerReporting` | Constant | Capability for power reporting |
| `CheckHealthText` | ✅ | `string` | `ReadOnlyReactiveProperty` | Result of health check |

## Common Methods

| Method | Support | Sync/Async | Description |
| ------ | ------- | ---------- | ----------- |
| `OpenAsync` | ✅ | Asynchronous | Initializes and opens the device |
| `CloseAsync` | ✅ | Asynchronous | Closes the device session |
| `ClaimAsync` | ✅ | Asynchronous | Gains exclusive access |
| `ReleaseAsync` | ✅ | Asynchronous | Releases exclusive access |
| `CheckHealthAsync`| ✅ | Asynchronous | Verifies device health |
| `ClearInputAsync` | ✅ | Asynchronous | Clears input event queues |
| `ClearOutputAsync`| ✅ | Asynchronous | Clears pending output |
| `DirectIOAsync` | ✅ | Asynchronous | Device-specific commands |

## Common Events

| Event | Support | Type | Reactive (R3) |
| ----- | ------- | ---- | ------------- |
| `DataEvent` | ✅ | `UposDataEventArgs` | `Observable<UposDataEventArgs>` |
| `DirectIOEvent` | ✅ | `UposDirectIOEventArgs` | `Observable<UposDirectIOEventArgs>` |
| `ErrorEvent` | ✅ | `UposErrorEventArgs` | `Observable<UposErrorEventArgs>` |
| `OutputComplete` | ✅ | `UposOutputCompleteEventArgs` | `Observable<UposOutputCompleteEventArgs>` |
| `StatusUpdate` | ✅ | `UposStatusUpdateEventArgs` | `Observable<UposStatusUpdateEventArgs>` |
