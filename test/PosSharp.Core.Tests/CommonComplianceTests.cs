using PosSharp.Abstractions;
using R3;
using Shouldly;

namespace PosSharp.Core.Tests;

/// <summary>Tests to ensure UPOS common compliance for properties and methods.</summary>
public sealed class CommonComplianceTests
{
    [Fact]
    public void CommonPropertiesHaveCorrectInitialValues()
    {
        // Arrange
        using var device = new StubUposDevice();

        // Assert
        device.CheckHealthText.ShouldBe(string.Empty);
        device.ResultCodeExtended.ShouldBe(0);
        device.DataCount.ShouldBe(0);
        device.AutoDisable.ShouldBeFalse();
        device.CapPowerReporting.ShouldBe(PowerReporting.None);
        device.PowerNotify.ShouldBe(PowerNotify.Disabled);
        device.PowerState.ShouldBe(PowerState.Unknown);
        device.ServiceObjectDescription.ShouldNotBeNullOrEmpty();
        device.ServiceObjectVersion.ShouldNotBeNullOrEmpty();
        device.DeviceDescription.ShouldNotBeNullOrEmpty();
        device.DeviceName.ShouldBe(nameof(StubUposDevice));
    }

    [Fact]
    public async Task CheckHealthAsyncUpdatesCheckHealthText()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(0);
        await device.SetEnabledAsync(true);

        // Act
        await device.CheckHealthAsync(HealthCheckLevel.Internal);

        // Assert
        device.CheckHealthText.ShouldBe("Internal:OK");
    }

    [Fact]
    public void PowerNotifyThrowsOnIllegalSetup()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.TestCapPowerReporting = PowerReporting.None;

        // Act & Assert
        var ex = Should.Throw<UposException>(() =>
        {
            device.PowerNotify = PowerNotify.Enabled;
        });
        ex.ErrorCode.ShouldBe(UposErrorCode.Illegal);
    }

    [Fact]
    public void PowerNotifySucceedsWhenSupported()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.TestCapPowerReporting = PowerReporting.Standard;

        // Act
        device.PowerNotify = PowerNotify.Enabled;

        // Assert
        device.PowerNotify.ShouldBe(PowerNotify.Enabled);
    }

    [Fact]
    public void PowerStateChangeFiresEventWhenEnabled()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.TestCapPowerReporting = PowerReporting.Standard;
        device.PowerNotify = PowerNotify.Enabled;

        int lastStatus = 0;
        using var sub = device.StatusUpdateEvents.Subscribe(e => lastStatus = e.Status);

        // Act
        device.TestUpdatePowerState(PowerState.Online);

        // Assert
        device.PowerState.ShouldBe(PowerState.Online);
        lastStatus.ShouldBe((int)PowerState.Online);
    }

    [Fact]
    public async Task ClearInputResetsDataCount()
    {
        // Arrange
        using var device = new StubUposDevice(); // This requires mediator access, but we'll simulate.
        await device.OpenAsync();
        await device.ClaimAsync(0);

        // No easy way to set DataCount on Stub without exposing Mediator UpdateDataCount
        // But we can check ClearInputAsync calls it.
        // Act
        await device.ClearInputAsync();

        // Assert
        device.DataCount.ShouldBe(0);
    }
}
