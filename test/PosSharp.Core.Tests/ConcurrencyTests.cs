using Xunit;
using PosSharp.Abstractions;
using R3;

namespace PosSharp.Core.Tests;

/// <summary>Tests for concurrency and event queuing logic.</summary>
public class ConcurrencyTests
{
    /// <summary>Verifies that only one operation can succeed when multiple threads call BeginOperation concurrently.</summary>
    [Fact]
    public async Task BeginOperation_ConcurrentCalls_OnlyOneSucceeds()
    {
        // Arrange
        var mediator = new UposMediator();
        mediator.UpdateState(ControlState.Enabled);
        
        const int taskCount = 100;
        var tasks = new Task<bool>[taskCount];
        int successCount = 0;

        // Act
        for (int i = 0; i < taskCount; i++)
        {
            tasks[i] = Task.Run(async () =>
            {
                try
                {
                    using (mediator.BeginOperation())
                    {
                        // Simulate some work
                        await Task.Delay(100, TestContext.Current.CancellationToken);
                        Interlocked.Increment(ref successCount);
                        return true;
                    }
                }
                catch (UposStateException)
                {
                    return false;
                }
            });
        }

        await Task.WhenAll(tasks);

        // Assert
        // In a strictly concurrent environment, only one should succeed at any given time.
        // However, since they might execute sequentially depending on scheduler, 
        // we check if they at least don't overlap.
        // Actually, with Thread.Sleep(10), many will overlap.
        Assert.Equal(1, successCount);
    }

    /// <summary>Verifies that data events are queued when DataEventEnabled is false.</summary>
    [Fact]
    public void PublishDataEvent_WhenDisabled_QueuesEvents()
    {
        // Arrange
        var device = new StubUposDevice
        {
            DataEventEnabled = false
        };
        int eventCount = 0;
        device.DataEvents.Subscribe(_ => eventCount++);

        // Act
        device.TestPublishDataEvent(new UposDataEventArgs(1));
        device.TestPublishDataEvent(new UposDataEventArgs(2));

        // Assert
        Assert.Equal(0, eventCount);
        Assert.Equal(2, device.DataCount);
    }

    /// <summary>Verifies that queued events are flushed when DataEventEnabled is set to true.</summary>
    [Fact]
    public void DataEventEnabled_SetToTrue_FlushesQueue()
    {
        // Arrange
        var device = new StubUposDevice
        {
            DataEventEnabled = false
        };
        var receivedStatuses = new List<int>();
        device.DataEvents.Subscribe(e => receivedStatuses.Add(e.Status));

        device.TestPublishDataEvent(new UposDataEventArgs(100));
        device.TestPublishDataEvent(new UposDataEventArgs(200));

        // Act
        device.DataEventEnabled = true;

        // Assert
        Assert.Equal(2, receivedStatuses.Count);
        Assert.Contains(100, receivedStatuses);
        Assert.Contains(200, receivedStatuses);
        Assert.Equal(0, device.DataCount);
    }

    /// <summary>Verifies that ClearInputAsync correctly clears the event queue.</summary>
    [Fact]
    public async Task ClearInput_ClearsQueue()
    {
        // Arrange
        var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);
        await device.ClaimAsync(0, TestContext.Current.CancellationToken);
        device.DataEventEnabled = false;

        device.TestPublishDataEvent(new UposDataEventArgs(1));
        Assert.Equal(1, device.DataCount);

        // Act
        await device.ClearInputAsync(TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(0, device.DataCount);
    }
    
    /// <summary>Verifies that AutoDisable correctly stops event firing and leaves remaining events in the queue.</summary>
    [Fact]
    public void AutoDisable_WorksWithQueuedEvents()
    {
        // Arrange
        var device = new StubUposDevice
        {
            AutoDisable = true,
            DataEventEnabled = false
        };

        int eventCount = 0;
        device.DataEvents.Subscribe(_ => eventCount++);

        device.TestPublishDataEvent(new UposDataEventArgs(1));
        device.TestPublishDataEvent(new UposDataEventArgs(2));

        // Act
        device.DataEventEnabled = true;

        // Assert
        Assert.Equal(1, eventCount); // Only one should be fired before AutoDisable kicks in
        Assert.False(device.DataEventEnabled);
        Assert.Equal(1, device.DataCount); // One remains in queue
    }
}
