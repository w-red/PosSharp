using Xunit;
using PosSharp.Abstractions;
using PosSharp.Core.Lifecycle;
using Shouldly;

namespace PosSharp.Core.Tests;

/// <summary>Tests for the <see cref="StandardLifecycleHandler"/> class.</summary>
public sealed class LifecycleHandlerTests
{
    private readonly StandardLifecycleHandler handler = new();

    /// <summary>Verifies that VerifyState does not throw when the current state is equal to the required state.</summary>
    /// <param name="current">The current state.</param>
    /// <param name="required">The required state.</param>
    [Theory]
    [InlineData(ControlState.Closed, ControlState.Closed)]
    [InlineData(ControlState.Idle, ControlState.Idle)]
    [InlineData(ControlState.Claimed, ControlState.Claimed)]
    [InlineData(ControlState.Enabled, ControlState.Enabled)]
    public void VerifyState_WhenCurrentEqualsRequired_ShouldNotThrow(ControlState current, ControlState required)
    {
        // Act & Assert
        Should.NotThrow(() => handler.VerifyState(current, required));
    }

    /// <summary>Verifies that VerifyState throws UposStateException when the current state is logically less than the required state.</summary>
    /// <param name="current">The current state.</param>
    /// <param name="required">The required state.</param>
    [Theory]
    [InlineData(ControlState.Closed, ControlState.Idle)]
    [InlineData(ControlState.Idle, ControlState.Claimed)]
    [InlineData(ControlState.Claimed, ControlState.Enabled)]
    public void VerifyState_WhenCurrentIsLessThanRequired_ShouldThrow(ControlState current, ControlState required)
    {
        // Act & Assert
        Should.Throw<UposStateException>(() => handler.VerifyState(current, required));
    }

    /// <summary>Verifies that VerifyState does not throw when the current state is logically greater than the required state.</summary>
    /// <param name="current">The current state.</param>
    /// <param name="required">The required state.</param>
    [Theory]
    [InlineData(ControlState.Idle, ControlState.Closed)]
    [InlineData(ControlState.Claimed, ControlState.Idle)]
    [InlineData(ControlState.Enabled, ControlState.Claimed)]
    public void VerifyState_WhenCurrentIsGreaterThanRequired_ShouldNotThrow(ControlState current, ControlState required)
    {
        // Act & Assert
        Should.NotThrow(() => handler.VerifyState(current, required));
    }

    /// <summary>Verifies all state transition rules defined in the handler.</summary>
    /// <param name="from">The starting state.</param>
    /// <param name="to">The target state.</param>
    /// <param name="allowed">Whether the transition should be allowed.</param>
    [Theory]
    // Closed transitions
    [InlineData(ControlState.Closed, ControlState.Idle, true)] // Open
    [InlineData(ControlState.Closed, ControlState.Closed, true)]
    [InlineData(ControlState.Closed, ControlState.Claimed, false)]
    // Idle transitions
    [InlineData(ControlState.Idle, ControlState.Closed, true)] // Close
    [InlineData(ControlState.Idle, ControlState.Claimed, true)] // Claim
    [InlineData(ControlState.Idle, ControlState.Enabled, false)]
    // Claimed transitions
    [InlineData(ControlState.Claimed, ControlState.Idle, true)] // Release
    [InlineData(ControlState.Claimed, ControlState.Enabled, true)] // Enable
    [InlineData(ControlState.Claimed, ControlState.Closed, true)] // Close (Always allowed)
    // Enabled transitions
    [InlineData(ControlState.Enabled, ControlState.Claimed, true)] // Disable
    [InlineData(ControlState.Enabled, ControlState.Enabled, false)] // No self-transition in handler
    [InlineData(ControlState.Enabled, ControlState.Closed, true)] // Close (Always allowed)
    [InlineData(ControlState.Enabled, ControlState.Idle, true)] // Indirect Release (Allowed)
    public void ValidateTransition_TestsAllRules(ControlState from, ControlState to, bool allowed)
    {
        // Act & Assert
        if (allowed)
        {
            Should.NotThrow(() => handler.ValidateTransition(from, to));
        }
        else
        {
            Should.Throw<UposStateException>(() => handler.ValidateTransition(from, to));
        }
    }
}
