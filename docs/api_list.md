# API Reference (Full List)

[Japanese (日本語)](api_list.jp.md)

Reference guide for public APIs in the **PosSharp** framework.

## PosSharp.Abstractions

Core interfaces and types with zero dependencies on the implementation.

### Interfaces
- **`IUposDevice`**: The primary interface for all UPOS devices. Contains reactive properties (`State`, `Claimed`, etc.) and asynchronous methods (`OpenAsync`, `ClaimAsync`, etc.).
- **`IUposMediator`**: Internal state management interface used for synchronization.
- **`IUposLifecycleManager`**: Interface for governing device state transitions.
- **`IUposEventSink`**: Interface for devices that can receive and process UPOS events.

### Enumerations
- **`ControlState`**: `Closed`, `Idle`, `Busy`.
- **`UposErrorCode`**: Standard UPOS error codes (`Success`, `Closed`, `Claimed`, `Enabled`, `Failure`, etc.).
- **`PowerState`**: `Unknown`, `Online`, `Off`, `Offline`, `OffOffline`.
- **`PowerNotify`**: `Disabled`, `Enabled`.
- **`HealthCheckLevel`**: `Internal`, `External`, `Interactive`.

### Event Arguments
- `UposDataEventArgs`
- `UposDirectIOEventArgs`
- `UposErrorEventArgs`
- `UposOutputCompleteEventArgs`
- `UposStatusUpdateEventArgs`

---

## PosSharp.Core

Standard implementation of the framework.

### Base Classes
- **`UposDeviceBase`**: The base class for implementing any UPOS device. Provides automatic property synchronization, power management, and lifecycle handling.
- **`UposMediator`**: Standard implementation of the state mediator.
- **`UposLifecycleManager`**: Standard implementation of the lifecycle coordinator.

### Lifecycle Handlers
- **`StandardLifecycleHandler`**: Default transition logic for standard UPOS devices.

### Exceptions
- **`UposStateException`**: Thrown when a method is called in an invalid device state (e.g., calling `ClaimAsync` before `OpenAsync`).

---

## How to use Reactive Properties (R3)

Properties in PosSharp are powered by [R3](https://github.com/Cysharp/R3). You can interact with them in two primary ways:

### 1. Subscribing to Changes (`Subscribe`)
The standard way to react to state transitions in real-time.

```csharp
// Executed whenever the state changes
device.State.Subscribe(state => 
{
    Console.WriteLine($"Current state: {state}");
});
```

### 2. Getting the Current Value (`Value`)
Use this when you need an immediate snapshot of the current value.

```csharp
var currentState = device.State.Value;
if (currentState == ControlState.Idle) 
{
    // Do something when idle
}
```

---

## Extension Methods
- **`UposMediatorExtensions`**: Helper methods for validating states within the mediator.
  - `ValidateOpen()`: Checks if the device is Open.
  - `ValidateClaimed()`: Checks if the device is Claimed.
  - `ValidateEnabled()`: Checks if the device is Enabled.
  - `ValidateNotBusy()`: Checks if the device is not Busy.

### Usage Example
Use these in your device implementation classes to check preconditions before performing operations.

```csharp
public void PrintReceipt(string data)
{
    // Ensure the device is enabled and not already busy
    mediator.ValidateEnabled();
    mediator.ValidateNotBusy();

    using (mediator.BeginOperation())
    {
        // Actual printing logic here
    }
}
```
