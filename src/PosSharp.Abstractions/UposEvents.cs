namespace PosSharp.Abstractions;

/// <summary>Represents the base record for all UPOS event arguments.</summary>
public abstract record UposEventArgs;

/// <summary>Represents arguments for a DataEvent.</summary>
/// <param name="Status">The input status information.</param>
public sealed record UposDataEventArgs(int Status) : UposEventArgs;

/// <summary>Represents arguments for an ErrorEvent.</summary>
/// <param name="ErrorCode">The error code.</param>
/// <param name="ExtendedErrorCode">The extended error code.</param>
/// <param name="ErrorLocus">The locus of the error (where it occurred).</param>
/// <param name="ErrorResponse">The response to the error.</param>
public sealed record UposErrorEventArgs(
    UposErrorCode ErrorCode,
    int ExtendedErrorCode,
    UposErrorLocus ErrorLocus,
    UposErrorResponse ErrorResponse
) : UposEventArgs;

/// <summary>Represents arguments for a StatusUpdateEvent.</summary>
/// <param name="Status">The status information.</param>
public sealed record UposStatusUpdateEventArgs(int Status) : UposEventArgs;

/// <summary>Represents arguments for a DirectIOEvent.</summary>
/// <param name="EventNumber">The event number.</param>
/// <param name="Data">The data associated with the event.</param>
/// <param name="Object">The object associated with the event.</param>
public sealed record UposDirectIoEventArgs(int EventNumber, int Data, object? Object) : UposEventArgs;

/// <summary>Represents arguments for an OutputCompleteEvent.</summary>
/// <param name="OutputId">The ID of the completed output operation.</param>
public sealed record UposOutputCompleteEventArgs(int OutputId) : UposEventArgs;
