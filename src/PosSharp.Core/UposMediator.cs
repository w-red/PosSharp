using PosSharp.Abstractions;
using R3;

namespace PosSharp.Core;

/// <summary>A reactive implementation of the UPOS mediator.</summary>
public class UposMediator : IUposMediator
{
    private readonly ReactiveProperty<ControlState> state = new(ControlState.Closed);
    private readonly ReactiveProperty<bool> isBusy = new(false);
    private readonly ReactiveProperty<UposErrorCode> lastError = new(UposErrorCode.Success);
    private readonly ReactiveProperty<int> lastErrorExtended = new(0);
    private readonly ReactiveProperty<string> checkHealthText = new(string.Empty);
    private readonly ReactiveProperty<PowerState> powerState = new(Abstractions.PowerState.Unknown);
    private readonly ReactiveProperty<int> dataCount = new(0);
    private readonly IDisposable disposables;

    private readonly AtomicState<MediatorSnapshot> snapshot = new(MediatorSnapshot.Initial);
    private int disposedFlag;

    /// <summary>Initializes a new instance of the <see cref="UposMediator"/> class.</summary>
    public UposMediator()
    {
        disposables = Disposable.Combine(
            state,
            isBusy,
            lastError,
            lastErrorExtended,
            checkHealthText,
            powerState,
            dataCount
        );
    }

    /// <inheritdoc />
    public virtual ReadOnlyReactiveProperty<ControlState> State => state;

    /// <inheritdoc />
    public virtual ControlState CurrentState => snapshot.Current.State;

    /// <inheritdoc />
    public virtual ReadOnlyReactiveProperty<bool> IsBusy => isBusy;

    /// <inheritdoc />
    public virtual bool IsBusyValue => snapshot.Current.IsBusy;

    /// <inheritdoc />
    public virtual ReadOnlyReactiveProperty<UposErrorCode> LastError => lastError;

    /// <inheritdoc />
    public virtual ReadOnlyReactiveProperty<int> LastErrorExtended => lastErrorExtended;

    /// <inheritdoc />
    public virtual ReadOnlyReactiveProperty<string> CheckHealthText => checkHealthText;

    /// <inheritdoc />
    public virtual ReadOnlyReactiveProperty<PowerState> PowerState => powerState;

    /// <inheritdoc />
    public virtual ReadOnlyReactiveProperty<int> DataCount => dataCount;

    /// <inheritdoc />
    public virtual void UpdateState(ControlState state)
    {
        var result = snapshot.Transition(s => s.State == state ? s : s with { State = state });
        if (result.Changed)
        {
            this.state.Value = state;
        }
    }

    /// <inheritdoc />
    public virtual void SetBusy(bool isBusy)
    {
        var result = snapshot.Transition(s => s.IsBusy == isBusy ? s : s with { IsBusy = isBusy });
        if (result.Changed)
        {
            this.isBusy.Value = isBusy;
        }
    }

    /// <inheritdoc />
    public virtual IDisposable BeginOperation()
    {
        var result = snapshot.Transition(s => s.IsBusy ? s : s with { IsBusy = true });

        if (!result.Changed)
        {
            throw new UposStateException("Device is already busy.", UposErrorCode.Busy);
        }

        // Now we are atomically marked as busy in the snapshot.
        isBusy.Value = true;

        try
        {
            if (result.NewState.State != ControlState.Enabled)
            {
                // Stryker disable all : Exception message
                throw new UposStateException(
                    $"Operation requires Enabled state, but current state is {result.NewState.State}.",
                    UposErrorCode.Disabled
                );
                // Stryker restore all
            }
        }
        catch
        {
            // Reset if validation fails
            snapshot.Transition(s => s with { IsBusy = false });
            isBusy.Value = false;
            throw;
        }

        return Disposable.Create(() =>
        {
            snapshot.Transition(s => s with { IsBusy = false });
            isBusy.Value = false;
        });
    }

    /// <inheritdoc />
    public virtual void ReportError(UposErrorCode errorCode, int extendedCode = 0)
    {
        var result = snapshot.Transition(s =>
            s.LastError == errorCode && s.LastErrorExtended == extendedCode
                ? s
                : s with { LastError = errorCode, LastErrorExtended = extendedCode });

        if (result.Changed)
        {
            if (result.OldState.LastError != result.NewState.LastError)
            {
                lastError.Value = errorCode;
            }

            if (result.OldState.LastErrorExtended != result.NewState.LastErrorExtended)
            {
                lastErrorExtended.Value = extendedCode;
            }
        }
    }

    /// <inheritdoc />
    public virtual void UpdateCheckHealthText(string text)
    {
        checkHealthText.Value = text;
    }

    /// <inheritdoc />
    public virtual void UpdatePowerState(PowerState powerState)
    {
        this.powerState.Value = powerState;
    }

    /// <inheritdoc />
    public virtual void UpdateDataCount(int count)
    {
        var result = snapshot.Transition(s => s.DataCount == count ? s : s with { DataCount = count });
        if (result.Changed)
        {
            dataCount.Value = count;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // stryker disable all : Infrastructure
        Dispose(true);
        GC.SuppressFinalize(this);
        // stryker restore all
    }

    /// <summary> Releases the unmanaged resources used by the <see cref="UposMediator"/> and optionally releases the managed resources.</summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        // Stryker disable all : Infrastructure cleanup logic
        if (Interlocked.Exchange(ref disposedFlag, 1) != 0)
        {
            return;
        }

        if (disposing)
        {
            disposables.Dispose();
        }

        // disposedFlag is already set
        // Stryker restore all
    }
}
