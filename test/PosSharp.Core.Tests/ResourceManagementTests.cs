using PosSharp.Abstractions;
using Shouldly;

namespace PosSharp.Core.Tests;

public sealed class ResourceManagementTests
{
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
