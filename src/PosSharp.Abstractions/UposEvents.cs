namespace PosSharp.Abstractions;

/// <summary>Represents the base record for all UPOS event arguments.</summary>
public abstract class UposEventArgs : System.EventArgs;

/// <summary>Represents arguments for a DataEvent.</summary>
public sealed class UposDataEventArgs(int status) : UposEventArgs
{
    public int Status { get; } = status;
}

/// <summary>Represents arguments for an ErrorEvent.</summary>
public sealed class UposErrorEventArgs(
    UposErrorCode errorCode,
    int extendedErrorCode,
    UposErrorLocus errorLocus,
    UposErrorResponse errorResponse
) : UposEventArgs
{
    public UposErrorCode ErrorCode { get; } = errorCode;
    public int ExtendedErrorCode { get; } = extendedErrorCode;
    public UposErrorLocus ErrorLocus { get; } = errorLocus;
    public UposErrorResponse ErrorResponse { get; } = errorResponse;
}

/// <summary>Represents arguments for a StatusUpdateEvent.</summary>
public sealed class UposStatusUpdateEventArgs(int status) : UposEventArgs
{
    public int Status { get; } = status;
}

/// <summary>Represents arguments for a DirectIOEvent.</summary>
public sealed class UposDirectIoEventArgs(int eventNumber, int data, object? objectData) : UposEventArgs
{
    public int EventNumber { get; } = eventNumber;
    public int Data { get; } = data;
    public object? Object { get; } = objectData;
}

/// <summary>Represents arguments for an OutputCompleteEvent.</summary>
public sealed class UposOutputCompleteEventArgs(int outputId) : UposEventArgs
{
    public int OutputId { get; } = outputId;
}
