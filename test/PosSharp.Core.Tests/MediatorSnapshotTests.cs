using PosSharp.Abstractions;
using Xunit;

namespace PosSharp.Core.Tests;

public class MediatorSnapshotTests
{
    [Fact]
    public void Initial_ShouldHaveDefaultValues()
    {
        var initial = MediatorSnapshot.Initial;

        Assert.Equal(ControlState.Closed, initial.State);
        Assert.False(initial.IsBusy);
        Assert.Equal(UposErrorCode.Success, initial.LastError);
        Assert.Equal(0, initial.LastErrorExtended);
        Assert.Equal(0, initial.DataCount);
    }

    [Fact]
    public void Equality_ShouldWorkBasedOnContent()
    {
        var s1 = new MediatorSnapshot(ControlState.Enabled, true, UposErrorCode.Failure, 123, 5);
        var s2 = new MediatorSnapshot(ControlState.Enabled, true, UposErrorCode.Failure, 123, 5);
        var s3 = s1 with { IsBusy = false };

        Assert.Equal(s1, s2);
        Assert.NotEqual(s1, s3);
    }
}
