namespace PosSharp.Abstractions;

/// <summary>Represents the logical state of a UPOS device.</summary>
public enum ControlState
{
    /// <summary>The device is closed and not initialized.</summary>
    Closed = 0,

    /// <summary>The device is open but not yet claimed.</summary>
    Idle = 1,

    /// <summary>The device is open and claimed, but not yet enabled.</summary>
    Claimed = 2,

    /// <summary>The device is open, claimed, and enabled.</summary>
    Enabled = 3,

    /// <summary>The device is currently executing an operation.</summary>
    Busy = 4,

    /// <summary>The device has encountered an unrecoverable error.</summary>
    Error = 5,
}
