# API Reference (Overview)

[Japanese (日本語)](api_list.jp.md)

Reference guide for public APIs in the **PosSharp** framework.

## PosSharp.Abstractions

Core interfaces and types with zero dependencies on the implementation.

### Interfaces

- **`[IUposDevice](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice)`**: The primary interface for all UPOS devices. Includes reactive properties (`State`, `Claimed`, etc.) and asynchronous methods (`[OpenAsync](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.OpenAsync(System.Threading.CancellationToken))`, `[ClaimAsync](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.ClaimAsync(int,System.Threading.CancellationToken))`, etc.).
- **`[IUposMediator](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposMediator)`**: Internal state management interface used for synchronization.
- **`[UposLifecycleManager](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.Lifecycle.UposLifecycleManager)`**: Class for governing device state transitions.
- **`[IUposEventSink](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposEventSink)`**: Interface for devices that can receive and process UPOS events.

### Events (Reactive Streams)

Standard UPOS event streams provided by `[IUposDevice](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice)` and their corresponding argument types.

| Event Stream | Corresponding Event Args | Description |
| :--- | :--- | :--- |
| `[DataEvents](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.DataEvents)` | `[UposDataEventArgs](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposDataEventArgs)` | Fired when input data is received from the device. |
| `[ErrorEvents](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.ErrorEvents)` | `[UposErrorEventArgs](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorEventArgs)` | Fired when an error occurs during asynchronous processing. |
| `[StatusUpdateEvents](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.StatusUpdateEvents)` | `[UposStatusUpdateEventArgs](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposStatusUpdateEventArgs)` | Fired when the device status (e.g., power state) changes. |
| `[DirectIoEvents](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.DirectIoEvents)` | `[UposDirectIoEventArgs](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposDirectIoEventArgs)` | Fired when a device-specific DirectIO event occurs. |
| `[OutputCompleteEvents](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.OutputCompleteEvents)` | `[UposOutputCompleteEventArgs](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposOutputCompleteEventArgs)` | Fired when an asynchronous output operation completes. |

### Properties

