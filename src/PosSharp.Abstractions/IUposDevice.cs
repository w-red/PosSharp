using R3;

namespace PosSharp.Abstractions;

/// <summary>Basic interface for all UPOS devices, providing common lifecycle and state management.</summary>
public interface IUposDevice : IDisposable
{
    /// <summary>Gets the current logical state of the device.</summary>
    ReadOnlyReactiveProperty<ControlState> State { get; }

    /// <summary>Gets the frozen capabilities of the device.</summary>
    UposCapabilities Capabilities { get; }

    /// <summary>Gets a value indicating whether the device is currently processing an operation.</summary>
    ReadOnlyReactiveProperty<bool> IsBusy { get; }

    /// <summary>Gets a value indicating whether the device is currently processing an operation (Synchronous).</summary>
    bool IsBusyValue { get; }

    /// <summary>Gets the result code of the last completed operation.</summary>
    ReadOnlyReactiveProperty<UposErrorCode> LastError { get; }

    /// <summary>Gets a value indicating whether data event notification is enabled.</summary>
    bool DataEventEnabled { get; set; }

    /// <summary>Gets a value indicating whether data event notification is enabled (Reactive).</summary>
    ReadOnlyReactiveProperty<bool> DataEventEnabledProperty { get; }

    /// <summary>Gets a value indicating whether the device is open.</summary>
    bool IsOpen { get; }

    /// <summary>Gets a value indicating whether the device is claimed.</summary>
    bool IsClaimed { get; }

    /// <summary>Gets a value indicating whether the device is enabled.</summary>
    bool IsEnabled { get; }

    // ------------------------------------------------------------------
    // Common UPOS Properties
    // ------------------------------------------------------------------

    /// <summary>Gets a text description of the results of the last <see cref="CheckHealthAsync"/> call.</summary>
    string CheckHealthText { get; }

    /// <summary>Gets the extended result code of the last completed operation.</summary>
    int ResultCodeExtended { get; }

    /// <summary>Gets the number of data events currently queued.</summary>
    int DataCount { get; }

    /// <summary>Gets or sets a value indicating whether the device is automatically disabled after a data event is fired.</summary>
    bool AutoDisable { get; set; }

    /// <summary>Gets the power reporting capabilities of the device.</summary>
    PowerReporting CapPowerReporting { get; }

    /// <summary>Gets or sets the power notification mode.</summary>
    PowerNotify PowerNotify { get; set; }

    /// <summary>Gets the current power state of the device.</summary>
    PowerState PowerState { get; }

    /// <summary>Gets a description of the Service Object.</summary>
    string ServiceObjectDescription { get; }

    /// <summary>Gets the version of the Service Object.</summary>
    string ServiceObjectVersion { get; }

    /// <summary>Gets a description of the physical device.</summary>
    string DeviceDescription { get; }

    /// <summary>Gets the name of the physical device.</summary>
    string DeviceName { get; }

    // ------------------------------------------------------------------
    // Common UPOS Methods
    // ------------------------------------------------------------------

    /// <summary>Attempts to open the device for communication.</summary>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OpenAsync(CancellationToken ct = default);

    /// <summary>Closes communication with the device.</summary>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CloseAsync(CancellationToken ct = default);

    /// <summary>Claims exclusive access to the device.</summary>
    /// <param name="timeout">Timeout in milliseconds.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ClaimAsync(int timeout, CancellationToken ct = default);

    /// <summary>Releases exclusive access to the device.</summary>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ReleaseAsync(CancellationToken ct = default);

    /// <summary>Enables or disables the device.</summary>
    /// <param name="enabled"><see langword="true"/> to enable; <see langword="false"/> to disable.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetEnabledAsync(bool enabled, CancellationToken ct = default);

    /// <summary>Tests the health of the device.</summary>
    /// <param name="level">The level of health check to perform.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CheckHealthAsync(HealthCheckLevel level, CancellationToken ct = default);

    /// <summary>Sends a direct command to the device.</summary>
    /// <param name="command">The command to send.</param>
    /// <param name="data">Input/output data value.</param>
    /// <param name="obj">Input/output object.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DirectIOAsync(int command, int data, object obj, CancellationToken ct = default);

    /// <summary>Clears all pending input data.</summary>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ClearInputAsync(CancellationToken ct = default);

    /// <summary>Clears all pending buffered output data.</summary>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ClearOutputAsync(CancellationToken ct = default);

    // ------------------------------------------------------------------
    // Common UPOS Events
    // ------------------------------------------------------------------

    /// <summary>Gets the sequence of data events fired by the device.</summary>
    Observable<UposDataEventArgs> DataEvents { get; }

    /// <summary>Gets the sequence of error events fired by the device.</summary>
    Observable<UposErrorEventArgs> ErrorEvents { get; }

    /// <summary>Gets the sequence of status update events fired by the device.</summary>
    Observable<UposStatusUpdateEventArgs> StatusUpdateEvents { get; }

    /// <summary>Gets the sequence of direct IO events fired by the device.</summary>
    Observable<UposDirectIoEventArgs> DirectIoEvents { get; }

    /// <summary>Gets the sequence of output complete events fired by the device.</summary>
    Observable<UposOutputCompleteEventArgs> OutputCompleteEvents { get; }
}
