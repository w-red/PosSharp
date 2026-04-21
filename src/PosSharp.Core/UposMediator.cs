using PosSharp.Abstractions;
using R3;

namespace PosSharp.Core;

/// <summary>A reactive implementation of the UPOS mediator.</summary>
public sealed class UposMediator
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
    public ReadOnlyReactiveProperty<ControlState> State => state;

    /// <inheritdoc />
    public ControlState CurrentState => state.Value;

    /// <inheritdoc />
    public ReadOnlyReactiveProperty<bool> IsBusy => isBusy;

    /// <inheritdoc />
    public bool IsBusyValue => isBusy.Value;

    /// <inheritdoc />
    public ReadOnlyReactiveProperty<UposErrorCode> LastError => lastError;

    /// <inheritdoc />
    public ReadOnlyReactiveProperty<int> LastErrorExtended => lastErrorExtended;

    /// <inheritdoc />
    public ReadOnlyReactiveProperty<string> CheckHealthText => checkHealthText;

    /// <inheritdoc />
    public ReadOnlyReactiveProperty<PowerState> PowerState => powerState;

    /// <inheritdoc />
    public ReadOnlyReactiveProperty<int> DataCount => dataCount;

    /// <inheritdoc />
    public void UpdateState(ControlState state)
    {
        this.state.Value = state;
    }

    /// <inheritdoc />
    public void SetBusy(bool isBusy)
    {
        this.isBusy.Value = isBusy;
    }

    /// <inheritdoc />
    public IDisposable BeginOperation()
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
    public void ReportError(UposErrorCode errorCode, int extendedCode = 0)
    {
        lastError.Value = errorCode;
        lastErrorExtended.Value = extendedCode;
    }

    /// <inheritdoc />
    public void UpdateCheckHealthText(string text)
    {
        checkHealthText.Value = text;
    }

    /// <inheritdoc />
    public void UpdatePowerState(PowerState powerState)
    {
        this.powerState.Value = powerState;
    }

    /// <inheritdoc />
    public void UpdateDataCount(int count)
    {
        dataCount.Value = count;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        state.Dispose();
        isBusy.Dispose();
        lastError.Dispose();
        lastErrorExtended.Dispose();
        checkHealthText.Dispose();
        powerState.Dispose();
        dataCount.Dispose();
        disposed = true;
    }
}
