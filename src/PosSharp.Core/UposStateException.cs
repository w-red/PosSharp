using PosSharp.Abstractions;

namespace PosSharp.Core;

/// <summary>Exception thrown when a UPOS device is in an invalid state for the requested operation.</summary>
public sealed class UposStateException : InvalidOperationException
{
    /// <summary>Initializes a new instance of the <see cref="UposStateException"/> class.</summary>
    /// <param name="message">The exception message.</param>
    public UposStateException(string message)
        : base(message)
    {
        CurrentState = ControlState.Error;
        AllowedStates = [];
    }

    /// <summary>Initializes a new instance of the <see cref="UposStateException"/> class.</summary>
    /// <param name="currentState">The current state of the device.</param>
    /// <param name="allowedStates">The states that would have been valid for the operation.</param>
    public UposStateException(ControlState currentState, IReadOnlyList<ControlState> allowedStates)
        : base($"Invalid state transition from {currentState}. Allowed: {string.Join(", ", allowedStates)}")
    {
        CurrentState = currentState;
        AllowedStates = allowedStates;
    }

    /// <summary>Gets the state of the device at the time the exception was thrown.</summary>
    public ControlState CurrentState { get; }

    /// <summary>Gets the list of states that were allowed for the operation.</summary>
    public IReadOnlyList<ControlState> AllowedStates { get; }
}
