using PosSharp.Abstractions;
using PosSharp.Core.Lifecycle;
using Shouldly;

namespace PosSharp.Core.Tests;

public sealed class LifecycleHandlerTests
{
    private readonly StandardLifecycleHandler handler = new();

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

    [Theory]
    [InlineData(ControlState.Closed, ControlState.Idle)]
    [InlineData(ControlState.Idle, ControlState.Claimed)]
    [InlineData(ControlState.Claimed, ControlState.Enabled)]
    public void VerifyState_WhenCurrentIsLessThanRequired_ShouldThrow(ControlState current, ControlState required)
    {
        // Act & Assert
        Should.Throw<UposStateException>(() => handler.VerifyState(current, required));
    }

    [Theory]
    [InlineData(ControlState.Idle, ControlState.Closed)]
    [InlineData(ControlState.Claimed, ControlState.Idle)]
    [InlineData(ControlState.Enabled, ControlState.Claimed)]
    public void VerifyState_WhenCurrentIsGreaterThanRequired_ShouldNotThrow(ControlState current, ControlState required)
    {
        // Act & Assert
        Should.NotThrow(() => handler.VerifyState(current, required));
    }

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
