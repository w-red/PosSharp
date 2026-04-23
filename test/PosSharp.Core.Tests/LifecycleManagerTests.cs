using PosSharp.Abstractions;
using PosSharp.Core;
using PosSharp.Core.Lifecycle;
using R3;
using Shouldly;

namespace PosSharp.Core.Tests;

public sealed class LifecycleManagerTests
{
    [Fact]
    public async Task Reset_ResetsAllStateProperties()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync();
        await device.ClaimAsync(1000);
        await device.SetEnabledAsync(true);
        device.TestUpdateError(UposErrorCode.Failure);

        using (device.TestBeginOperation())
        {
            device.IsBusyValue.ShouldBeTrue();

            // Act
            device.Lifecycle.Reset();

            // Assert
            device.IsOpen.ShouldBeFalse();
            device.IsBusyValue.ShouldBeFalse();
            device.LastError.CurrentValue.ShouldBe(UposErrorCode.Success);
        }
    }

    [Fact]
    public void TransitionTo_UpdatesMediatorState()
    {
        // Arrange
        var mediator = new UposMediator();
        var handler = new StandardLifecycleHandler();
        var manager = new UposLifecycleManager(mediator, handler);

        // Act
        manager.TransitionTo(ControlState.Idle);

        // Assert
        mediator.CurrentState.ShouldBe(ControlState.Idle);
    }

    [Fact]
    public void TransitionTo_WhenDisabled_BypassesValidation()
    {
        // Arrange
        var mediator = new UposMediator();
        var handler = new StandardLifecycleHandler();
        var manager = new UposLifecycleManager(mediator, handler);
        manager.IsStateVerificationEnabled = false;

        // Act & Assert (Closed -> Enabled is normally illegal)
        Should.NotThrow(() => manager.TransitionTo(ControlState.Enabled));
        mediator.CurrentState.ShouldBe(ControlState.Enabled);
    }

    [Fact]
    public async Task IsStateVerificationEnabled_WhenFalse_BypassesValidation()
    {
        // Arrange
        using var device = new StubUposDevice();

        // Skip Open/Claim to remain in Closed
        // Act
        device.Lifecycle.IsStateVerificationEnabled = false;

        // Act & Assert (Should NOT throw even if in Closed state)
        await Should.NotThrowAsync(() => device.SetEnabledAsync(true));
    }

    [Fact]
    public void IsStateVerificationEnabled_TogglesCorrectly()
    {
        var mediator = new UposMediator();
        var handler = new StandardLifecycleHandler();
        var manager = new UposLifecycleManager(mediator, handler);
        manager.IsStateVerificationEnabled.ShouldBeTrue();

        manager.IsStateVerificationEnabled = false;
        manager.IsStateVerificationEnabled.ShouldBeFalse();
    }

    [Fact]
    public void VerifyState_WithSingleState_BypassesWhenDisabled()
    {
        // Arrange
        var mediator = new UposMediator();
        var handler = new StandardLifecycleHandler();
        var manager = new UposLifecycleManager(mediator, handler);
        manager.IsStateVerificationEnabled = false;

        // Act & Assert (Should not throw even if Closed < Enabled)
        Should.NotThrow(() => manager.VerifyState(ControlState.Enabled));
    }

    [Fact]
    public void VerifyState_WithMultipleStates_ThrowsWhenInvalid()
    {
        // Arrange
        var mediator = new UposMediator();
        var handler = new StandardLifecycleHandler();
        var manager = new UposLifecycleManager(mediator, handler);

        // Act & Assert
        var ex = Should.Throw<UposStateException>(() =>
            manager.VerifyState(ControlState.Claimed, ControlState.Enabled)
        );
        ex.CurrentState.ShouldBe(ControlState.Closed);
        ex.AllowedStates.ShouldBe(new[] { ControlState.Claimed, ControlState.Enabled });
    }

    [Fact]
    public void VerifyState_WithMultipleStates_SucceedsWhenValid()
    {
        // Arrange
        var mediator = new UposMediator();
        mediator.UpdateState(ControlState.Claimed);
        var handler = new StandardLifecycleHandler();
        var manager = new UposLifecycleManager(mediator, handler);

        // Act & Assert
        Should.NotThrow(() => manager.VerifyState(ControlState.Claimed, ControlState.Enabled));
    }

    [Fact]
    public void VerifyState_WithSingleState_ThrowsWhenInvalid()
    {
        // Arrange
        var mediator = new UposMediator();
        var handler = new StandardLifecycleHandler();
        var manager = new UposLifecycleManager(mediator, handler);

        // Act & Assert
        Should.Throw<UposStateException>(() => manager.VerifyState(ControlState.Idle));
    }

    [Fact]
    public void IsStateVerificationEnabled_TogglesValidation()
    {
        // Arrange
        var mediator = new UposMediator();
        var handler = new StandardLifecycleHandler();
        var manager = new UposLifecycleManager(mediator, handler);
        manager.TransitionTo(ControlState.Idle); // Current is Idle

        // Act & Assert (Enabled: Should throw)
        manager.IsStateVerificationEnabled = true;
        Should.Throw<UposStateException>(() => manager.TransitionTo(ControlState.Enabled));

        // Act & Assert (Disabled: Should NOT throw, even if transition is invalid)
        manager.IsStateVerificationEnabled = false;
        Should.NotThrow(() => manager.TransitionTo(ControlState.Enabled));
        mediator.CurrentState.ShouldBe(ControlState.Enabled);
    }

    [Fact]
    public void PreMethods_SkipValidation_WhenDisabled()
    {
        // Arrange
        var mediator = new UposMediator();
        var handler = new StandardLifecycleHandler();
        var manager = new UposLifecycleManager(mediator, handler);
        manager.IsStateVerificationEnabled = false;

        // Act & Assert (Should NOT throw even if state is invalid)
        Should.NotThrow(() => manager.PreClaim()); // Closed -> Claimed is normally invalid
        Should.NotThrow(() => manager.PreEnable());
    }

    [Fact]
    public void LifecycleMethods_ThrowWhenInvalidState_AndEnabled()
    {
        // Arrange
        var mediator = new UposMediator();
        var handler = new StandardLifecycleHandler();
        var manager = new UposLifecycleManager(mediator, handler);

        // remains in Closed state
        // Act & Assert
        Should.Throw<UposStateException>(() => manager.PreClaim()); // Closed -> Claimed (Forbidden)
        Should.Throw<UposStateException>(() => manager.PreEnable()); // Closed -> Enabled (Forbidden)
        Should.Throw<UposStateException>(() => manager.PreDisable()); // Closed -> Claimed (Forbidden)
    }

    [Fact]
    public void LifecycleMethods_CallValidator_WhenEnabled()
    {
        // Arrange
        var mediator = new UposMediator();
        var handler = new MockLifecycleHandler();
        var manager = new UposLifecycleManager(mediator, handler);

        // Act
        manager.PreOpen();
        manager.PreClose();
        manager.PreClaim();
        manager.PreRelease();
        manager.PreEnable();
        manager.PreDisable();
        manager.TransitionTo(ControlState.Idle);
        manager.VerifyState(ControlState.Closed);

        // Assert
        handler.ValidatedTransitions.Count.ShouldBe(7); // PreXxx (6) + TransitionTo (1)
        handler.StateVerifications.Count.ShouldBe(1);
    }

    [Fact]
    public void VerifyState_Params_BypassesWhenDisabled()
    {
        // Arrange
        var mediator = new UposMediator();
        var handler = new MockLifecycleHandler();
        var manager = new UposLifecycleManager(mediator, handler);
        manager.IsStateVerificationEnabled = false;

        // Act
        manager.VerifyState(ControlState.Idle, ControlState.Busy);

        // Assert - Should not access mediator.CurrentState or anything
        // (Just verifying it doesn't throw or do work)
    }

    [Fact]
    public void PostMethods_UpdateMediatorStateCorrectly()
    {
        // Arrange
        var mediator = new UposMediator();
        var handler = new StandardLifecycleHandler();
        var manager = new UposLifecycleManager(mediator, handler);

        // Act & Assert
        manager.PostOpen();
        mediator.CurrentState.ShouldBe(ControlState.Idle);

        manager.PostClaim();
        mediator.CurrentState.ShouldBe(ControlState.Claimed);

        manager.PostEnable();
        mediator.CurrentState.ShouldBe(ControlState.Enabled);

        manager.PostDisable();
        mediator.CurrentState.ShouldBe(ControlState.Claimed);

        manager.PostRelease();
        mediator.CurrentState.ShouldBe(ControlState.Idle);

        manager.PostClose();
        mediator.CurrentState.ShouldBe(ControlState.Closed);
    }

    private sealed class MockLifecycleHandler : IUposLifecycleHandler
    {
        public List<(ControlState Current, ControlState Target)> ValidatedTransitions { get; } = new();
        public List<(ControlState Current, ControlState Required)> StateVerifications { get; } = new();

        public void ValidateTransition(ControlState currentState, ControlState targetState)
        {
            ValidatedTransitions.Add((currentState, targetState));
        }

        public void VerifyState(ControlState currentState, ControlState requiredState)
        {
            StateVerifications.Add((currentState, requiredState));
        }
    }
}
