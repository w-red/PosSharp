using System.Collections.Concurrent;
using PosSharp.Abstractions;
using PosSharp.Core.Lifecycle;
using R3;

namespace PosSharp.Core;

/// <summary>A generic base implementation of <see cref="IUposDevice"/> that manages state transitions
/// via a modular mediator and lifecycle manager.</summary>
public abstract class UposDeviceBase : IUposDevice, IUposEventSink
{
    private readonly ReactiveProperty<bool> dataEventEnabled = new(false);
    private readonly Subject<UposDataEventArgs> dataSubject = new();
    private readonly ConcurrentQueue<UposDataEventArgs> dataEventQueue = new();
    private readonly Subject<UposErrorEventArgs> errorSubject = new();
    private readonly Subject<UposStatusUpdateEventArgs> statusUpdateSubject = new();
    private readonly Subject<UposDirectIoEventArgs> directIoSubject = new();
    private readonly Subject<UposOutputCompleteEventArgs> outputCompleteSubject = new();
    private readonly IDisposable coreDisposables;

    private PowerNotify powerNotify = PowerNotify.Disabled;
    private UposCapabilities capabilities = UposCapabilities.Empty;
    private int isDataEventEnabledFlag;
    private int isFlushingFlag;
    private int disposedFlag;
    private DisposableBag extensionDisposables;

    /// <summary>Initializes a new instance of the <see cref="UposDeviceBase"/> class with default mediator and handler.</summary>
    protected UposDeviceBase()
        : this(new UposMediator(), new StandardLifecycleHandler()) { }

    /// <summary>Initializes a new instance of the <see cref="UposDeviceBase"/> class with provided mediator and handler.</summary>
    /// <param name="mediator">The state mediator.</param>
    /// <param name="handler">The lifecycle validation handler.</param>
    protected UposDeviceBase(IUposMediator mediator, IUposLifecycleHandler handler)
    {
        Mediator = mediator;
        Lifecycle = new UposLifecycleManager(mediator, handler);

        coreDisposables = Disposable.Combine(
            dataSubject,
            errorSubject,
            statusUpdateSubject,
            directIoSubject,
            outputCompleteSubject,
            dataEventEnabled,
            Mediator
        );
    }

    // ------------------------------------------------------------------
    // Public Properties
    // ------------------------------------------------------------------

    /// <inheritdoc/>
    public ReadOnlyReactiveProperty<ControlState> State => Mediator.State;
    
    /// <inheritdoc/>
    public UposCapabilities Capabilities => capabilities;

    /// <inheritdoc/>
    public ReadOnlyReactiveProperty<bool> IsBusy => Mediator.IsBusy;

    /// <inheritdoc/>
    public bool IsBusyValue => Mediator.IsBusyValue;

    /// <inheritdoc/>
    public ReadOnlyReactiveProperty<UposErrorCode> LastError => Mediator.LastError;

