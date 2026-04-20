// Copyright (c) PosSharp Project. All rights reserved.
// Licensed under the MIT License.

using PosSharp.Abstractions;
using Shouldly;

namespace PosSharp.Core.Tests;

public sealed class BusyStateTests
{
    [Fact]
    public async Task BeginOperation_SetsIsBusyTrue()
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
    public async Task BeginOperation_WhenNotEnabled_ThrowsUposStateException()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(1000);

        // Act & Assert
        await Should.ThrowAsync<UposStateException>(() => Task.FromResult(device.TestBeginOperation()));
    }

    [Fact]
    public async Task BeginOperation_WhenAlreadyBusy_ThrowsInvalidOperationException()
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
