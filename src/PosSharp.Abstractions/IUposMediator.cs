using R3;

namespace PosSharp.Abstractions;

/// <summary>Defines a mediator that coordinates state, busy status, and error reporting for a UPOS device.</summary>
public interface IUposMediator : IDisposable
{
    /// <summary>Gets the current logical state of the device.</summary>
    ReadOnlyReactiveProperty<ControlState> State { get; }

    /// <summary>Gets the current value of the logical state.</summary>
    ControlState CurrentState { get; }

    /// <summary>Gets a value indicating whether the device is currently busy performing an operation.</summary>
    ReadOnlyReactiveProperty<bool> IsBusy { get; }

    /// <summary>Gets a value indicating whether the current value of the busy status is true.</summary>
    bool IsBusyValue { get; }

    /// <summary>Gets the last error code encountered by the device.</summary>
    ReadOnlyReactiveProperty<UposErrorCode> LastError { get; }

    /// <summary>Gets the extended result code of the last completed operation.</summary>
    ReadOnlyReactiveProperty<int> LastErrorExtended { get; }

    /// <summary>Gets the check health results text.</summary>
    ReadOnlyReactiveProperty<string> CheckHealthText { get; }

    /// <summary>Gets the current power state of the device.</summary>
    ReadOnlyReactiveProperty<PowerState> PowerState { get; }

    /// <summary>Gets the number of data events currently queued.</summary>
    ReadOnlyReactiveProperty<int> DataCount { get; }

    /// <summary>Updates the current state of the device.</summary>
    /// <param name="state">The new state.</param>
    void UpdateState(ControlState state);

    /// <summary>Sets the busy status of the device.</summary>
    /// <param name="isBusy">True if busy; otherwise, false.</param>
    void SetBusy(bool isBusy);

    /// <summary>Begins a device operation, setting the <see cref="IsBusy"/> property to true.
    /// Returns a disposable that automatically resets the busy state when disposed.
    /// </summary>
    /// <returns>A disposable to end the operation.</returns>
    IDisposable BeginOperation();

    /// <summary>Reports an error encountered by the device.</summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="extendedCode">The optional extended error code.</param>
    void ReportError(UposErrorCode errorCode, int extendedCode = 0);

    /// <summary>Updates the health check text.</summary>
    /// <param name="text">The result text.</param>
    void UpdateCheckHealthText(string text);

    /// <summary>Updates the current power state.</summary>
    /// <param name="powerState">The new power state.</param>
    void UpdatePowerState(PowerState powerState);

    /// <summary>Updates the count of queued data events.</summary>
    /// <param name="count">The new count.</param>
    void UpdateDataCount(int count);
}
