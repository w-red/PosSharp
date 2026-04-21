using PosSharp.Abstractions;
using Shouldly;

namespace PosSharp.Core.Tests;

public sealed class CancellationTests
{
    [Fact]
    public async Task OpenAsyncWhenCancelledThrowsOperationCanceledException()
    {
        // Arrange
        using var device = new CancellableStubUposDevice();
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act & Assert
        await Should.ThrowAsync<OperationCanceledException>(() => device.OpenAsync(cts.Token));
        device.State.CurrentValue.ShouldBe(ControlState.Closed);
    }

    [Fact]
    public async Task ClaimAsyncWhenCancelledThrowsOperationCanceledException()
    {
        // Arrange
        using var device = new CancellableStubUposDevice();
        await device.OpenAsync();
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act & Assert
        await Should.ThrowAsync<OperationCanceledException>(() => device.ClaimAsync(1000, cts.Token));
        device.State.CurrentValue.ShouldBe(ControlState.Idle);
    }
}
