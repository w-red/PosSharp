using Xunit;

namespace PosSharp.Core.Tests;

public class AtomicStateTests
{
    private sealed record TestState(string Value);

    [Fact]
    public void Constructor_ShouldInitializeWithState()
    {
        var initial = new TestState("Initial");
        var atomic = new AtomicState<TestState>(initial);

        Assert.Same(initial, atomic.Current);
    }

    [Fact]
    public void Constructor_ShouldThrowOnNull()
    {
        Assert.Throws<ArgumentNullException>(() => new AtomicState<TestState>(null!));
    }

    [Fact]
    public void Transition_ShouldUpdateState()
    {
        var atomic = new AtomicState<TestState>(new TestState("Initial"));
        var next = new TestState("Next");

        var result = atomic.Transition(s => next);

        Assert.True(result.Changed);
        Assert.Equal("Initial", result.OldState.Value);
        Assert.Equal("Next", result.NewState.Value);
        Assert.Same(next, atomic.Current);
    }

    [Fact]
    public void Transition_ShouldNotUpdate_WhenSameReferenceReturned()
    {
        var initial = new TestState("Initial");
        var atomic = new AtomicState<TestState>(initial);

        var result = atomic.Transition(s => s);

        Assert.False(result.Changed);
        Assert.Same(initial, result.OldState);
        Assert.Same(initial, result.NewState);
        Assert.Same(initial, atomic.Current);
    }

    [Fact]
    public void Transition_ShouldHandleNullTransform()
    {
        var atomic = new AtomicState<TestState>(new TestState("Initial"));
        Assert.Throws<ArgumentNullException>(() => atomic.Transition(null!));
    }

    [Fact]
    public void Exchange_ShouldReplaceState()
    {
        var initial = new TestState("Initial");
        var atomic = new AtomicState<TestState>(initial);
        var next = new TestState("Next");

        var old = atomic.Exchange(next);

        Assert.Same(initial, old);
        Assert.Same(next, atomic.Current);
    }

    [Fact]
    public void ConcurrentTransitions_ShouldBeConsistent()
    {
        var atomic = new AtomicState<int[]>(new int[] { 0 });
        const int iterations = 1000;
        const int threadCount = 10;

        var threads = Enumerable.Range(0, threadCount).Select(_ => new Thread(() =>
        {
            for (int i = 0; i < iterations; i++)
            {
                atomic.Transition(s => new int[] { s[0] + 1 });
            }
        })).ToList();

        threads.ForEach(t => t.Start());
        threads.ForEach(t => t.Join());

        Assert.Equal(iterations * threadCount, atomic.Current[0]);
    }
}
