namespace PosSharp.Abstractions;

/// <summary>Exception thrown when a UPOS operation fails with a specific result code.</summary>
public class UposException : Exception
{
    /// <summary>Initializes a new instance of the <see cref="UposException"/> class.</summary>
    /// <param name="message">The health check results text.</param>
    /// <param name="errorCode">The standard UPOS error code.</param>
    /// <param name="extendedErrorCode">The optional extended error code.</param>
    public UposException(string message, UposErrorCode errorCode, int extendedErrorCode = 0)
        : base(message)
    {
        ErrorCode = errorCode;
        ExtendedErrorCode = extendedErrorCode;
    }

    /// <summary>Gets the standard UPOS error code associated with this exception.</summary>
    public UposErrorCode ErrorCode { get; }

    /// <summary>Gets the extended error code associated with this exception.</summary>
    public int ExtendedErrorCode { get; }
}
