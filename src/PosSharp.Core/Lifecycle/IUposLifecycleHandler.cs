using PosSharp.Abstractions;

namespace PosSharp.Core.Lifecycle;

/// <summary>Defines rules for UPOS device state transitions.</summary>
public interface IUposLifecycleHandler
{
    /// <summary>Validates whether a transition to a specific state is allowed from the current state.</summary>
    /// <param name="currentState">The current logical state.</param>
    /// <param name="targetState">The target logical state.</param>
    /// <exception cref="UposStateException">Thrown if the transition is invalid.</exception>
    void ValidateTransition(ControlState currentState, ControlState targetState);

    /// <summary>Validates whether an operation is allowed in the current state.</summary>
    /// <param name="currentState">The current logical state.</param>
    /// <param name="requiredState">The minimum state required for the operation.</param>
    /// <exception cref="UposStateException">Thrown if the current state does not meet requirements.</exception>
    void VerifyState(ControlState currentState, ControlState requiredState);
}
