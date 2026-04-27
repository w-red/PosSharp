using Xunit;
using PosSharp.Abstractions;
using R3;
using Shouldly;

namespace PosSharp.Core.Tests;

/// <summary>Tests for state verification toggle and property synchronization in <see cref="UposDeviceBase"/>.</summary>
public sealed class VerificationTests
{
    /// <summary>Verifies that state verification can be disabled to allow normally illegal transitions.</summary>
    [Fact]
    public async Task StateVerificationCanBeDisabled()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.State.CurrentValue.ShouldBe(ControlState.Closed);

        // Act & Assert
        // Normally, ClaimAsync requires state to be Idle (after Open).
        // Calling it while Closed should fail if verification is enabled.
        await Should.ThrowAsync<UposStateException>(async () =>
        {
            await device.ClaimAsync(timeout: 0, TestContext.Current.CancellationToken);
        });

        // Now disable verification
        device.IsStateVerificationEnabled = false;

        // Should NOT throw now
        await device.ClaimAsync(timeout: 0, TestContext.Current.CancellationToken);

        // State is updated by PostClaim anyway
        device.State.CurrentValue.ShouldBe(ControlState.Claimed);
    }

    /// <summary>Verifies that the DataEventEnabled property correctly synchronizes with its reactive counterpart.</summary>
    [Fact]
    public void DataEventEnabledPropertySyncs()
    {
        // Arrange
        using var device = new StubUposDevice();
        bool reactiveValue = false;
        device.DataEventEnabledProperty.Subscribe(v => reactiveValue = v);

        // Act
        device.DataEventEnabled = true;

        // Assert
        device.DataEventEnabled.ShouldBeTrue();
        reactiveValue.ShouldBeTrue();

        // Act
        device.DataEventEnabled = false;

        // Assert
        device.DataEventEnabled.ShouldBeFalse();
        reactiveValue.ShouldBeFalse();
    }

    /// <summary>Verifies that synchronization properties return the correct initial values.</summary>
    [Fact]
    public void SyncPropertiesReturnCorrectValues()
    {
        // Arrange
        using var device = new StubUposDevice();

        // Initial (Closed)
        device.IsOpen.ShouldBeFalse();
        device.IsClaimed.ShouldBeFalse();
        device.IsEnabled.ShouldBeFalse();

        // After Move/Transition (Simulation)
        // Note: StubUposDevice might not have direct state access except via methods
        // but we can check after Open
    }

    /// <summary>Verifies that synchronization properties reflect the actual state after lifecycle transitions.</summary>
    [Fact]
    public async Task SyncPropertiesReflectActualState()
    {
        // Arrange
        using var device = new StubUposDevice();

        // Act (Open)
        await device.OpenAsync(TestContext.Current.CancellationToken);
        device.IsOpen.ShouldBeTrue();
        device.IsClaimed.ShouldBeFalse();
        device.IsEnabled.ShouldBeFalse();

        // Act (Claim)
        await device.ClaimAsync(100, TestContext.Current.CancellationToken);
        device.IsClaimed.ShouldBeTrue();
        device.IsEnabled.ShouldBeFalse();

        // Act (Enable)
        await device.SetEnabledAsync(true, TestContext.Current.CancellationToken);
        device.IsClaimed.ShouldBeTrue();
        device.IsEnabled.ShouldBeTrue();

        // Act (Disable)
        await device.SetEnabledAsync(false, TestContext.Current.CancellationToken);
        device.IsClaimed.ShouldBeTrue();
        device.IsEnabled.ShouldBeFalse();

        // Act (Close)
        await device.CloseAsync(TestContext.Current.CancellationToken);
        device.IsOpen.ShouldBeFalse();
    }
}
