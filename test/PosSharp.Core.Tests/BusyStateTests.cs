using Xunit;
using Shouldly;

namespace PosSharp.Core.Tests;

/// <summary>Tests for the busy state management in <see cref="UposDeviceBase"/>.</summary>
public sealed class BusyStateTests
{
    /// <summary>Verifies that BeginOperation correctly sets the busy status and resets it upon disposal.</summary>
    [Fact]
    public async Task BeginOperationSetsIsBusyTrue()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);
        await device.ClaimAsync(1000, TestContext.Current.CancellationToken);
        await device.SetEnabledAsync(true, TestContext.Current.CancellationToken);

        // Act
        using (device.TestBeginOperation())
        {
            // Assert
            device.IsBusy.CurrentValue.ShouldBeTrue();
        }

        // Post-Assert
        device.IsBusy.CurrentValue.ShouldBeFalse();
    }

    /// <summary>Verifies that BeginOperation throws UposStateException when the device is not enabled.</summary>
    [Fact]
    public async Task BeginOperationWhenNotEnabledThrowsUposStateException()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);
        await device.ClaimAsync(1000, TestContext.Current.CancellationToken);

        // Act & Assert
        await Should.ThrowAsync<UposStateException>(() => Task.FromResult(device.TestBeginOperation()));
    }

    /// <summary>Verifies that BeginOperation throws UposStateException when the device is already busy.</summary>
    [Fact]
    public async Task BeginOperationWhenAlreadyBusyThrowsUposStateException()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);
        await device.ClaimAsync(1000, TestContext.Current.CancellationToken);
        await device.SetEnabledAsync(true, TestContext.Current.CancellationToken);

        using var guard = device.TestBeginOperation();

        // Act & Assert
        var ex = Should.Throw<UposStateException>(() => device.TestBeginOperation());
        ex.Message.ShouldBe("Device is already busy.");
    }
}


