using PosSharp.Abstractions;
using Shouldly;

namespace PosSharp.Core.Tests;

public sealed class BusyStateTests
{
    [Fact]
    public async Task BeginOperationSetsIsBusyTrue()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(1000);
        await device.SetEnabledAsync(true);

        // Act
        using (device.TestBeginOperation())
        {
            // Assert
            device.IsBusy.CurrentValue.ShouldBeTrue();
        }

        // Post-Assert
        device.IsBusy.CurrentValue.ShouldBeFalse();
    }

    [Fact]
    public async Task BeginOperationWhenNotEnabledThrowsUposStateException()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(1000);

        // Act & Assert
        await Should.ThrowAsync<UposStateException>(() => Task.FromResult(device.TestBeginOperation()));
    }

    [Fact]
    public async Task BeginOperationWhenAlreadyBusyThrowsInvalidOperationException()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(1000);
        await device.SetEnabledAsync(true);

        using var guard = device.TestBeginOperation();

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => device.TestBeginOperation());
    }
}
