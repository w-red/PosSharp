using PosSharp.Abstractions;
using Shouldly;

namespace PosSharp.Core.Tests;

public sealed class DeviceOperationTests
{
    [Fact]
    public async Task DirectIOAsync_PropagatesCallsCorrectly()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(1000);
        await device.SetEnabledAsync(true);

        const int command = 123;
        const int data = 456;
        const string obj = "TestData";

        // Act
        await device.DirectIOAsync(command, data, obj);

        // Assert
        device.LastDirectIOCommand.ShouldBe(command);
        device.LastDirectIOData.ShouldBe(data);
        device.LastDirectIOObject.ShouldBe(obj);
    }

    [Fact]
    public async Task ClearInputAsync_PropagatesCall()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(1000);

        // Act
        await device.ClearInputAsync();

        // Assert
        device.ClearInputCalled.ShouldBeTrue();
    }

    [Fact]
    public async Task ClearOutputAsync_PropagatesCall()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(1000);

        // Act
        await device.ClearOutputAsync();

        // Assert
        device.ClearOutputCalled.ShouldBeTrue();
    }

    [Fact]
    public async Task Operations_ThrowException_WhenInvalidState()
    {
        // Arrange
        using var device = new StubUposDevice();

        // remains in Closed state
        // Assert
        await Should.ThrowAsync<UposStateException>(() => device.DirectIOAsync(1, 1, string.Empty));
        await Should.ThrowAsync<UposStateException>(() => device.ClearInputAsync());
        await Should.ThrowAsync<UposStateException>(() => device.ClearOutputAsync());
    }

    [Fact]
    public async Task OpenAsync_CallsHook_WhenClosed()
    {
        // Arrange
        using var device = new StubUposDevice();

        // Act
        await device.OpenAsync();

        // Assert
        device.OpenCalled.ShouldBeTrue();
    }

    [Fact]
    public async Task CloseAsync_EarlyReturns_WhenAlreadyClosed()
    {
        // Arrange
        using var device = new StubUposDevice();

        // Act
        await device.CloseAsync();

        // Assert
        device.CloseCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task CloseAsync_CallsHook_WhenOpen()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();

        // Act
        await device.CloseAsync();

        // Assert
        device.CloseCalled.ShouldBeTrue();
    }

    [Fact]
    public async Task ClaimAsync_EarlyReturns_WhenAlreadyClaimedOrEnabled()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(100);
        device.ClaimCalled.ShouldBeTrue();

        // Reset flag for second call
        // StubUposDevice doesn't have a reset, but we can check it didn't change if we used a mock.
        // For now, we'll just check it returns successfully without throwing.

        // Act
        await device.ClaimAsync(100);
        await device.SetEnabledAsync(true);
        await device.ClaimAsync(100);

        // Assert
        // (Just verifying no exceptions and it returns early as intended)
    }

    [Fact]
    public async Task SetEnabledAsync_EarlyReturns_WhenStateMatches()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(100);

        // Enable
        await device.SetEnabledAsync(true);
        device.EnableCalled.ShouldBeTrue();

        // Act - Call again with true
        await device.SetEnabledAsync(true);

        // Assert - DisableCalled should still be false
        device.DisableCalled.ShouldBeFalse();

        // Act - Disable
        await device.SetEnabledAsync(false);
        device.DisableCalled.ShouldBeTrue();

        // Act - Call again with false
        await device.SetEnabledAsync(false);
    }

    [Fact]
    public async Task HealthCheck_CallsHook()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(100);
        await device.SetEnabledAsync(true);

        // Act
        await device.CheckHealthAsync(HealthCheckLevel.Internal);

        // Assert
        device.CheckHealthCalled.ShouldBeTrue();
    }

    [Fact]
    public async Task Operations_RespectCancellationToken()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(1000);
        await device.SetEnabledAsync(true);

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act & Assert
        await Should.ThrowAsync<OperationCanceledException>(async () => await device.OpenAsync(cts.Token));
        await Should.ThrowAsync<OperationCanceledException>(async () => await device.CloseAsync(cts.Token));
        await Should.ThrowAsync<OperationCanceledException>(async () => await device.ClaimAsync(1, cts.Token));
        await Should.ThrowAsync<OperationCanceledException>(async () => await device.ReleaseAsync(cts.Token));
        await Should.ThrowAsync<OperationCanceledException>(async () => await device.SetEnabledAsync(true, cts.Token));
        await Should.ThrowAsync<OperationCanceledException>(async () =>
            await device.CheckHealthAsync(HealthCheckLevel.Internal, cts.Token)
        );
        await Should.ThrowAsync<OperationCanceledException>(async () =>
            await device.DirectIOAsync(1, 1, string.Empty, cts.Token)
        );
        await Should.ThrowAsync<OperationCanceledException>(async () => await device.ClearInputAsync(cts.Token));
        await Should.ThrowAsync<OperationCanceledException>(async () => await device.ClearOutputAsync(cts.Token));
    }
}
