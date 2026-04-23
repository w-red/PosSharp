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

## Extension Methods
- **`UposMediatorExtensions`**: Helper methods for validating states within the mediator.
