// Copyright (c) PosSharp Project. All rights reserved.
// Licensed under the MIT License.

using PosSharp.Abstractions;
using Shouldly;

namespace PosSharp.Core.Tests;

public sealed class CancellationTests
{
    [Fact]
    public async Task OpenAsync_WhenCancelled_ThrowsOperationCanceledException()
    {
        // Arrange
        using var device = new CancellableStubUposDevice();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Should.ThrowAsync<OperationCanceledException>(() => device.OpenAsync(cts.Token));
        device.State.CurrentValue.ShouldBe(ControlState.Closed);
    }

    [Fact]
    public async Task ClaimAsync_WhenCancelled_ThrowsOperationCanceledException()
    {
        // Arrange
        using var device = new CancellableStubUposDevice();
        await device.OpenAsync();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Should.ThrowAsync<OperationCanceledException>(() => device.ClaimAsync(1000, cts.Token));
        device.State.CurrentValue.ShouldBe(ControlState.Idle);
    }
}
