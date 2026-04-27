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

    private int currentState = (int)ControlState.Closed;
    private int currentLastError = (int)UposErrorCode.Success;
    private int currentLastErrorExtended;
    private int currentDataCount;
    private int isBusyFlag;
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
    public virtual ControlState CurrentState => CurrentStateInternal;

    /// <inheritdoc />
    public virtual ReadOnlyReactiveProperty<bool> IsBusy => isBusy;

    /// <inheritdoc />
    public virtual bool IsBusyValue
    {
        get => Volatile.Read(ref isBusyFlag) == 1;
        private set => Interlocked.Exchange(ref isBusyFlag, value ? 1 : 0);
    }

    /// <summary>Tries to acquire the busy lock atomically.</summary>
    /// <returns>True if the lock was acquired; otherwise, false.</returns>
    protected bool TryAcquireBusyLock() => Interlocked.CompareExchange(ref isBusyFlag, 1, 0) == 0;

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

    /// <summary>Gets the current logical state of the device.</summary>
    protected ControlState CurrentStateInternal
    {
        get => (ControlState)Volatile.Read(ref currentState);
        set => Interlocked.Exchange(ref currentState, (int)value);
    }

    /// <summary>Gets the last error code encountered by the device.</summary>
    protected UposErrorCode LastErrorInternal
    {
        get => (UposErrorCode)Volatile.Read(ref currentLastError);
        set => Interlocked.Exchange(ref currentLastError, (int)value);
    }

    /// <summary>Gets the extended result code of the last completed operation.</summary>
    protected int LastErrorExtendedInternal
    {
        get => Volatile.Read(ref currentLastErrorExtended);
        set => Interlocked.Exchange(ref currentLastErrorExtended, value);
    }

    /// <summary>Gets the current count of queued data events.</summary>
    protected int DataCountInternal
    {
        get => Volatile.Read(ref currentDataCount);
        set => Interlocked.Exchange(ref currentDataCount, value);
    }

    /// <inheritdoc />
    public virtual void UpdateState(ControlState state)
    {
        if (CurrentStateInternal != state)
        {
            CurrentStateInternal = state;
            this.state.Value = state;
        }
    }

    /// <inheritdoc />
    public virtual void SetBusy(bool isBusy)
    {
        if (IsBusyValue != isBusy)
        {
            IsBusyValue = isBusy;
            this.isBusy.Value = isBusy;
        }
    }

    /// <inheritdoc />
    public virtual IDisposable BeginOperation()
    {
        if (!TryAcquireBusyLock())
        {
            throw new UposStateException("Device is already busy.", UposErrorCode.Busy);
        }

        // Now we are atomically marked as busy.
        isBusy.Value = true;

        try
        {
            if (CurrentStateInternal != ControlState.Enabled)
            {
                // Stryker disable all : Exception message
                throw new UposStateException(
                    $"Operation requires Enabled state, but current state is {CurrentStateInternal}.",
                    UposErrorCode.Disabled
                );
                // Stryker restore all
            }
        }
        catch
        {
            // Reset if validation fails
            IsBusyValue = false;
            isBusy.Value = false;
            throw;
        }

        return Disposable.Create(() =>
        {
            IsBusyValue = false;
            isBusy.Value = false;
        });
    }

    /// <inheritdoc />
    public virtual void ReportError(UposErrorCode errorCode, int extendedCode = 0)
    {
        if (LastErrorInternal != errorCode)
        {
            LastErrorInternal = errorCode;
            lastError.Value = errorCode;
        }

        if (LastErrorExtendedInternal != extendedCode)
        {
            LastErrorExtendedInternal = extendedCode;
            lastErrorExtended.Value = extendedCode;
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
        if (DataCountInternal != count)
        {
            DataCountInternal = count;
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
