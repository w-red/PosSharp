using Xunit;
using PosSharp.Abstractions;
using R3;
using Shouldly;

namespace PosSharp.Core.Tests;

/// <summary>Tests for <see cref="UposDeviceBase"/> lifecycle state transitions.</summary>
public sealed class LifecycleTests
{
    /// <summary>Verifies that OpenAsync transitions the device from Closed to Idle state.</summary>
    [Fact]
    public async Task OpenAsyncWhenClosedTransitionsToIdle()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.State.CurrentValue.ShouldBe(ControlState.Closed);

        // Act
        await device.OpenAsync(TestContext.Current.CancellationToken);

        // Assert
        device.State.CurrentValue.ShouldBe(ControlState.Idle);
    }

    /// <summary>Verifies that ClaimAsync transitions the device from Idle to Claimed state.</summary>
    [Fact]
    public async Task ClaimAsyncWhenIdleTransitionsToClaimed()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);

        // Act
        await device.ClaimAsync(timeout: 1000, TestContext.Current.CancellationToken);

        // Assert
        device.State.CurrentValue.ShouldBe(ControlState.Claimed);
    }

    /// <summary>Verifies that enabling the device transitions it from Claimed to Enabled state.</summary>
    [Fact]
    public async Task SetEnabledAsyncTrueWhenClaimedTransitionsToEnabled()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);
        await device.ClaimAsync(timeout: 1000, TestContext.Current.CancellationToken);

        // Act
        await device.SetEnabledAsync(true, TestContext.Current.CancellationToken);

        // Assert
        device.State.CurrentValue.ShouldBe(ControlState.Enabled);
    }

    /// <summary>Verifies that disabling the device transitions it from Enabled back to Claimed state.</summary>
    [Fact]
    public async Task SetEnabledAsyncFalseWhenEnabledTransitionsToClaimed()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);
        await device.ClaimAsync(timeout: 1000, TestContext.Current.CancellationToken);
        await device.SetEnabledAsync(true, TestContext.Current.CancellationToken);

        // Act
        await device.SetEnabledAsync(false, TestContext.Current.CancellationToken);

        // Assert
        device.State.CurrentValue.ShouldBe(ControlState.Claimed);
    }

    /// <summary>Verifies that ReleaseAsync transitions the device from Claimed back to Idle state.</summary>
    [Fact]
    public async Task ReleaseAsyncWhenClaimedTransitionsToIdle()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);
        await device.ClaimAsync(timeout: 1000, TestContext.Current.CancellationToken);

        // Act
        await device.ReleaseAsync(TestContext.Current.CancellationToken);

        // Assert
        device.State.CurrentValue.ShouldBe(ControlState.Idle);
    }

    /// <summary>Verifies that CloseAsync transitions the device from Idle back to Closed state.</summary>
    [Fact]
    public async Task CloseAsyncWhenIdleTransitionsToClosed()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);

        // Act
        await device.CloseAsync(TestContext.Current.CancellationToken);

        // Assert
        device.State.CurrentValue.ShouldBe(ControlState.Closed);
    }

    /// <summary>Verifies that CloseAsync resets internal state properties like IsBusy and LastError.</summary>
    [Fact]
    public async Task CloseAsyncResetsInternalState()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);
        await device.ClaimAsync(timeout: 1000, TestContext.Current.CancellationToken);
        await device.SetEnabledAsync(true, TestContext.Current.CancellationToken);

        using (device.TestBeginOperation())
        {
            device.TestUpdateError(UposErrorCode.Failure);

            // Act
            await device.CloseAsync(TestContext.Current.CancellationToken);
        }

        // Assert
        device.State.CurrentValue.ShouldBe(ControlState.Closed);
        device.IsBusyValue.ShouldBeFalse();
        device.LastError.CurrentValue.ShouldBe(UposErrorCode.Success);
    }

    /// <summary>Verifies that Reset directly resets the device state and clears errors.</summary>
    [Fact]
    public void Reset_ClearsStateAtomics()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.Lifecycle.PostOpen();
        device.Lifecycle.PostClaim();
        device.Lifecycle.PostEnable();
        device.TestUpdateError(UposErrorCode.Failure);

        using (device.TestBeginOperation())
        {
            device.IsBusyValue.ShouldBeTrue();

            // Act
            device.Lifecycle.Reset();
        }

        // Assert
        device.State.CurrentValue.ShouldBe(ControlState.Closed);
        device.IsBusyValue.ShouldBeFalse();
        device.LastError.CurrentValue.ShouldBe(UposErrorCode.Success);
    }

    /// <summary>Verifies that VerifyState throws UposStateException when the device is not in the required state.</summary>
    [Fact]
    public void VerifyState_ThrowsWhenStateMismatch()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.State.CurrentValue.ShouldBe(ControlState.Closed);

        // Act & Assert
        Should.Throw<UposStateException>(() => device.Lifecycle.VerifyState(ControlState.Enabled));
    }

    /// <summary>Verifies that VerifyState(params) throws UposStateException when the device is not in any of the allowed states.</summary>
    [Fact]
    public void VerifyState_Params_ThrowsWhenStateMismatch()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.State.CurrentValue.ShouldBe(ControlState.Closed);

        // Act & Assert
        Should.Throw<UposStateException>(() => device.Lifecycle.VerifyState(ControlState.Idle, ControlState.Claimed));
    }

    /// <summary>Verifies that VerifyState(params) does not throw when the device is in one of the allowed states.</summary>
    [Fact]
    public void VerifyState_Params_Success()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.Lifecycle.PostOpen(); // Idle

        // Act & Assert
        Should.NotThrow(() => device.Lifecycle.VerifyState(ControlState.Idle, ControlState.Claimed));
    }

    /// <summary>Verifies that VerifyState does not throw when verification is disabled.</summary>
    [Fact]
    public void VerifyState_DoesNotThrowWhenDisabled()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.IsStateVerificationEnabled = false;

        // Act & Assert
        Should.NotThrow(() => device.Lifecycle.VerifyState(ControlState.Enabled));
    }

    /// <summary>Verifies that VerifyState(params) does not throw when verification is disabled.</summary>
    [Fact]
    public void VerifyState_Params_DoesNotThrowWhenDisabled()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.IsStateVerificationEnabled = false;

        // Act & Assert
        Should.NotThrow(() => device.Lifecycle.VerifyState(ControlState.Idle, ControlState.Claimed));
    }

    /// <summary>Verifies that VerifyState does not throw when the state matches.</summary>
    [Fact]
    public void VerifyState_Single_Success()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.Lifecycle.PostOpen(); // Idle

        // Act & Assert
        Should.NotThrow(() => device.Lifecycle.VerifyState(ControlState.Idle));
    }

    /// <summary>Verifies that TransitionTo allows invalid transitions when verification is disabled.</summary>
    [Fact]
    public void TransitionTo_AllowsInvalidWhenDisabled()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.IsStateVerificationEnabled = false;
        device.State.CurrentValue.ShouldBe(ControlState.Closed);

        // Act
        // Closed -> Enabled is normally invalid
        device.Lifecycle.TransitionTo(ControlState.Enabled);

        // Assert
        device.State.CurrentValue.ShouldBe(ControlState.Enabled);
    }

    /// <summary>Verifies that PreOpen does not throw when verification is disabled, even in invalid states.</summary>
    [Fact]
    public void PreOpen_AllowsInvalidWhenDisabled()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.IsStateVerificationEnabled = false;
        device.Lifecycle.PostOpen(); // Idle
        device.Lifecycle.PostClaim();
        device.Lifecycle.PostEnable(); // Enabled

        // Act & Assert
        // PreOpen normally expects Closed, but it should not throw here
        Should.NotThrow(() => device.Lifecycle.PreOpen());
    }

    /// <summary>Verifies that PreClaim does not throw when verification is disabled, even in invalid states.</summary>
    [Fact]
    public void PreClaim_AllowsInvalidWhenDisabled()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.IsStateVerificationEnabled = false;
        // PreClaim normally expects Idle, but here we are Closed
        Should.NotThrow(() => device.Lifecycle.PreClaim());
    }

    /// <summary>Verifies that PreEnable does not throw when verification is disabled, even in invalid states.</summary>
    [Fact]
    public void PreEnable_AllowsInvalidWhenDisabled()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.IsStateVerificationEnabled = false;
        // PreEnable normally expects Claimed, but here we are Closed
        Should.NotThrow(() => device.Lifecycle.PreEnable());
    }

    /// <summary>Verifies that PreDisable does not throw when verification is disabled, even in invalid states.</summary>
    [Fact]
    public void PreDisable_AllowsInvalidWhenDisabled()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.IsStateVerificationEnabled = false;
        // PreDisable normally expects Enabled, but here we are Closed
        Should.NotThrow(() => device.Lifecycle.PreDisable());
    }

    /// <summary>Verifies that PreRelease does not throw when verification is disabled, even in invalid states.</summary>
    [Fact]
    public void PreRelease_AllowsInvalidWhenDisabled()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.IsStateVerificationEnabled = false;
        // PreRelease normally expects Claimed or Enabled, but here we are Closed
        Should.NotThrow(() => device.Lifecycle.PreRelease());
    }

    /// <summary>Verifies that PreClose does not throw when verification is disabled, even in invalid states.</summary>
    [Fact]
    public void PreClose_AllowsInvalidWhenDisabled()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.IsStateVerificationEnabled = false;
        // PreClose normally expects anything but Closed, but even if it's Closed it shouldn't throw if disabled
        Should.NotThrow(() => device.Lifecycle.PreClose());
    }
}



