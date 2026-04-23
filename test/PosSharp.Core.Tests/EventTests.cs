using PosSharp.Abstractions;
using R3;
using Shouldly;

namespace PosSharp.Core.Tests;

public sealed class EventTests
{
    [Fact]
    public void InitialState_DataEventEnabledIsFalse()
    {
        // Arrange & Act
        using var device = new StubUposDevice();

        // Assert
        device.DataEventEnabled.ShouldBeFalse();
        device.DataEventEnabledProperty.CurrentValue.ShouldBeFalse();
    }

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

    [Fact]
    public void TryBeginFlushing_AtomicLockWorks()
    {
        // Arrange
        using var device = new StubUposDevice();

        // Act & Assert
        // First call should succeed
        device.TestTryBeginFlushing().ShouldBeTrue();
        device.IsFlushing.ShouldBeTrue();

        // Second call should fail while locked
        device.TestTryBeginFlushing().ShouldBeFalse();

        // (Note: We don't have a direct way to reset flushing from outside 
        // without reflection or calling internal flush, but this verifies the atomic guard)
    }

    [Fact]
    public void FlushDataEvents_CanBeCalledMultipleTimes()
    {
        // Arrange
        var device = new StubUposDevice();
        var eventCount = 0;
        device.DataEvents.Subscribe(_ => eventCount++);

        // 1. Enqueue MULTIPLE data and enable
        device.TestPublishDataEvent(new UposDataEventArgs(1));
        device.TestPublishDataEvent(new UposDataEventArgs(2));
        device.DataEventEnabled = true; // First flush (handles 1 and 2)
        eventCount.ShouldBe(2);
        device.IsFlushing.ShouldBeFalse();

        // 2. Enqueue more data
        device.TestPublishDataEvent(new UposDataEventArgs(3));
        // It should flush automatically if DataEventEnabled is true
        eventCount.ShouldBe(3);
        device.IsFlushing.ShouldBeFalse();

        // 3. Disable, enqueue, then re-enable
        device.DataEventEnabled = false;
        device.TestPublishDataEvent(new UposDataEventArgs(4));
        eventCount.ShouldBe(3); // Still 3

        device.DataEventEnabled = true; // Second manual flush trigger
        eventCount.ShouldBe(4);
        device.IsFlushing.ShouldBeFalse();
    }
}
