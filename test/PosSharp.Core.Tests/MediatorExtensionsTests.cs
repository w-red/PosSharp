using Xunit;
using PosSharp.Abstractions;
using Shouldly;

namespace PosSharp.Core.Tests;

public class MediatorExtensionsTests
{
    [Fact]
    public void ValidateOpen_Throws_When_Closed()
    {
        var mediator = new UposMediator();
        mediator.UpdateState(ControlState.Closed);

        Should.Throw<UposStateException>(() => mediator.ValidateOpen())
            .ErrorCode.ShouldBe(UposErrorCode.Closed);
    }

    [Fact]
    public void ValidateOpen_Succeeds_When_Idle()
    {
        var mediator = new UposMediator();
        mediator.UpdateState(ControlState.Idle);

        mediator.ValidateOpen(); // Should not throw
    }

    [Fact]
    public void ValidateClaimed_Throws_When_Idle()
    {
        var mediator = new UposMediator();
        mediator.UpdateState(ControlState.Idle);

        Should.Throw<UposStateException>(() => mediator.ValidateClaimed())
            .ErrorCode.ShouldBe(UposErrorCode.NotClaimed);
    }

    [Fact]
    public void ValidateClaimed_Succeeds_When_Claimed()
    {
        var mediator = new UposMediator();
        mediator.UpdateState(ControlState.Claimed);

        mediator.ValidateClaimed(); // Should not throw
    }

    [Fact]
    public void ValidateEnabled_Throws_When_Claimed()
    {
        var mediator = new UposMediator();
        mediator.UpdateState(ControlState.Claimed);

        Should.Throw<UposStateException>(() => mediator.ValidateEnabled())
            .ErrorCode.ShouldBe(UposErrorCode.Disabled);
    }

    [Fact]
    public void ValidateEnabled_Succeeds_When_Enabled()
    {
        var mediator = new UposMediator();
        mediator.UpdateState(ControlState.Enabled);

        mediator.ValidateEnabled(); // Should not throw
    }

    [Fact]
    public void ValidateNotBusy_Throws_When_Busy()
    {
        var mediator = new UposMediator();
        mediator.SetBusy(true);

        Should.Throw<UposStateException>(() => mediator.ValidateNotBusy())
            .ErrorCode.ShouldBe(UposErrorCode.Busy);
    }

    [Fact]
    public void ValidateNotBusy_Succeeds_When_Not_Busy()
    {
        var mediator = new UposMediator();
        mediator.SetBusy(false);

        mediator.ValidateNotBusy(); // Should not throw
    }
}
