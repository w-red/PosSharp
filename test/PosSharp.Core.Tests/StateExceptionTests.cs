using Xunit;
using PosSharp.Abstractions;
using PosSharp.Core.Lifecycle;
using Shouldly;

namespace PosSharp.Core.Tests;

/// <summary>Tests for various state-related exception scenarios in <see cref="UposDeviceBase"/>.</summary>
public sealed class StateExceptionTests
{
    /// <summary>Verifies that OpenAsync throws UposStateException when the device is already open.</summary>
    [Fact]
    public async Task OpenAsyncWhenAlreadyOpenThrowsUposStateException()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);

        // Act & Assert
        var ex = await Should.ThrowAsync<UposStateException>(() => device.OpenAsync(TestContext.Current.CancellationToken));
        ex.CurrentState.ShouldBe(ControlState.Idle);
        ex.AllowedStates.ShouldContain(ControlState.Closed);
    }

    /// <summary>Verifies that ClaimAsync throws UposStateException when the device is not open.</summary>
    [Fact]
    public async Task ClaimAsyncWhenClosedThrowsUposStateException()
    {
        // Arrange
        using var device = new StubUposDevice();

        // Act & Assert
        var ex = await Should.ThrowAsync<UposStateException>(() => device.ClaimAsync(1000, TestContext.Current.CancellationToken));
        ex.CurrentState.ShouldBe(ControlState.Closed);
        ex.AllowedStates.ShouldContain(ControlState.Idle);
    }

    /// <summary>Verifies that enabling the device throws UposStateException when the device is not claimed.</summary>
    [Fact]
    public async Task SetEnabledAsyncWhenNotClaimedThrowsUposStateException()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);

        // Act & Assert
        var ex = await Should.ThrowAsync<UposStateException>(() => device.SetEnabledAsync(true, TestContext.Current.CancellationToken));
        ex.CurrentState.ShouldBe(ControlState.Idle);
        ex.AllowedStates.ShouldContain(ControlState.Claimed);
    }

    /// <summary>Verifies that ReleaseAsync throws UposStateException when the device is not claimed.</summary>
    [Fact]
    public async Task ReleaseAsyncWhenNotClaimedThrowsUposStateException()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);

        // Act & Assert
        var ex = await Should.ThrowAsync<UposStateException>(() => device.ReleaseAsync(TestContext.Current.CancellationToken));
        ex.CurrentState.ShouldBe(ControlState.Idle);
        ex.AllowedStates.ShouldContain(ControlState.Claimed);
        ex.AllowedStates.ShouldContain(ControlState.Enabled);
    }

    /// <summary>Verifies that SetEnabledAsync throws UposStateException when the device is not claimed (redundant check for clarity).</summary>
    [Fact]
    public async Task EnableAsyncWhenNotClaimedThrowsUposStateException()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);

        // Act & Assert
        var ex = await Should.ThrowAsync<UposStateException>(() => device.SetEnabledAsync(true, TestContext.Current.CancellationToken));
        ex.CurrentState.ShouldBe(ControlState.Idle);
        ex.AllowedStates.ShouldBe([ControlState.Claimed]);
    }

    /// <summary>Verifies that StandardLifecycleHandler.ValidateTransition throws UposStateException for invalid transitions.</summary>
    /// <param name="from">The starting state.</param>
    /// <param name="to">The target state.</param>
    [Theory]
    [InlineData(ControlState.Closed, ControlState.Enabled)]
    [InlineData(ControlState.Idle, ControlState.Enabled)]
    public void StandardLifecycleHandler_ValidateTransition_ThrowsForInvalid(ControlState from, ControlState to)
    {
        var handler = new StandardLifecycleHandler();
        var ex = Should.Throw<UposStateException>(() => handler.ValidateTransition(from, to));
        ex.CurrentState.ShouldBe(from);
    }

    /// <summary>Verifies that StandardLifecycleHandler.VerifyState throws UposStateException when the required state level is higher than the current state.</summary>
    [Fact]
    public void StandardLifecycleHandler_VerifyState_ThrowsWhenStateTooLow()
    {
        var handler = new StandardLifecycleHandler();
        var ex = Should.Throw<UposStateException>(() => handler.VerifyState(ControlState.Idle, ControlState.Enabled));
        ex.Message.ShouldContain("Operation requires Enabled state, but current state is Idle.");
    }
}
