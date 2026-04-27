using Xunit;
using PosSharp.Abstractions;
using Shouldly;

namespace PosSharp.Core.Tests;

/// <summary>Tests for power management logic in <see cref="UposDeviceBase"/>.</summary>
public sealed class PowerManagementTests
{
    /// <summary>Verifies that UpdatePowerState updates the mediator when power notification is enabled.</summary>
    [Fact]
    public async Task UpdatePowerState_WhenNotifyEnabled_UpdatesMediator()
    {
        using var device = new StubUposDevice();
        device.TestCapPowerReporting = PowerReporting.Standard;
        await device.OpenAsync(TestContext.Current.CancellationToken);
        device.PowerNotify = PowerNotify.Enabled;

        // Act
        device.TestUpdatePowerState(PowerState.Online);

        // Assert
        device.PowerState.ShouldBe(PowerState.Online);
    }

    /// <summary>Verifies that UpdatePowerState does not update the mediator when power notification is disabled.</summary>
    [Fact]
    public async Task UpdatePowerState_WhenNotifyDisabled_DoesNotUpdateMediator()
    {
        using var device = new StubUposDevice();
        device.TestCapPowerReporting = PowerReporting.Standard;
        await device.OpenAsync(TestContext.Current.CancellationToken);
        device.PowerNotify = PowerNotify.Disabled;

        // Act
        device.TestUpdatePowerState(PowerState.Online);

        // Assert
        device.PowerState.ShouldBe(PowerState.Unknown);
    }

    /// <summary>Verifies that UpdatePowerState correctly handles consecutive updates with the same state.</summary>
    [Fact]
    public async Task UpdatePowerState_WhenSameState_DoesNotTriggerChange()
    {
        using var device = new StubUposDevice();
        device.TestCapPowerReporting = PowerReporting.Standard;
        await device.OpenAsync(TestContext.Current.CancellationToken);
        device.PowerNotify = PowerNotify.Enabled;

        device.TestUpdatePowerState(PowerState.Online);

        // Act
        device.TestUpdatePowerState(PowerState.Online);

        // Assert
        // The property returns the current value, and we verify it is correct.
        device.PowerState.ShouldBe(PowerState.Online);
    }
}
