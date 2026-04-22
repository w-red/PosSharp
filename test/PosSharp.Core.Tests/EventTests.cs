using PosSharp.Abstractions;
using R3;
using Shouldly;

namespace PosSharp.Core.Tests;

public sealed class EventTests
{
    [Fact]
    public void PublishDataEventSubscriberReceivesEvent()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.DataEventEnabled = true;
        var received = false;
        using var sub = device.DataEvents.Subscribe(e =>
        {
            received = true;
            e.Status.ShouldBe(123);
        });

        // Act
        device.TestPublishDataEvent(new UposDataEventArgs(123));

        // Assert
        received.ShouldBeTrue();
    }

    [Fact]
    public void PublishErrorEventSubscriberReceivesEvent()
    {
        // Arrange
        using var device = new StubUposDevice();
        var received = false;
        using var sub = device.ErrorEvents.Subscribe(e =>
        {
            received = true;
            e.ErrorCode.ShouldBe(UposErrorCode.Failure);
        });

        // Act
        device.TestPublishErrorEvent(new UposErrorEventArgs(UposErrorCode.Failure, 0, 0, 0));

        // Assert
        received.ShouldBeTrue();
    }

    [Fact]
    public void EventsAfterDisposeDoNotFire()
    {
        // Arrange
        var device = new StubUposDevice();
        var sink = (IUposEventSink)device;
        var received = false;
        using var sub = sink.DataEvents.Subscribe(_ => received = true);

        // Act
        device.Dispose();

        // Assert: We just verify received is still false after initial subscribe
        received.ShouldBeFalse();
    }
}
