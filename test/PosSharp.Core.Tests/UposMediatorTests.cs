using Xunit;
using PosSharp.Abstractions;
using R3;
using Shouldly;

namespace PosSharp.Core.Tests;

/// <summary>Tests for the <see cref="UposMediator"/> class.</summary>
public sealed class UposMediatorTests
{
    /// <summary>Verifies that the initial state of the mediator is correct.</summary>
    [Fact]
    public void InitialState_IsCorrect()
    {
        // Arrange & Act
        using var mediator = new UposMediator();

        // Assert
        mediator.CurrentState.ShouldBe(ControlState.Closed);
        mediator.State.CurrentValue.ShouldBe(ControlState.Closed);
        mediator.IsBusyValue.ShouldBeFalse();
        mediator.IsBusy.CurrentValue.ShouldBeFalse();
        mediator.LastError.CurrentValue.ShouldBe(UposErrorCode.Success);
        mediator.LastErrorExtended.CurrentValue.ShouldBe(0);
        mediator.DataCount.CurrentValue.ShouldBe(0);
        mediator.PowerState.CurrentValue.ShouldBe(PowerState.Unknown);
        mediator.CheckHealthText.CurrentValue.ShouldBe(string.Empty);
    }

    /// <summary>Verifies that UpdateState fires events only when the state actually changes.</summary>
    [Fact]
    public void UpdateState_FiresEvent_OnlyWhenChanged()
    {
        // Arrange
        using var mediator = new UposMediator();
        var callCount = 0;
        using var sub = mediator.State.Subscribe(_ => callCount++);

        // Act
        mediator.UpdateState(ControlState.Idle);
        mediator.UpdateState(ControlState.Idle); // Should not fire

        // Assert
        mediator.CurrentState.ShouldBe(ControlState.Idle);
        callCount.ShouldBe(2); // 1 (initial) + 1 (change)
    }

    /// <summary>Verifies that SetBusy fires events only when the busy status actually changes.</summary>
    [Fact]
    public void SetBusy_FiresEvent_OnlyWhenChanged()
    {
        // Arrange
        using var mediator = new UposMediator();
        var callCount = 0;
        using var sub = mediator.IsBusy.Subscribe(_ => callCount++);

        // Act
        mediator.SetBusy(true);
        mediator.SetBusy(true);

        // Assert
        mediator.IsBusyValue.ShouldBeTrue();
        callCount.ShouldBe(2); // Initial (false) + Change (true)
    }

    /// <summary>Verifies that BeginOperation acquires the lock and resets it when disposed.</summary>
    [Fact]
    public void BeginOperation_AcquiresLock_AndResetsOnDispose()
    {
        // Arrange
        using var mediator = new UposMediator();
        mediator.UpdateState(ControlState.Enabled);

        // Act
        using (mediator.BeginOperation())
        {
            mediator.IsBusyValue.ShouldBeTrue();
            
            // Should throw if called again while busy
            Should.Throw<UposStateException>(() => mediator.BeginOperation());
        }

        // Assert
        mediator.IsBusyValue.ShouldBeFalse();
    }

    /// <summary>Verifies that BeginOperation throws an exception when the device is not enabled.</summary>
    [Fact]
    public void BeginOperation_Throws_WhenNotEnabled()
    {
        // Arrange
        using var mediator = new UposMediator();
        mediator.UpdateState(ControlState.Idle);

        // Act & Assert
        Should.Throw<UposStateException>(() => mediator.BeginOperation());
        mediator.IsBusyValue.ShouldBeFalse();
        mediator.IsBusy.CurrentValue.ShouldBeFalse(); // Logic kill: check reactive property reset
    }

    /// <summary>Verifies that UpdateCheckHealthText correctly updates the property.</summary>
    [Fact]
    public void UpdateCheckHealthText_UpdatesProperty()
    {
        // Arrange
        using var mediator = new UposMediator();
        const string text = "Health Check OK";

        // Act
        mediator.UpdateCheckHealthText(text);

        // Assert
        mediator.CheckHealthText.CurrentValue.ShouldBe(text);
    }

    /// <summary>Verifies that ReportError updates the error-related properties.</summary>
    [Fact]
    public void ReportError_UpdatesProperties()
    {
        // Arrange
        using var mediator = new UposMediator();

        // Act
        mediator.ReportError(UposErrorCode.Failure, 101);

        // Assert
        mediator.LastError.CurrentValue.ShouldBe(UposErrorCode.Failure);
        mediator.LastErrorExtended.CurrentValue.ShouldBe(101);
    }

    /// <summary>Verifies that UpdateDataCount fires events only when the count changes.</summary>
    [Fact]
    public void UpdateDataCount_FiresEvent_OnlyWhenChanged()
    {
        // Arrange
        using var mediator = new UposMediator();
        var callCount = 0;
        using var sub = mediator.DataCount.Subscribe(_ => callCount++);

        // Act
        mediator.UpdateDataCount(5);
        mediator.UpdateDataCount(5);

        // Assert
        mediator.DataCount.CurrentValue.ShouldBe(5);
        callCount.ShouldBe(2); // Initial (0) + Change (5)
    }
}


