namespace PosSharp.Abstractions;

/// <summary>Specifies the level of health check to perform.</summary>
public enum HealthCheckLevel
{
    /// <summary>Performs an internal health check.</summary>
    Internal = 1,

    /// <summary>Performs an external health check.</summary>
    External = 2,

    /// <summary>Performs an interactive health check.</summary>
    Interactive = 3,
}

/// <summary>Specifies the power reporting capabilities of the device.</summary>
public enum PowerReporting
{
    /// <summary>Power reporting is not supported.</summary>
    None = 0,

    /// <summary>Basic power reporting is supported.</summary>
    Standard = 1,

    /// <summary>Advanced power reporting is supported.</summary>
    Advanced = 2,
}

/// <summary>Specifies the current power state of the device.</summary>
public enum PowerState
{
    /// <summary>The power state is unknown or not supported.</summary>
    Unknown = 0,

    /// <summary>The device is powered on.</summary>
    Online = 2001,

    /// <summary>The device is powered off.</summary>
    Off = 2002,

    /// <summary>The device is powered on but offline.</summary>
    Offline = 2003,

    /// <summary>The device is powered off and offline.</summary>
    OffOffline = 2004,
}

/// <summary>Specifies the power notification mode.</summary>
public enum PowerNotify
{
    /// <summary>Power notification is disabled.</summary>
    Disabled = 0,

    /// <summary>Power notification is enabled.</summary>
    Enabled = 1,
}

/// <summary>Specifies the locus of a device error.</summary>
public enum UposErrorLocus
{
    /// <summary>The error occurred during an output operation.</summary>
    Output = 1,

    /// <summary>The error occurred during an input operation.</summary>
    Input = 2,

    /// <summary>The error occurred during an input operation while data was being queued.</summary>
    InputData = 3,
}

/// <summary>Specifies the response to a device error.</summary>
public enum UposErrorResponse
{
    /// <summary>The error should be ignored.</summary>
    Retry = 1,

    /// <summary>The error should be reported as clear.</summary>
    Clear = 2,
}
