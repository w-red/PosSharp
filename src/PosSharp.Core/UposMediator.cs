using PosSharp.Abstractions;
using R3;

namespace PosSharp.Core;

/// <summary>A reactive implementation of the UPOS mediator.</summary>
public class UposMediator : IUposMediator
{
    private readonly ReactiveProperty<ControlState> state = new(Abstractions.ControlState.Closed);
    private readonly ReactiveProperty<bool> isBusy = new(false);
    private readonly ReactiveProperty<UposErrorCode> lastError = new(UposErrorCode.Success);
    private readonly ReactiveProperty<int> lastErrorExtended = new(0);
    private readonly ReactiveProperty<string> checkHealthText = new(string.Empty);
    private readonly ReactiveProperty<PowerState> powerState = new(Abstractions.PowerState.Unknown);
    private readonly ReactiveProperty<int> dataCount = new(0);
    private readonly IDisposable disposables;

    private int currentState = (int)Abstractions.ControlState.Closed;
    private int currentLastError = (int)UposErrorCode.Success;
    private int currentLastErrorExtended;
    private int currentDataCount;
    private int busyFlag;
    private bool disposed;

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
    public virtual ControlState CurrentState => (ControlState)Volatile.Read(ref currentState);

    /// <inheritdoc />
    public virtual ReadOnlyReactiveProperty<bool> IsBusy => isBusy;

    /// <inheritdoc />
    public virtual bool IsBusyValue => Volatile.Read(ref busyFlag) == 1;

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
        if (Interlocked.Exchange(ref currentState, (int)state) != (int)state)
        {
            this.state.Value = state;
        }
    }

    /// <inheritdoc />
    public virtual void SetBusy(bool isBusy)
    {
        int newVal = isBusy ? 1 : 0;
        if (Interlocked.Exchange(ref busyFlag, newVal) != newVal)
        {
            this.isBusy.Value = isBusy;
        }
    }

    /// <inheritdoc />
    public virtual IDisposable BeginOperation()
    {
        if (Interlocked.CompareExchange(ref busyFlag, 1, 0) != 0)
        {
            throw new UposStateException("Device is already busy.");
        }

        // Now we are atomically marked as busy.
        isBusy.Value = true;

        try
        {
            if (currentState != (int)ControlState.Enabled)
            {
                throw new UposStateException(
                    $"Operation requires Enabled state, but current state is {(ControlState)currentState}."
                );
            }
        }
        catch
        {
            // Reset if validation fails
            Interlocked.Exchange(ref busyFlag, 0);
            isBusy.Value = false;
            throw;
        }

        return Disposable.Create(() =>
        {
            Interlocked.Exchange(ref busyFlag, 0);
            isBusy.Value = false;
        });
    }

    /// <inheritdoc />
    public virtual void ReportError(UposErrorCode errorCode, int extendedCode = 0)
    {
        bool changed = false;
        if (Interlocked.Exchange(ref currentLastError, (int)errorCode) != (int)errorCode)
        {
            lastError.Value = errorCode;
            changed = true;
        }

        if (Interlocked.Exchange(ref currentLastErrorExtended, extendedCode) != extendedCode)
        {
            lastErrorExtended.Value = extendedCode;
            changed = true;
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
        if (Interlocked.Exchange(ref currentDataCount, count) != count)
        {
            dataCount.Value = count;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary> Releases the unmanaged resources used by the <see cref="UposMediator"/> and optionally releases the managed resources.</summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }

        if (disposing)
        {
            disposables.Dispose();
        }

        disposed = true;
    }
}
