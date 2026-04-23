namespace PosSharp.Abstractions;

/// <summary>Represents the base record for all UPOS event arguments.</summary>
public abstract class UposEventArgs : System.EventArgs;

/// <summary>Represents arguments for a DataEvent.</summary>
public sealed class UposDataEventArgs(int status) : UposEventArgs
{
    /// <summary>Gets the status of the data event.</summary>
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
    /// <summary>Gets the error code of the error event.</summary>
    public UposErrorCode ErrorCode { get; } = errorCode;

    /// <summary>Gets the extended error code of the error event.</summary>
    public int ExtendedErrorCode { get; } = extendedErrorCode;

    /// <summary>Gets the locus of the error event.</summary>
    public UposErrorLocus ErrorLocus { get; } = errorLocus;

    /// <summary>Gets the response to the error event.</summary>
    public UposErrorResponse ErrorResponse { get; } = errorResponse;
}

/// <summary>Represents arguments for a StatusUpdateEvent.</summary>
public sealed class UposStatusUpdateEventArgs(int status) : UposEventArgs
{
    /// <summary>Gets the status value of the status update event.</summary>
    public int Status { get; } = status;
}

/// <summary>Represents arguments for a DirectIOEvent.</summary>
public sealed class UposDirectIoEventArgs(int eventNumber, int data, object? objectData) : UposEventArgs
{
    /// <summary>Gets the event number of the DirectIO event.</summary>
    public int EventNumber { get; } = eventNumber;

    /// <summary>Gets the data value of the DirectIO event.</summary>
    public int Data { get; } = data;

    /// <summary>Gets the additional object data of the DirectIO event.</summary>
    public object? Object { get; } = objectData;
}

/// <summary>Represents arguments for an OutputCompleteEvent.</summary>
public sealed class UposOutputCompleteEventArgs(int outputId) : UposEventArgs
{
    /// <summary>Gets the output identifier of the output complete event.</summary>
    public int OutputId { get; } = outputId;
}
