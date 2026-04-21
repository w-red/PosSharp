using PosSharp.Abstractions;

namespace PosSharp.Core.Lifecycle;

/// <summary>Implements standard UPOS lifecycle rules.</summary>
public sealed class StandardLifecycleHandler : IUposLifecycleHandler
{
    /// <inheritdoc />
    public void ValidateTransition(ControlState currentState, ControlState targetState)
    {
        switch (targetState)
        {
            case ControlState.Idle:
                // Allowed from Closed (Open) or Claimed/Enabled (Release).
                if (currentState is not ControlState.Closed and not ControlState.Claimed and not ControlState.Enabled)
                {
                    throw new UposStateException(
                        currentState,
                        [ControlState.Closed, ControlState.Claimed, ControlState.Enabled]
                    );
                }

                break;

            case ControlState.Claimed:
                // Allowed from Idle (Claim) or Enabled (Disable).
                if (currentState is not ControlState.Idle and not ControlState.Enabled)
                {
                    throw new UposStateException(currentState, [ControlState.Idle, ControlState.Enabled]);
                }

                break;

            case ControlState.Enabled:
                // Allowed from Claimed (Enable).
                if (currentState != ControlState.Claimed)
                {
                    throw new UposStateException(currentState, [ControlState.Claimed]);
                }

                break;

            case ControlState.Closed:
                // Always allowed to close.
                break;
        }
    }

    /// <inheritdoc />
    public void VerifyState(ControlState currentState, ControlState requiredState)
    {
        if (currentState < requiredState)
        {
            throw new UposStateException(
                $"Operation requires {requiredState} state, but current state is {currentState}."
            );
        }
    }
}
