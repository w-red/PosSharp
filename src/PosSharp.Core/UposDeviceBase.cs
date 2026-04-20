// Copyright (c) PosSharp Project. All rights reserved.
// Licensed under the MIT License.

using PosSharp.Abstractions;
using R3;

namespace PosSharp.Core;

/// <summary>
/// A generic base implementation of <see cref="IUposDevice"/> that manages state transitions.
/// </summary>
public abstract class UposDeviceBase : IUposDevice, IUposEventSink
{
    private readonly ReactiveProperty<ControlState> state = new(ControlState.Closed);
    private readonly ReactiveProperty<bool> isBusy = new(false);
    private readonly ReactiveProperty<UposErrorCode> lastError = new(UposErrorCode.Success);

    private readonly Subject<UposDataEventArgs> dataSubject = new();
    private readonly Subject<UposErrorEventArgs> errorSubject = new();
    private readonly Subject<UposStatusUpdateEventArgs> statusUpdateSubject = new();
    private readonly Subject<UposDirectIoEventArgs> directIoSubject = new();
    private readonly Subject<UposOutputCompleteEventArgs> outputCompleteSubject = new();

    // ------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------

    /// <inheritdoc/>
    public ReadOnlyReactiveProperty<ControlState> State => this.state;

    /// <inheritdoc/>
    public ReadOnlyReactiveProperty<bool> IsBusy => this.isBusy;

    /// <inheritdoc/>
    public ReadOnlyReactiveProperty<UposErrorCode> LastError => this.lastError;

    /// <inheritdoc/>
    public IObservable<UposDataEventArgs> DataEvents => this.dataSubject.AsSystemObservable();

    /// <inheritdoc/>
    public IObservable<UposErrorEventArgs> ErrorEvents => this.errorSubject.AsSystemObservable();

    /// <inheritdoc/>
    public IObservable<UposStatusUpdateEventArgs> StatusUpdateEvents => this.statusUpdateSubject.AsSystemObservable();

    /// <inheritdoc/>
    public IObservable<UposDirectIoEventArgs> DirectIoEvents => this.directIoSubject.AsSystemObservable();

    /// <inheritdoc/>
    public IObservable<UposOutputCompleteEventArgs> OutputCompleteEvents => this.outputCompleteSubject.AsSystemObservable();

    /// <summary>Gets the disposable container for subscriptions managed by this device.</summary>
    protected CompositeDisposable Disposables { get; } = new();

    // ------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------

    /// <inheritdoc/>
    public async Task OpenAsync(CancellationToken ct = default)
    {
        this.VerifyState(ControlState.Closed);
        await this.OnOpenAsync(ct);
        this.state.Value = ControlState.Idle;
    }

    /// <inheritdoc/>
    public async Task CloseAsync(CancellationToken ct = default)
    {
        if (this.state.Value == ControlState.Closed)
        {
            return;
        }

        await this.OnCloseAsync(ct);
        this.state.Value = ControlState.Closed;
    }

    /// <inheritdoc/>
    public async Task ClaimAsync(int timeout, CancellationToken ct = default)
    {
        this.VerifyState(ControlState.Idle);
        await this.OnClaimAsync(timeout, ct);
        this.state.Value = ControlState.Claimed;
    }

    /// <inheritdoc/>
    public async Task ReleaseAsync(CancellationToken ct = default)
    {
        this.VerifyState(ControlState.Claimed, ControlState.Enabled);
        await this.OnReleaseAsync(ct);
        this.state.Value = ControlState.Idle;
    }

    /// <inheritdoc/>
    public async Task SetEnabledAsync(bool enabled, CancellationToken ct = default)
    {
        if (enabled)
        {
            this.VerifyState(ControlState.Claimed);
            await this.OnEnableAsync(ct);
            this.state.Value = ControlState.Enabled;
        }
        else
        {
            this.VerifyState(ControlState.Enabled);
            await this.OnDisableAsync(ct);
            this.state.Value = ControlState.Claimed;
        }
    }

    /// <inheritdoc/>
    public virtual void Dispose()
    {
        this.dataSubject.Dispose();
        this.errorSubject.Dispose();
        this.statusUpdateSubject.Dispose();
        this.directIoSubject.Dispose();
        this.outputCompleteSubject.Dispose();

        this.Disposables.Dispose();
        this.state.Dispose();
        this.isBusy.Dispose();
        this.lastError.Dispose();

        GC.SuppressFinalize(this);
    }

    // ------------------------------------------------------------------
    // Protected members
    // ------------------------------------------------------------------

    /// <summary>
    /// Begins a device operation, setting the <see cref="IsBusy"/> property to true.
    /// Returns a disposable that automatically resets the busy state when disposed.
    /// </summary>
    /// <returns>A disposable to end the operation.</returns>
    /// <exception cref="UposStateException">The device is not enabled or already busy.</exception>
    protected IDisposable BeginOperation()
    {
        this.VerifyState(ControlState.Enabled);

        if (this.isBusy.Value)
        {
            throw new InvalidOperationException("Device is already busy with another operation.");
        }

        this.isBusy.Value = true;
        return new OperationGuard(this);
    }

    /// <summary>Verifies that the current state is one of the allowed states.</summary>
    /// <param name="allowedStates">The states that are permitted at this point.</param>
    /// <exception cref="UposStateException">
    /// Thrown when the current state is not one of the <paramref name="allowedStates"/>.
    /// </exception>
    protected void VerifyState(params ControlState[] allowedStates)
    {
        if (!allowedStates.Contains(this.state.Value))
        {
            throw new UposStateException(this.state.Value, allowedStates);
        }
    }

    /// <summary>Publishes a DataEvent to subscribers.</summary>
    /// <param name="args">The event arguments.</param>
    protected void PublishDataEvent(UposDataEventArgs args) => this.dataSubject.OnNext(args);

    /// <summary>Publishes an ErrorEvent to subscribers.</summary>
    /// <param name="args">The event arguments.</param>
    protected void PublishErrorEvent(UposErrorEventArgs args) => this.errorSubject.OnNext(args);

    /// <summary>Publishes a StatusUpdateEvent to subscribers.</summary>
    /// <param name="args">The event arguments.</param>
    protected void PublishStatusUpdateEvent(UposStatusUpdateEventArgs args) => this.statusUpdateSubject.OnNext(args);

    /// <summary>Publishes a DirectIOEvent to subscribers.</summary>
    /// <param name="args">The event arguments.</param>
    protected void PublishDirectIoEvent(UposDirectIoEventArgs args) => this.directIoSubject.OnNext(args);

    /// <summary>Publishes an OutputCompleteEvent to subscribers.</summary>
    /// <param name="args">The event arguments.</param>
    protected void PublishOutputCompleteEvent(UposOutputCompleteEventArgs args) => this.outputCompleteSubject.OnNext(args);

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

    private sealed class OperationGuard : IDisposable
    {
        private readonly UposDeviceBase device;

        public OperationGuard(UposDeviceBase device) => this.device = device;

        public void Dispose() => this.device.isBusy.Value = false;
    }
}
