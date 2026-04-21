namespace PosSharp.Abstractions;

/// <summary>Provides common status codes used across multiple UPOS device categories.</summary>
public static class UposCommonCodes
{
    /// <summary>StatusUpdateEvent: Power status - The device is powered on.</summary>
    public const int PowerOnline = 2001;

    /// <summary>StatusUpdateEvent: Power status - The device is powered off or detached.</summary>
    public const int PowerOffline = 2002;

    /// <summary>StatusUpdateEvent: Power status - The device is powered on, but it is in a low-power state.</summary>
    public const int PowerIneligible = 2003;

    /// <summary>StatusUpdateEvent: Power status - The device is powered on, but its power state cannot be determined.</summary>
    public const int PowerOfflineUnspecified = 2004;
}
