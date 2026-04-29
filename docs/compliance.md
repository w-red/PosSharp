# UPOS Compliance Matrix

[English](compliance.md) | [日本語](compliance.jp.md)

---


This document outlines the level of compliance with the **UnifiedPOS (UPOS) v1.16** specification for common properties and methods.

## Common Properties

| Property | Support | Type | Reactive (R3) | Description |
| -------- | ------- | ---- | ------------- | ----------- |
| `State` | 笨・| `ControlState` | `ReadOnlyReactiveProperty` | `Closed`, `Idle`, `Busy` |
| `DeviceEnabled` | 笨・| `bool` | `ReadOnlyReactiveProperty` | Device operational state |
| `Claimed` | 笨・| `bool` | `ReadOnlyReactiveProperty` | Exclusive access status |
| `DataCount` | 笨・| `int` | `ReadOnlyReactiveProperty` | Number of queued data events |
| `DataEventEnabled` | 笨・| `bool` | `ReactiveProperty` | Event delivery toggle |
| `PowerNotify` | 笨・| `PowerNotify` | `ReactiveProperty` | Power notification setting |
| `PowerState` | 笨・| `PowerState` | `ReadOnlyReactiveProperty` | Current power condition |
| `ResultCode` | 笨・| `UposErrorCode` | `ReadOnlyReactiveProperty` | Last operation result |
| `ResultCodeExtended` | 笨・| `int` | `ReadOnlyReactiveProperty` | Specific error detail |
| `CapPowerReporting` | 笨・| `PowerReporting` | Constant | Capability for power reporting |
| `CheckHealthText` | 笨・| `string` | `ReadOnlyReactiveProperty` | Result of health check |

## Common Methods

| Method | Support | Sync/Async | Description |
| ------ | ------- | ---------- | ----------- |
| `OpenAsync` | 笨・| Asynchronous | Initializes and opens the device |
| `CloseAsync` | 笨・| Asynchronous | Closes the device session |
| `ClaimAsync` | 笨・| Asynchronous | Gains exclusive access |
| `ReleaseAsync` | 笨・| Asynchronous | Releases exclusive access |
| `CheckHealthAsync`| 笨・| Asynchronous | Verifies device health |
| `ClearInputAsync` | 笨・| Asynchronous | Clears input event queues |
| `ClearOutputAsync`| 笨・| Asynchronous | Clears pending output |
| `DirectIOAsync` | 笨・| Asynchronous | Device-specific commands |

## Common Events

| Event | Support | Type | Reactive (R3) |
| ----- | ------- | ---- | ------------- |
| `DataEvent` | 笨・| `UposDataEventArgs` | `Observable<UposDataEventArgs>` |
| `DirectIOEvent` | 笨・| `UposDirectIOEventArgs` | `Observable<UposDirectIOEventArgs>` |
| `ErrorEvent` | 笨・| `UposErrorEventArgs` | `Observable<UposErrorEventArgs>` |
| `OutputComplete` | 笨・| `UposOutputCompleteEventArgs` | `Observable<UposOutputCompleteEventArgs>` |
| `StatusUpdate` | 笨・| `UposStatusUpdateEventArgs` | `Observable<UposStatusUpdateEventArgs>` |
