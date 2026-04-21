using PosSharp.Abstractions;
using PosSharp.Core.Lifecycle;
using R3;

namespace PosSharp.Core;

/// <summary>A generic base implementation of <see cref="IUposDevice"/> that manages state transitions
/// via a modular mediator and lifecycle manager.</summary>
public abstract class UposDeviceBase
    : IUposDevice,
    IUposEventSink
{
    private readonly ReactiveProperty<bool> dataEventEnabled = new(false);
    private readonly Subject<UposDataEventArgs> dataSubject = new();
    private readonly Subject<UposErrorEventArgs> errorSubject = new();
    private readonly Subject<UposStatusUpdateEventArgs> statusUpdateSubject = new();
    private readonly Subject<UposDirectIoEventArgs> directIoSubject = new();
    private readonly Subject<UposOutputCompleteEventArgs> outputCompleteSubject = new();

    private bool autoDisable;
    private PowerNotify powerNotify = PowerNotify.Disabled;

    /// <summary>Initializes a new instance of the <see cref="UposDeviceBase"/> class with default mediator and handler.</summary>
    protected UposDeviceBase()
        : this(new UposMediator(), new StandardLifecycleHandler())
    {
    }

    /// <summary>Initializes a new instance of the <see cref="UposDeviceBase"/> class with provided mediator and handler.</summary>
    /// <param name="mediator">The state mediator.</param>
    /// <param name="handler">The lifecycle validation handler.</param>
    protected UposDeviceBase(IUposMediator mediator, IUposLifecycleHandler handler)
    {
        Mediator = mediator;
        Lifecycle = new UposLifecycleManager(mediator, handler);
    }

    // ------------------------------------------------------------------
    // Public Properties
    // ------------------------------------------------------------------

    /// <inheritdoc/>
    public ReadOnlyReactiveProperty<ControlState> State => Mediator.State;

    /// <inheritdoc/>
    public ReadOnlyReactiveProperty<bool> IsBusy => Mediator.IsBusy;

    /// <inheritdoc/>
    public bool IsBusyValue => Mediator.IsBusyValue;

    /// <inheritdoc/>
    public ReadOnlyReactiveProperty<UposErrorCode> LastError => Mediator.LastError;

    /// <inheritdoc/>
    public bool DataEventEnabled
    {
        get => dataEventEnabled.Value;
        set => dataEventEnabled.Value = value;
    }

    /// <inheritdoc/>
    public ReadOnlyReactiveProperty<bool> DataEventEnabledProperty => dataEventEnabled;

    /// <summary>Gets or sets a value indicating whether state verification is enabled.</summary>
    public bool IsStateVerificationEnabled
    {
        get => Lifecycle.IsStateVerificationEnabled;
        set => Lifecycle.IsStateVerificationEnabled = value;
    }

    /// <inheritdoc/>
    public bool IsOpen => Mediator.CurrentState != ControlState.Closed;

    /// <inheritdoc/>
    public bool IsClaimed => Mediator.CurrentState is ControlState.Claimed or ControlState.Enabled;

    /// <inheritdoc/>
    public bool IsEnabled => Mediator.CurrentState == ControlState.Enabled;

    /// <inheritdoc/>
    public string CheckHealthText => Mediator.CheckHealthText.CurrentValue;

    /// <inheritdoc/>
    public int ResultCodeExtended => Mediator.LastErrorExtended.CurrentValue;

    /// <inheritdoc/>
    public int DataCount => Mediator.DataCount.CurrentValue;

    /// <inheritdoc/>
    public bool AutoDisable
    {
        get => autoDisable;
        set => autoDisable = value;
    }

    /// <inheritdoc/>
    public virtual PowerReporting CapPowerReporting => PowerReporting.None;

    /// <inheritdoc/>
    public PowerNotify PowerNotify
    {
        get => powerNotify;
        set
        {
            if (CapPowerReporting == PowerReporting.None && value != PowerNotify.Disabled)
            {
                throw new UposException("Power notification is not supported by this device.", UposErrorCode.Illegal);
            }

            powerNotify = value;
        }
    }

    /// <inheritdoc/>
    public PowerState PowerState => Mediator.PowerState.CurrentValue;

    /// <inheritdoc/>
    public virtual string ServiceObjectDescription => "PosSharp Service Object";

    /// <inheritdoc/>
    public virtual string ServiceObjectVersion => "1.0";

    /// <inheritdoc/>
    public virtual string DeviceDescription => "PosSharp Virtual Device";

    /// <inheritdoc/>
    public virtual string DeviceName => GetType().Name;

    /// <inheritdoc/>
    public Observable<UposDataEventArgs> DataEvents => dataSubject;

    /// <inheritdoc/>
    public Observable<UposErrorEventArgs> ErrorEvents => errorSubject;

    /// <inheritdoc/>
    public Observable<UposStatusUpdateEventArgs> StatusUpdateEvents => statusUpdateSubject;

    /// <inheritdoc/>
    public Observable<UposDirectIoEventArgs> DirectIoEvents => directIoSubject;

    /// <inheritdoc/>
    public Observable<UposOutputCompleteEventArgs> OutputCompleteEvents => outputCompleteSubject;

    // ------------------------------------------------------------------
    // Protected Properties
    // ------------------------------------------------------------------

    /// <summary>Gets the mediator that coordinates state and operations.</summary>
    protected IUposMediator Mediator { get; }

    /// <summary>Gets the lifecycle manager that validates state transitions.</summary>
    protected UposLifecycleManager Lifecycle { get; }

    /// <summary>Gets the disposable container for subscriptions managed by this device.</summary>
    protected CompositeDisposable Disposables { get; } = new();

    /// <summary>Gets the internal reactive property for data event enabled state.</summary>
    protected ReactiveProperty<bool> DataEventEnabledInternal => dataEventEnabled;

    // ------------------------------------------------------------------
    // Public Methods
    // ------------------------------------------------------------------

    /// <inheritdoc/>
    public async Task OpenAsync(CancellationToken ct = default)
    {
        Lifecycle.PreOpen();
        await OnOpenAsync(ct);
        Lifecycle.PostOpen();
    }

    /// <inheritdoc/>
    public async Task CloseAsync(CancellationToken ct = default)
    {
        if (Mediator.CurrentState == ControlState.Closed)
        {
            return;
        }

        Lifecycle.PreClose();
        await OnCloseAsync(ct);
        Lifecycle.PostClose();
    }

    /// <inheritdoc/>
    public async Task ClaimAsync(int timeout, CancellationToken ct = default)
    {
        Lifecycle.PreClaim();
        await OnClaimAsync(timeout, ct);
        Lifecycle.PostClaim();
    }

    /// <inheritdoc/>
    public async Task ReleaseAsync(CancellationToken ct = default)
    {
        Lifecycle.PreRelease();
        await OnReleaseAsync(ct);
        Lifecycle.PostRelease();
    }

    /// <inheritdoc/>
    public async Task SetEnabledAsync(bool enabled, CancellationToken ct = default)
    {
        if (enabled)
        {
            Lifecycle.PreEnable();
            await OnEnableAsync(ct);
            Lifecycle.PostEnable();
        }
        else
        {
            Lifecycle.PreDisable();
            await OnDisableAsync(ct);
            Lifecycle.PostDisable();
        }
    }

    /// <inheritdoc/>
    public async Task CheckHealthAsync(HealthCheckLevel level, CancellationToken ct = default)
    {
        VerifyState(ControlState.Enabled);
        using (BeginOperation())
        {
            var result = await OnCheckHealthAsync(level, ct);
            Mediator.UpdateCheckHealthText(result);
        }
    }

    /// <inheritdoc/>
    public async Task DirectIOAsync(int command, int data, object obj, CancellationToken ct = default)
    {
        // DirectIO normally allowed in Claimed or Enabled states
        VerifyState(ControlState.Claimed, ControlState.Enabled);
        await OnDirectIOAsync(command, data, obj, ct);
    }

    /// <inheritdoc/>
    public async Task ClearInputAsync(CancellationToken ct = default)
    {
        VerifyState(ControlState.Claimed, ControlState.Enabled);
        await OnClearInputAsync(ct);
        Mediator.UpdateDataCount(0);
    }

    /// <inheritdoc/>
    public async Task ClearOutputAsync(CancellationToken ct = default)
    {
        VerifyState(ControlState.Claimed, ControlState.Enabled);
        await OnClearOutputAsync(ct);
    }

    /// <inheritdoc/>
    public virtual void Dispose()
    {
        dataSubject.Dispose();
        errorSubject.Dispose();
        statusUpdateSubject.Dispose();
        directIoSubject.Dispose();
        outputCompleteSubject.Dispose();

        dataEventEnabled.Dispose();
        Disposables.Dispose();
        Mediator.Dispose();

        GC.SuppressFinalize(this);
    }

    // ------------------------------------------------------------------
    // Protected Methods
    // ------------------------------------------------------------------

    /// <summary>
    /// Begins a device operation, setting the <see cref="IsBusy"/> property to true.
    /// Returns a disposable that automatically resets the busy state when disposed.
    /// </summary>
    /// <returns>A disposable to end the operation.</returns>
    /// <exception cref="UposStateException">The device is not enabled or already busy.</exception>
    protected IDisposable BeginOperation() => Mediator.BeginOperation();

    /// <summary>Verifies that the device is in one of the allowed states.</summary>
    /// <param name="allowedStates">The allowed states.</param>
    /// <exception cref="UposStateException">Thrown when verification fails.</exception>
    protected void VerifyState(params ControlState[] allowedStates) => Lifecycle.VerifyState(allowedStates);

    /// <summary>Publishes a DataEvent to subscribers.</summary>
    /// <param name="args">The event arguments.</param>
    protected void PublishDataEvent(UposDataEventArgs args) => dataSubject.OnNext(args);

    /// <summary>Publishes an ErrorEvent to subscribers.</summary>
    /// <param name="args">The event arguments.</param>
    protected void PublishErrorEvent(UposErrorEventArgs args) => errorSubject.OnNext(args);

    /// <summary>Publishes a StatusUpdateEvent to subscribers.</summary>
    /// <param name="args">The event arguments.</param>
    protected void PublishStatusUpdateEvent(UposStatusUpdateEventArgs args) => statusUpdateSubject.OnNext(args);

    /// <summary>Publishes a DirectIOEvent to subscribers.</summary>
    /// <param name="args">The event arguments.</param>
    protected void PublishDirectIoEvent(UposDirectIoEventArgs args) => directIoSubject.OnNext(args);

    /// <summary>Publishes an OutputCompleteEvent to subscribers.</summary>
    /// <param name="args">The event arguments.</param>
    protected void PublishOutputCompleteEvent(UposOutputCompleteEventArgs args) => outputCompleteSubject.OnNext(args);

    /// <summary>Updates the current power state and fires an event if notification is enabled.</summary>
    /// <param name="newState">The new power state.</param>
    protected void UpdatePowerState(PowerState newState)
    {
        if (Mediator.PowerState.CurrentValue == newState)
        {
            return;
        }

        Mediator.UpdatePowerState(newState);

        if (PowerNotify == PowerNotify.Enabled && CapPowerReporting != PowerReporting.None)
        {
            PublishStatusUpdateEvent(new UposStatusUpdateEventArgs((int)newState));
        }
    }

    /// <summary>Called by <see cref="CheckHealthAsync"/> to perform health check logic.</summary>
    /// <param name="level">Level of health check.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Checking result text.</returns>
    protected abstract Task<string> OnCheckHealthAsync(HealthCheckLevel level, CancellationToken ct);

    /// <summary>Called by <see cref="DirectIOAsync"/> to perform device-specific command logic.</summary>
    /// <param name="command">Command ID.</param>
    /// <param name="data">Input/output data.</param>
    /// <param name="obj">Input/output object.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task OnDirectIOAsync(int command, int data, object obj, CancellationToken ct);

    /// <summary>Called by <see cref="ClearInputAsync"/> to clear input buffers.</summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task OnClearInputAsync(CancellationToken ct);

    /// <summary>Called by <see cref="ClearOutputAsync"/> to clear output buffers.</summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task OnClearOutputAsync(CancellationToken ct);

    /// <summary>Called by <see cref="OpenAsync"/> to perform device-specific open logic.</summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task OnOpenAsync(CancellationToken ct);

    /// <summary>Called by <see cref="CloseAsync"/> to perform device-specific close logic.</summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task OnCloseAsync(CancellationToken ct);

    /// <summary>Called by <see cref="ClaimAsync"/> to perform device-specific claim logic.</summary>
    /// <param name="timeout">Timeout in milliseconds.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task OnClaimAsync(int timeout, CancellationToken ct);

    /// <summary>Called by <see cref="ReleaseAsync"/> to perform device-specific release logic.</summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task OnReleaseAsync(CancellationToken ct);

    /// <summary>Called by <see cref="SetEnabledAsync"/> when enabling the device.</summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task OnEnableAsync(CancellationToken ct);

    /// <summary>Called by <see cref="SetEnabledAsync"/> when disabling the device.</summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task OnDisableAsync(CancellationToken ct);
}
