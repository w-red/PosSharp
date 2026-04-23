using PosSharp.Abstractions;

namespace PosSharp.Core;

/// <summary>Provides extension methods for <see cref="IUposMediator"/> to simplify state validation.</summary>
public static class UposMediatorExtensions
{
    /// <summary>Validates that the device is not in the <see cref="ControlState.Closed"/> state.</summary>
    /// <param name="mediator">The mediator to validate.</param>
    /// <exception cref="UposStateException">Thrown if the device is closed.</exception>
    public static void ValidateOpen(this IUposMediator mediator)
    {
        if (mediator.CurrentState == ControlState.Closed)
        {
            throw new UposStateException("Device must be open to perform this operation.");
        }
    }

    /// <summary>Validates that the device is at least in the <see cref="ControlState.Claimed"/> state.</summary>
    /// <param name="mediator">The mediator to validate.</param>
    /// <exception cref="UposStateException">Thrown if the device is not claimed.</exception>
    public static void ValidateClaimed(this IUposMediator mediator)
    {
        ValidateOpen(mediator);
        if (mediator.CurrentState < ControlState.Claimed)
        {
            throw new UposStateException("Device must be claimed to perform this operation.");
        }
    }

    /// <summary>Validates that the device is at least in the <see cref="ControlState.Enabled"/> state.</summary>
    /// <param name="mediator">The mediator to validate.</param>
    /// <exception cref="UposStateException">Thrown if the device is not enabled.</exception>
    public static void ValidateEnabled(this IUposMediator mediator)
    {
        ValidateClaimed(mediator);
        if (mediator.CurrentState < ControlState.Enabled)
        {
            throw new UposStateException("Device must be enabled to perform this operation.");
        }
    }

    /// <summary>Validates that the device is not currently busy.</summary>
    /// <param name="mediator">The mediator to validate.</param>
    /// <exception cref="UposStateException">Thrown if the device is busy.</exception>
    public static void ValidateNotBusy(this IUposMediator mediator)
    {
        if (mediator.IsBusyValue)
        {
            throw new UposStateException("Device is currently busy.");
        }
    }
}