    /// <inheritdoc/>
    public bool DataEventEnabled
    {
        get
        {
            ThrowIfDisposed();
            return Volatile.Read(ref isDataEventEnabledFlag) == 1;
        }
        set
        {
            ThrowIfDisposed();
            int newVal = value ? 1 : 0;
            if (Interlocked.Exchange(ref isDataEventEnabledFlag, newVal) != newVal)
            {
                dataEventEnabled.Value = value;
                if (value)
                {
                    FlushDataEvents();
                }
            }
        }
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
    public bool AutoDisable { get; set; }

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

    /// <summary>Gets a value indicating whether buffered data events are currently being flushed.</summary>
    protected bool IsFlushing
    {
        get => Volatile.Read(ref isFlushingFlag) == 1;
        private set => Interlocked.Exchange(ref isFlushingFlag, value ? 1 : 0);
    }

    /// <summary>Tries to acquire the flushing lock atomically.</summary>
    /// <returns>True if the lock was acquired; otherwise, false.</returns>
    protected bool TryBeginFlushing() => Interlocked.CompareExchange(ref isFlushingFlag, 1, 0) == 0;

    // ------------------------------------------------------------------
    // Public Methods
    // ------------------------------------------------------------------

    /// <inheritdoc/>
    public async Task OpenAsync(CancellationToken ct = default)
    {
        ThrowIfDisposed();
        ct.ThrowIfCancellationRequested();

        Lifecycle.PreOpen();
        await OnOpenAsync(ct);
        Lifecycle.PostOpen();
    }

    /// <inheritdoc/>
    public async Task CloseAsync(CancellationToken ct = default)
    {
        ThrowIfDisposed();
        ct.ThrowIfCancellationRequested();

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
        ThrowIfDisposed();
        ct.ThrowIfCancellationRequested();

        var currentState = Mediator.CurrentState;
        if (currentState is
            ControlState.Claimed
            or ControlState.Enabled)
        {
            return;
        }

        Lifecycle.PreClaim();
        await OnClaimAsync(timeout, ct);
        Lifecycle.PostClaim();
    }

    /// <inheritdoc/>
    public async Task ReleaseAsync(CancellationToken ct = default)
    {
        ThrowIfDisposed();
        ct.ThrowIfCancellationRequested();

        Lifecycle.PreRelease();
        await OnReleaseAsync(ct);
        Lifecycle.PostRelease();
    }

    /// <inheritdoc/>
    public async Task SetEnabledAsync(bool enabled, CancellationToken ct = default)
    {
        ThrowIfDisposed();
        ct.ThrowIfCancellationRequested();

        var currentState = Mediator.CurrentState;
        if (enabled && currentState == ControlState.Enabled)
        {
            return;
        }

        if (!enabled && currentState == ControlState.Claimed)
        {
            return;
        }

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
        ThrowIfDisposed();
        ct.ThrowIfCancellationRequested();

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
        ThrowIfDisposed();
        ct.ThrowIfCancellationRequested();

        // DirectIO normally allowed in Claimed or Enabled states
        VerifyState(ControlState.Claimed, ControlState.Enabled);
        await OnDirectIOAsync(command, data, obj, ct);
    }

    /// <inheritdoc/>
    public async Task ClearInputAsync(CancellationToken ct = default)
    {
        ThrowIfDisposed();
        ct.ThrowIfCancellationRequested();

        VerifyState(ControlState.Claimed, ControlState.Enabled);
        await OnClearInputAsync(ct);

        while (dataEventQueue.TryDequeue(out _)) { }
        Mediator.UpdateDataCount(0);
    }

    /// <inheritdoc/>
    public async Task ClearOutputAsync(CancellationToken ct = default)
    {
        ThrowIfDisposed();
        ct.ThrowIfCancellationRequested();

        VerifyState(ControlState.Claimed, ControlState.Enabled);
        await OnClearOutputAsync(ct);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        // stryker disable all : Infrastructure
        Dispose(true);
        GC.SuppressFinalize(this);
        // stryker restore all
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        // stryker disable all : Infrastructure
        if (Interlocked.Exchange(ref disposedFlag, 1) != 0)
        {
            return;
        }
        // stryker restore all

        if (disposing)
        {
            dataSubject.OnCompleted();
            errorSubject.OnCompleted();
            statusUpdateSubject.OnCompleted();
            directIoSubject.OnCompleted();
            outputCompleteSubject.OnCompleted();

            coreDisposables.Dispose();
            extensionDisposables.Dispose();
        }

        // disposedFlag is already set
    }

    /// <summary>Throws an <see cref="ObjectDisposedException"/> if the device is disposed.</summary>
    protected void ThrowIfDisposed()
    {
        if (Volatile.Read(ref disposedFlag) == 1)
        {
            throw new ObjectDisposedException(
                GetType().FullName,
                "The UPOS device has been disposed and cannot be accessed."
            );
        }
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

    /// <summary>Initializes the device capabilities with a frozen collection.</summary>
    /// <param name="caps">The capabilities to freeze.</param>
    protected void InitializeCapabilities(IDictionary<string, object> caps)
    {
        capabilities = new UposCapabilities(caps);
    }

    /// <summary>Adds a disposable to the extension disposable bag.</summary>
    /// <param name="disposable">The disposable to add.</param>
    protected void AddDisposable(IDisposable disposable)
    {
        disposable.AddTo(ref extensionDisposables);
    }

    /// <summary>Adds multiple disposables to the extension disposable bag.</summary>
    /// <param name="disposables">The disposables to add.</param>
    protected void AddDisposables(params IDisposable[] disposables)
    {
        foreach (var d in disposables)
        {
            d.AddTo(ref extensionDisposables);
        }
    }

    /// <summary>Verifies that the device is in one of the allowed states.</summary>
    /// <param name="allowedStates">The allowed states.</param>
    /// <exception cref="UposStateException">Thrown when verification fails.</exception>
    protected void VerifyState(params ControlState[] allowedStates) => Lifecycle.VerifyState(allowedStates);

    /// <summary>Publishes a DataEvent to subscribers or buffers it if notification is disabled.</summary>
    /// <param name="args">The event arguments.</param>
    protected void PublishDataEvent(UposDataEventArgs args)
    {
        dataEventQueue.Enqueue(args);
        Mediator.UpdateDataCount(dataEventQueue.Count);

        if (DataEventEnabled)
        {
            FlushDataEvents();
        }
    }

    /// <summary>Flushes buffered data events to subscribers.</summary>
    protected void FlushDataEvents()
    {
        if (!TryBeginFlushing())
        {
            return;
        }

        try
        {
            while (true)
            {
                while (DataEventEnabled && dataEventQueue.TryDequeue(out var args))
                {
                    dataSubject.OnNext(args);
                    Mediator.UpdateDataCount(dataEventQueue.Count);

                    if (AutoDisable)
                    {
                        DataEventEnabled = false;
                        return;
                    }
                }

                IsFlushing = false;

                if (!DataEventEnabled || dataEventQueue.IsEmpty || !TryBeginFlushing())
                {
                    break;
                }
            }
        }
        finally
        {
            IsFlushing = false;
        }
    }

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
        if (PowerNotify == PowerNotify.Disabled)
        {
            return;
        }

        Mediator.UpdatePowerState(newState);

        if (CapPowerReporting != PowerReporting.None)
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
