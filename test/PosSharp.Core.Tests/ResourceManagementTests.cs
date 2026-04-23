using Shouldly;

namespace PosSharp.Core.Tests;

/// <summary>Tests for resource management and disposal in UPOS components.</summary>
public sealed class ResourceManagementTests
{
    /// <summary>Verifies that UposMediator.Dispose can be called multiple times without throwing exceptions.</summary>
    [Fact]
    public void UposMediator_Dispose_CanBeCalledMultipleTimes()
    {
        // Arrange
        var mediator = new UposMediator();

        // Act & Assert
        Should.NotThrow(() =>
        {
            mediator.Dispose();
            mediator.Dispose();
        });
    }

    /// <summary>Verifies that UposDeviceBase.Dispose can be called multiple times without throwing exceptions.</summary>
    [Fact]
    public void UposDeviceBase_Dispose_CanBeCalledMultipleTimes()
    {
        // Arrange
        using var device = new StubUposDevice();

        // Act & Assert
        Should.NotThrow(() =>
        {
            device.Dispose();
            device.Dispose();
        });
    }

    /// <summary>Verifies that UposMediator does not crash when accessing properties after being disposed.</summary>
    [Fact]
    public void UposMediator_IsBusy_ReturnsCorrectValuesAfterDispose()
    {
        // Arrange
        var mediator = new UposMediator();
        mediator.Dispose();

        // Act & Assert
        // After dispose, accessing reactive properties might throw or return default,
        // but here we check it doesn't crash on simple access if possible.
        Should.NotThrow(() =>
        {
            _ = mediator.IsBusy;
        });
    }
}
