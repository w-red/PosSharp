using PosSharp.Abstractions;
using R3;

namespace PosSharp.Core;

/// <summary>A reactive implementation of the UPOS mediator.</summary>
/// <summary>A reactive implementation of the UPOS mediator.</summary>
public class UposMediator
    : IUposMediator
{
    private readonly ReactiveProperty<ControlState> state = new(Abstractions.ControlState.Closed);
    private readonly ReactiveProperty<bool> isBusy = new(false);
    private readonly ReactiveProperty<UposErrorCode> lastError = new(UposErrorCode.Success);
    private readonly ReactiveProperty<int> lastErrorExtended = new(0);
    private readonly ReactiveProperty<string> checkHealthText = new(string.Empty);
    private readonly ReactiveProperty<PowerState> powerState = new(Abstractions.PowerState.Unknown);
    private readonly ReactiveProperty<int> dataCount = new(0);
    private bool disposed;

    /// <inheritdoc />
    public virtual ReadOnlyReactiveProperty<ControlState> State => state;

    /// <inheritdoc />
    public virtual ControlState CurrentState => state.Value;

    /// <inheritdoc />
    public virtual ReadOnlyReactiveProperty<bool> IsBusy => isBusy;

    /// <inheritdoc />
    public virtual bool IsBusyValue => isBusy.Value;

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
        this.state.Value = state;
    }

    /// <inheritdoc />
    public virtual void SetBusy(bool isBusy)
    {
        this.isBusy.Value = isBusy;
    }

    /// <inheritdoc />
    public virtual IDisposable BeginOperation()
    {
        if (isBusy.Value)
        {
            throw new UposStateException("Device is already busy.");
        }

        if (state.Value != ControlState.Enabled)
        {
            throw new UposStateException($"Operation requires Enabled state, but current state is {state.Value}.");
        }

        isBusy.Value = true;
        return Disposable.Create(() => isBusy.Value = false);
    }

    /// <inheritdoc />
    public virtual void ReportError(UposErrorCode errorCode, int extendedCode = 0)
    {
        lastError.Value = errorCode;
        lastErrorExtended.Value = extendedCode;
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
        dataCount.Value = count;
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
            state.Dispose();
            isBusy.Dispose();
            lastError.Dispose();
            lastErrorExtended.Dispose();
            checkHealthText.Dispose();
            powerState.Dispose();
            dataCount.Dispose();
        }

        disposed = true;
    }
}
