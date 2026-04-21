namespace PosSharp.Abstractions;

/// <summary>Represents standard UPOS result codes.</summary>
public enum UposErrorCode
{
    /// <summary>Operation completed successfully.</summary>
    Success = 0,

    /// <summary>The device is closed.</summary>
    Closed = 101,

    /// <summary>The device is not claimed.</summary>
    NotClaimed = 102,

    /// <summary>The device is not enabled.</summary>
    Disabled = 103,

    /// <summary>Operation timed out.</summary>
    Timeout = 106,

    /// <summary>Device is busy with another operation.</summary>
    Busy = 107,

    /// <summary>General hardware error.</summary>
    Failure = 111,

    /// <summary>Invalid parameter or property value.</summary>
    Illegal = 114,

    /// <summary>Device is in an error state.</summary>
    Extended = 115,
}
