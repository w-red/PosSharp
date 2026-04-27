using PosSharp.Abstractions;

namespace PosSharp.Core;

/// <summary>Provides extension methods for <see cref="IUposMediator"/>.</summary>
public static class UposMediatorExtensions
{
    /// <summary>Validates that the device is in at least the Open state.</summary>
    /// <param name="mediator">The mediator.</param>
    /// <exception cref="UposStateException">The device is closed.</exception>
    public static void ValidateOpen(this IUposMediator mediator)
    {
        if (mediator.CurrentState == ControlState.Closed)
        {
            throw new UposStateException("Device must be open to perform this operation.", UposErrorCode.Closed);
        }
    }

    /// <summary>Validates that the device is in at least the Claimed state.</summary>
    /// <param name="mediator">The mediator.</param>
    /// <exception cref="UposStateException">The device is closed.</exception>
    public static void ValidateClaimed(this IUposMediator mediator)
    {
        if (mediator.CurrentState < ControlState.Claimed)
        {
            throw new UposStateException("Device must be claimed to perform this operation.", UposErrorCode.NotClaimed);
        }
    }

    /// <summary>Validates that the device is in the Enabled state.</summary>
    /// <param name="mediator">The mediator.</param>
    /// <exception cref="UposStateException">The device is not enabled.</exception>
    public static void ValidateEnabled(this IUposMediator mediator)
    {
        if (mediator.CurrentState < ControlState.Enabled)
        {
            throw new UposStateException("Device must be enabled to perform this operation.", UposErrorCode.Disabled);
        }
    }

    /// <summary>Validates that the device is not currently busy performing another operation.</summary>
    /// <param name="mediator">The mediator.</param>
    /// <exception cref="UposStateException">The device is busy.</exception>
    public static void ValidateNotBusy(this IUposMediator mediator)
    {
        if (mediator.IsBusyValue)
        {
            throw new UposStateException("Device is busy.", UposErrorCode.Busy);
        }
    }
}
