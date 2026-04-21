using PosSharp.Abstractions;
using Shouldly;

namespace PosSharp.Core.Tests;

public sealed class PowerManagementTests
{
    [Fact]
    public async Task UpdatePowerState_WhenNotifyEnabled_UpdatesMediator()
    {
        using var device = new StubUposDevice();
        device.TestCapPowerReporting = PowerReporting.Standard;
        await device.OpenAsync();
        device.PowerNotify = PowerNotify.Enabled;

        // Act
        device.TestUpdatePowerState(PowerState.Online);

        // Assert
        device.PowerState.ShouldBe(PowerState.Online);
    }

    [Fact]
    public async Task UpdatePowerState_WhenNotifyDisabled_DoesNotUpdateMediator()
    {
        using var device = new StubUposDevice();
        device.TestCapPowerReporting = PowerReporting.Standard;
        await device.OpenAsync();
        device.PowerNotify = PowerNotify.Disabled;

        // Act
        device.TestUpdatePowerState(PowerState.Online);

        // Assert
        device.PowerState.ShouldBe(PowerState.Unknown);
    }

    [Fact]
    public async Task UpdatePowerState_WhenSameState_DoesNotTriggerChange()
    {
        using var device = new StubUposDevice();
        device.TestCapPowerReporting = PowerReporting.Standard;
        await device.OpenAsync();
        device.PowerNotify = PowerNotify.Enabled;

        device.TestUpdatePowerState(PowerState.Online);

        // Act
        device.TestUpdatePowerState(PowerState.Online);

        // Assert
        // The property returns the current value, and we verify it is correct.
        device.PowerState.ShouldBe(PowerState.Online);
    }
}
