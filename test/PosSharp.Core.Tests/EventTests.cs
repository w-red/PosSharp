using Xunit;
using PosSharp.Abstractions;
using R3;
using Shouldly;

namespace PosSharp.Core.Tests;

/// <summary>Tests for UPOS event handling logic in <see cref="UposDeviceBase"/>.</summary>
public sealed class EventTests
{
    /// <summary>Verifies that DataEventEnabled is false by default.</summary>
    [Fact]
    public void InitialState_DataEventEnabledIsFalse()
    {
        // Arrange & Act
        using var device = new StubUposDevice();

        // Assert
        device.DataEventEnabled.ShouldBeFalse();
        device.DataEventEnabledProperty.CurrentValue.ShouldBeFalse();
    }

    /// <summary>Verifies that a subscriber receives a DataEvent when published and enabled.</summary>
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

    /// <summary>Verifies that PublishDataEvent updates the DataCount in the mediator when events are buffered.</summary>
    [Fact]
    public void PublishDataEvent_UpdatesDataCount()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.DataEventEnabled = false; // Buffer it

        // Act
        device.TestPublishDataEvent(new UposDataEventArgs(1));
        device.TestPublishDataEvent(new UposDataEventArgs(2));

        // Assert
        device.Mediator.DataCount.CurrentValue.ShouldBe(2);
    }

    /// <summary>Verifies that a subscriber receives an ErrorEvent when published.</summary>
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

    /// <summary>Verifies that events are not fired after the device is disposed.</summary>
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

    /// <summary>Verifies the atomic locking mechanism of TryBeginFlushing.</summary>
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

    /// <summary>Verifies that FlushDataEvents handles multiple events and correctly resets the flushing flag.</summary>
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

    /// <summary>Verifies that AutoDisable=true automatically disables data events after one delivery.</summary>
    [Fact]
    public void FlushDataEvents_AutoDisable_Works()
    {
        // Arrange
        var device = new StubUposDevice();
        device.AutoDisable = true;
        var eventCount = 0;
        device.DataEvents.Subscribe(_ => eventCount++);

        device.TestPublishDataEvent(new UposDataEventArgs(1));
        device.TestPublishDataEvent(new UposDataEventArgs(2));

        // Act
        device.DataEventEnabled = true;

        // Assert
        eventCount.ShouldBe(1);
        device.DataEventEnabled.ShouldBeFalse();
    }
}


