using PosSharp.Abstractions;

namespace PosSharp.Core.Lifecycle;

/// <summary>Manages the lifecycle of a UPOS device by coordinating state transitions.</summary>
/// <param name="mediator">The mediator to update.</param>
/// <param name="handler">The handler for validation rules.</param>
public sealed class UposLifecycleManager(IUposMediator mediator, IUposLifecycleHandler handler)
{
    /// <summary>Gets or sets a value indicating whether state verification is enabled.</summary>
    public bool IsStateVerificationEnabled { get; set; } = true;

    /// <summary>Transitions the device to the specified state.</summary>
    /// <param name="targetState">The target state.</param>
    public void TransitionTo(ControlState targetState)
    {
        if (IsStateVerificationEnabled)
        {
            handler.ValidateTransition(mediator.CurrentState, targetState);
        }

        mediator.UpdateState(targetState);
    }

    /// <summary>Validates requirements before opening the device.</summary>
    public void PreOpen()
    {
        if (IsStateVerificationEnabled)
        {
            handler.ValidateTransition(mediator.CurrentState, ControlState.Idle);
        }
    }

    /// <summary>Updates state after opening the device.</summary>
    public void PostOpen() => mediator.UpdateState(ControlState.Idle);

    /// <summary>Validates requirements before closing the device.</summary>
    public void PreClose()
    {
        if (IsStateVerificationEnabled)
        {
            handler.ValidateTransition(mediator.CurrentState, ControlState.Closed);
        }
    }

    /// <summary>Updates state after closing the device.</summary>
    public void PostClose() => Reset();

    /// <summary>Validates requirements before claiming the device.</summary>
    public void PreClaim()
    {
        if (IsStateVerificationEnabled)
        {
            handler.ValidateTransition(mediator.CurrentState, ControlState.Claimed);
        }
    }

    /// <summary>Updates state after claiming the device.</summary>
    public void PostClaim() => mediator.UpdateState(ControlState.Claimed);

    /// <summary>Validates requirements before releasing the device.</summary>
    public void PreRelease()
    {
        if (IsStateVerificationEnabled)
        {
            handler.ValidateTransition(mediator.CurrentState, ControlState.Idle);
        }
    }

    /// <summary>Updates state after releasing the device.</summary>
    public void PostRelease() => mediator.UpdateState(ControlState.Idle);

    /// <summary>Validates requirements before enabling the device.</summary>
    public void PreEnable()
    {
        if (IsStateVerificationEnabled)
        {
            handler.ValidateTransition(mediator.CurrentState, ControlState.Enabled);
        }
    }

    /// <summary>Updates state after enabling the device.</summary>
    public void PostEnable() => mediator.UpdateState(ControlState.Enabled);

    /// <summary>Validates requirements before disabling the device.</summary>
    public void PreDisable()
    {
        if (IsStateVerificationEnabled)
        {
            handler.ValidateTransition(mediator.CurrentState, ControlState.Claimed);
        }
    }

    /// <summary>Updates state after disabling the device.</summary>
    public void PostDisable() => mediator.UpdateState(ControlState.Claimed);

    /// <summary>Verifies that the device is in the required state.</summary>
    /// <param name="requiredState">The required state.</param>
    public void VerifyState(ControlState requiredState)
    {
        if (IsStateVerificationEnabled)
        {
            handler.VerifyState(mediator.CurrentState, requiredState);
        }
    }

    /// <summary>Verifies that the device is in one of the allowed states.</summary>
    /// <param name="allowedStates">The allowed states.</param>
    /// <exception cref="UposStateException">Thrown when verification fails.</exception>
    public void VerifyState(params ControlState[] allowedStates)
    {
        if (!IsStateVerificationEnabled)
        {
            return;
        }

        if (allowedStates.Length == 1)
        {
            handler.VerifyState(mediator.CurrentState, allowedStates[0]);
            return;
        }

        if (!allowedStates.Contains(mediator.CurrentState))
        {
            throw new UposStateException(mediator.CurrentState, allowedStates);
        }
    }

    /// <summary>Resets the device to the closed state.</summary>
    public void Reset()
    {
        mediator.UpdateState(ControlState.Closed);
        mediator.SetBusy(false);
        mediator.ReportError(UposErrorCode.Success);
    }
}
