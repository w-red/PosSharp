using PosSharp.Abstractions;
using PosSharp.Core;
using Shouldly;

namespace PosSharp.Core.Tests;

/// <summary>Tests for <see cref="UposDeviceBase"/> lifecycle state transitions.</summary>
public sealed class LifecycleTests
{
    [Fact]
    public async Task OpenAsyncWhenClosedTransitionsToIdle()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.State.CurrentValue.ShouldBe(ControlState.Closed);

        // Act
        await device.OpenAsync();

        // Assert
        device.State.CurrentValue.ShouldBe(ControlState.Idle);
    }

    [Fact]
    public async Task ClaimAsyncWhenIdleTransitionsToClaimed()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();

        // Act
        await device.ClaimAsync(timeout: 1000);

        // Assert
        device.State.CurrentValue.ShouldBe(ControlState.Claimed);
    }

    [Fact]
    public async Task SetEnabledAsyncTrueWhenClaimedTransitionsToEnabled()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(timeout: 1000);

        // Act
        await device.SetEnabledAsync(true);

        // Assert
        device.State.CurrentValue.ShouldBe(ControlState.Enabled);
    }

    [Fact]
    public async Task SetEnabledAsyncFalseWhenEnabledTransitionsToClaimed()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(timeout: 1000);
        await device.SetEnabledAsync(true);

        // Act
        await device.SetEnabledAsync(false);

        // Assert
        device.State.CurrentValue.ShouldBe(ControlState.Claimed);
    }

    [Fact]
    public async Task ReleaseAsyncWhenClaimedTransitionsToIdle()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(timeout: 1000);

        // Act
        await device.ReleaseAsync();

        // Assert
        device.State.CurrentValue.ShouldBe(ControlState.Idle);
    }

    [Fact]
    public async Task CloseAsyncWhenIdleTransitionsToClosed()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();

        // Act
        await device.CloseAsync();

        // Assert
        device.State.CurrentValue.ShouldBe(ControlState.Closed);
    }
}
