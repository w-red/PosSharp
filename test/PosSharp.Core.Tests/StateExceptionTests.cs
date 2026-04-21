using PosSharp.Abstractions;
using PosSharp.Core.Lifecycle;
using Shouldly;

namespace PosSharp.Core.Tests;

public sealed class StateExceptionTests
{
    [Fact]
    public async Task OpenAsyncWhenAlreadyOpenThrowsUposStateException()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();

        // Act & Assert
        var ex = await Should.ThrowAsync<UposStateException>(() => device.OpenAsync());
        ex.CurrentState.ShouldBe(ControlState.Idle);
        ex.AllowedStates.ShouldContain(ControlState.Closed);
    }

    [Fact]
    public async Task ClaimAsyncWhenClosedThrowsUposStateException()
    {
        // Arrange
        using var device = new StubUposDevice();

        // Act & Assert
        var ex = await Should.ThrowAsync<UposStateException>(() => device.ClaimAsync(1000));
        ex.CurrentState.ShouldBe(ControlState.Closed);
        ex.AllowedStates.ShouldContain(ControlState.Idle);
    }

    [Fact]
    public async Task SetEnabledAsyncWhenNotClaimedThrowsUposStateException()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();

        // Act & Assert
        var ex = await Should.ThrowAsync<UposStateException>(() => device.SetEnabledAsync(true));
        ex.CurrentState.ShouldBe(ControlState.Idle);
        ex.AllowedStates.ShouldContain(ControlState.Claimed);
    }

    [Fact]
    public async Task ReleaseAsyncWhenNotClaimedThrowsUposStateException()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();

        // Act & Assert
        var ex = await Should.ThrowAsync<UposStateException>(() => device.ReleaseAsync());
        ex.CurrentState.ShouldBe(ControlState.Idle);
        ex.AllowedStates.ShouldContain(ControlState.Claimed);
        ex.AllowedStates.ShouldContain(ControlState.Enabled);
    }

    [Fact]
    public async Task EnableAsyncWhenNotClaimedThrowsUposStateException()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();

        // Act & Assert
        var ex = await Should.ThrowAsync<UposStateException>(() => device.SetEnabledAsync(true));
        ex.CurrentState.ShouldBe(ControlState.Idle);
        ex.AllowedStates.ShouldBe(new[] { ControlState.Claimed });
    }

    [Theory]
    [InlineData(ControlState.Closed, ControlState.Enabled)]
    [InlineData(ControlState.Idle, ControlState.Enabled)]
    public void StandardLifecycleHandler_ValidateTransition_ThrowsForInvalid(ControlState from, ControlState to)
    {
        var handler = new StandardLifecycleHandler();
        var ex = Should.Throw<UposStateException>(() => handler.ValidateTransition(from, to));
        ex.CurrentState.ShouldBe(from);
    }

    [Fact]
    public void StandardLifecycleHandler_VerifyState_ThrowsWhenStateTooLow()
    {
        var handler = new StandardLifecycleHandler();
        var ex = Should.Throw<UposStateException>(() => handler.VerifyState(ControlState.Idle, ControlState.Enabled));
        ex.Message.ShouldContain("Operation requires Enabled state, but current state is Idle.");
    }
}
