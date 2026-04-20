// Copyright (c) PosSharp Project. All rights reserved.
// Licensed under the MIT License.

using PosSharp.Abstractions;
using Shouldly;

namespace PosSharp.Core.Tests;

public sealed class StateExceptionTests
{
    [Fact]
    public async Task OpenAsync_WhenAlreadyOpen_ThrowsUposStateException()
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
    public async Task ClaimAsync_WhenClosed_ThrowsUposStateException()
    {
        // Arrange
        using var device = new StubUposDevice();

        // Act & Assert
        var ex = await Should.ThrowAsync<UposStateException>(() => device.ClaimAsync(1000));
        ex.CurrentState.ShouldBe(ControlState.Closed);
        ex.AllowedStates.ShouldContain(ControlState.Idle);
    }

    [Fact]
    public async Task SetEnabledAsync_WhenNotClaimed_ThrowsUposStateException()
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
    public async Task ReleaseAsync_WhenNotClaimed_ThrowsUposStateException()
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
}
