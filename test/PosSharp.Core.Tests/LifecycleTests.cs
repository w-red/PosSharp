using PosSharp.Abstractions;
using Shouldly;

namespace PosSharp.Core.Tests;

/// <summary>Tests for <see cref="UposDeviceBase"/> lifecycle state transitions.</summary>
public sealed class LifecycleTests
{
    /// <summary>Verifies that OpenAsync transitions the device from Closed to Idle state.</summary>
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

    /// <summary>Verifies that ClaimAsync transitions the device from Idle to Claimed state.</summary>
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

    /// <summary>Verifies that enabling the device transitions it from Claimed to Enabled state.</summary>
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

    /// <summary>Verifies that disabling the device transitions it from Enabled back to Claimed state.</summary>
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

    /// <summary>Verifies that ReleaseAsync transitions the device from Claimed back to Idle state.</summary>
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

    /// <summary>Verifies that CloseAsync transitions the device from Idle back to Closed state.</summary>
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
