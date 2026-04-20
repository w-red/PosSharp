// Copyright (c) PosSharp Project. All rights reserved.
// Licensed under the MIT License.

using PosSharp.Abstractions;
using PosSharp.Core;
using Shouldly;

namespace PosSharp.Core.Tests;

/// <summary>
/// Tests for <see cref="UposDeviceBase"/> lifecycle state transitions.
/// </summary>
public sealed class LifecycleTests
{
    [Fact]
    public async Task OpenAsync_WhenClosed_TransitionsToIdle()
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
    public async Task ClaimAsync_WhenIdle_TransitionsToClaimed()
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
    public async Task SetEnabledAsync_True_WhenClaimed_TransitionsToEnabled()
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
    public async Task SetEnabledAsync_False_WhenEnabled_TransitionsToClaimed()
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
    public async Task ReleaseAsync_WhenClaimed_TransitionsToIdle()
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
    public async Task CloseAsync_WhenIdle_TransitionsToClosed()
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
