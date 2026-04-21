namespace PosSharp.Abstractions;

/// <summary>Represents standard UPOS result codes.</summary>
public enum UposErrorCode
{
    /// <summary>Operation completed successfully.</summary>
    Success = 0,

    /// <summary>The device is closed. (OPOS_E_CLOSED)</summary>
    Closed = 101,

    /// <summary>The device is claimed by another instance. (OPOS_E_CLAIMED)</summary>
    Claimed = 102,

    /// <summary>The device is not open. (OPOS_E_NOTOPEN)</summary>
    NotOpen = 103,

    /// <summary>The device is not enabled. (OPOS_E_DISABLED)</summary>
    Disabled = 104,

    /// <summary>The device is not claimed. (OPOS_E_NOTCLAIMED)</summary>
    NotClaimed = 105,

    /// <summary>Invalid parameter or property value. (OPOS_E_ILLEGAL)</summary>
    Illegal = 106,

    /// <summary>The physical hardware is not found. (OPOS_E_NOHARDWARE)</summary>
    NoHardware = 107,

    /// <summary>The device is offline. (OPOS_E_OFFLINE)</summary>
    Offline = 108,

    /// <summary>The service is not available. (OPOS_E_NOSERVICE)</summary>
    NoService = 109,

    /// <summary>General hardware error. (OPOS_E_FAILURE)</summary>
    Failure = 111,

    /// <summary>Operation timed out. (OPOS_E_TIMEOUT)</summary>
    Timeout = 112,

    /// <summary>Device is busy with another operation. (OPOS_E_BUSY)</summary>
    Busy = 113,

    /// <summary>Device is in an error state. (OPOS_E_EXTENDED)</summary>
    Extended = 114,
}