Main properties provided by `[IUposDevice](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice)`. Many are exposed as [R3](https://github.com/Cysharp/R3) reactive properties for real-time monitoring.

#### Status & Control

| Property | Type | Description |
| :--- | :--- | :--- |
| `State` | `ReadOnlyReactiveProperty<[ControlState](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.ControlState)>` | Current logical state (Closed, Idle, Busy). |
| `IsBusy` | `ReadOnlyReactiveProperty<bool>` | Indicates if an operation is currently in progress. |
| `LastError` | `ReadOnlyReactiveProperty<[UposErrorCode](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode)>` | Result code of the last completed operation. |
| `ResultCodeExtended` | `int` | Extended result code of the last completed operation. |
| `IsOpen` | `bool` | Indicates if the device is open. |
| `IsClaimed` | `bool` | Indicates if the device is claimed. |
| `IsEnabled` | `bool` | Indicates if the device is enabled. |

#### Data & Settings

| Property | Type | Description |
| :--- | :--- | :--- |
| `DataEventEnabled` | `bool` | Whether data event notifications are enabled. |
| `DataCount` | `int` | Number of data events currently queued. |
| `AutoDisable` | `bool` | If true, automatically sets `DataEventEnabled` to false after an event. |
| `CheckHealthText` | `string` | Result text of the `[CheckHealthAsync](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.IUposDevice#PosSharp.Abstractions.IUposDevice.CheckHealthAsync(PosSharp.Abstractions.HealthCheckLevel,System.Threading.CancellationToken))` operation. |

#### Power & Information

| Property | Type | Description |
| :--- | :--- | :--- |
| `PowerState` | `ReadOnlyReactiveProperty<[PowerState](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.PowerState)>` | Current power state of the device. |
| `PowerNotify` | `[PowerNotify](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.PowerNotify)` | Power notification mode (Disabled/Enabled). |
| `CapPowerReporting` | `[PowerReporting](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.PowerReporting)` | Power reporting capabilities. |
| `DeviceName` | `string` | Logical name of the device. |
| `DeviceDescription` | `string` | Description of the device. |
| `ServiceObjectDescription` | `string` | Description of the Service Object. |
| `ServiceObjectVersion` | `string` | Version of the Service Object. |

### Enumerations

- **`[ControlState](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.ControlState)`**: `Closed`, `Idle`, `Busy`.
- **`[UposErrorCode](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode)`**: Standard UPOS error codes (`Success`, `Closed`, `Claimed`, `Enabled`, `Failure`, etc.).
- **`[PowerState](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.PowerState)`**: `Unknown`, `Online`, `Off`, `Offline`, `OffOffline`.
- **`[PowerNotify](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.PowerNotify)`**: `Disabled`, `Enabled`.
- **`[HealthCheckLevel](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.HealthCheckLevel)`**: `Internal`, `External`, `Interactive`.

---

## PosSharp.Core

Standard implementation of the framework.

### Base Classes

- **`[UposDeviceBase](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposDeviceBase)`**: The base abstract class for implementing UPOS devices. Provides automatic property synchronization, power management, and lifecycle control.
- **`[UposMediator](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposMediator)`**: Standard implementation of the state mediator.
- **`[UposLifecycleManager](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.Lifecycle.UposLifecycleManager)`**: Standard implementation of the lifecycle coordinator.

### Lifecycle Handlers

- **`[StandardLifecycleHandler](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.Lifecycle.StandardLifecycleHandler)`**: Implementation of transition logic for standard UPOS devices.

### Exceptions

- **`[UposException](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposException)`**: The base exception thrown when a UPOS operation fails. Contains `ErrorCode` (standard error) and `ExtendedErrorCode` (extended error).
- **`[UposStateException](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposStateException)`**: Thrown when a method is executed in an invalid device state (e.g., calling `ClaimAsync` before `OpenAsync`). It inherits from `InvalidOperationException` and provides `CurrentState` and `AllowedStates` for debugging.
- **`OperationCanceledException`**: Thrown when the `CancellationToken` passed to an asynchronous method (e.g., `OpenAsync`) is canceled.

---

## Device Initialization and Naming

In PosSharp, device names are determined during instantiation rather than being passed as method arguments.

### 1. Manual Instantiation

```csharp
// Pass the name or configuration through the constructor
var device = new MyCashChanger("LogicalDevice1");

// No name required during method calls
await device.OpenAsync();
```

### 2. Resolution via Dependency Injection (DI)

Example using standard .NET DI (Keyed Services).

```csharp
// Registration
services.AddKeyedSingleton<IUposDevice, MyCashChanger>("CashChanger1");

// Usage (Injection)
public class PosService([FromKeyedServices("CashChanger1")] IUposDevice device)
{
    public async Task Initialize()
    {
        // An instance already bound to the name is injected
        await device.OpenAsync();
    }
}
```

---

## How to use Reactive Properties (R3)

Properties in PosSharp are powered by [R3](https://github.com/Cysharp/R3). You can interact with them in two primary ways:

### 1. Subscribing to Changes (`Subscribe`)

The standard way to react to state transitions in real-time.

```csharp
// Subscribe to state changes (Properties)
device.State.Subscribe(state => 
{
    Console.WriteLine($"Current state: {state}");
});

// Subscribe to device events (Streams)
device.DataEvents.Subscribe(e =>
{
    Console.WriteLine($"Data received: Status={e.Status}");
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

- **`[UposMediatorExtensions](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposMediatorExtensions)`**: Helper methods for state validation within the `[UposMediator](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposMediator)`.
  - `ValidateOpen()`: Checks if the device is Open. Throws `[UposStateException](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposStateException)` with `[UposErrorCode](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode).Closed` on failure.
  - `ValidateClaimed()`: Checks if the device is Claimed. Throws `[UposStateException](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposStateException)` with `[UposErrorCode](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode).NotClaimed` on failure.
  - `ValidateEnabled()`: Checks if the device is Enabled. Throws `[UposStateException](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposStateException)` with `[UposErrorCode](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode).Disabled` on failure.
  - `ValidateNotBusy()`: Checks if the device is not Busy. Throws `[UposStateException](https://github.com/w-red/PosSharp/wiki/PosSharp.Core.UposStateException)` with `[UposErrorCode](https://github.com/w-red/PosSharp/wiki/PosSharp.Abstractions.UposErrorCode).Busy` on failure.

### Usage Example

Use these in your device implementation classes to check preconditions before performing operations.

```csharp
public void PrintReceipt(string data)
{
    try 
    {
        // 1. Validate state before operation
        // (BeginOperation also does this, but explicit check is useful)
        mediator.ValidateEnabled();
        mediator.ValidateNotBusy();

        // 2. Start the operation (acquires busy lock, automatically released on dispose)
        using (mediator.BeginOperation())
        {
            // Actual printing logic here
        }
    }
    catch (UposStateException ex)
    {
        // Report the standard UPOS error code derived from the exception
        mediator.ReportError(ex.ErrorCode);
        throw;
    }
}
```

> [!TIP]
> `BeginOperation()` automatically performs `ValidateEnabled()` and `ValidateNotBusy()` internally.
> If no additional custom validation is needed, simply calling `BeginOperation()` is sufficient.
