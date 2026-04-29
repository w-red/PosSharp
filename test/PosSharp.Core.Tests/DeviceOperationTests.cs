using Xunit;
using PosSharp.Abstractions;
using Shouldly;

namespace PosSharp.Core.Tests;

/// <summary>Tests for standard UPOS device operations in <see cref="UposDeviceBase"/>.</summary>
public sealed class DeviceOperationTests
{
    /// <summary>Verifies that DirectIO calls are correctly propagated to the hook method.</summary>
    [Fact]
    public async Task DirectIOAsync_PropagatesCallsCorrectly()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);
        await device.ClaimAsync(1000, TestContext.Current.CancellationToken);
        await device.SetEnabledAsync(true, TestContext.Current.CancellationToken);

        const int command = 123;
        const int data = 456;
        const string obj = "TestData";

        // Act
        await device.DirectIOAsync(command, data, obj, TestContext.Current.CancellationToken);

        // Assert
        device.LastDirectIOCommand.ShouldBe(command);
        device.LastDirectIOData.ShouldBe(data);
        device.LastDirectIOObject.ShouldBe(obj);
    }

    /// <summary>Verifies that ClearInputAsync is correctly propagated to the hook method.</summary>
    [Fact]
    public async Task ClearInputAsync_PropagatesCall()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);
        await device.ClaimAsync(1000, TestContext.Current.CancellationToken);

        // Act
        await device.ClearInputAsync(TestContext.Current.CancellationToken);

        // Assert
        device.ClearInputCalled.ShouldBeTrue();
    }

    /// <summary>Verifies that ClearOutputAsync is correctly propagated to the hook method.</summary>
    [Fact]
    public async Task ClearOutputAsync_PropagatesCall()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);
        await device.ClaimAsync(1000, TestContext.Current.CancellationToken);

        // Act
        await device.ClearOutputAsync(TestContext.Current.CancellationToken);

        // Assert
        device.ClearOutputCalled.ShouldBeTrue();
    }

    /// <summary>Verifies that operations throw UposStateException when called in an invalid state.</summary>
    [Fact]
    public async Task Operations_ThrowException_WhenInvalidState()
    {
        // Arrange
        using var device = new StubUposDevice();

        // remains in Closed state
        // Assert
        await Should.ThrowAsync<UposStateException>(() => device.DirectIOAsync(1, 1, string.Empty, TestContext.Current.CancellationToken));
        await Should.ThrowAsync<UposStateException>(() => device.ClearInputAsync(TestContext.Current.CancellationToken));
        await Should.ThrowAsync<UposStateException>(() => device.ClearOutputAsync(TestContext.Current.CancellationToken));
    }

    /// <summary>Verifies that OpenAsync calls the internal hook method when the device is closed.</summary>
    [Fact]
    public async Task OpenAsync_CallsHook_WhenClosed()
    {
        // Arrange
        using var device = new StubUposDevice();

        // Act
        await device.OpenAsync(TestContext.Current.CancellationToken);

        // Assert
        device.OpenCalled.ShouldBeTrue();
    }

    /// <summary>Verifies that CloseAsync returns early if the device is already closed.</summary>
    [Fact]
    public async Task CloseAsync_EarlyReturns_WhenAlreadyClosed()
    {
        // Arrange
        using var device = new StubUposDevice();

        // Act
        await device.CloseAsync(TestContext.Current.CancellationToken);

        // Assert
        device.CloseCalled.ShouldBeFalse();
    }

    /// <summary>Verifies that CloseAsync calls the internal hook method when the device is open.</summary>
    [Fact]
    public async Task CloseAsync_CallsHook_WhenOpen()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);

        // Act
        await device.CloseAsync(TestContext.Current.CancellationToken);

        // Assert
        device.CloseCalled.ShouldBeTrue();
    }

    /// <summary>Verifies that ClaimAsync returns early if the device is already claimed or enabled.</summary>
    [Fact]
    public async Task ClaimAsync_EarlyReturns_WhenAlreadyClaimedOrEnabled()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);

        // Act
        await device.ClaimAsync(100, TestContext.Current.CancellationToken);

        // Assert
        device.ClaimCalled.ShouldBeTrue();
        device.ClaimCallCount.ShouldBe(1);

        // Act - Second claim (already claimed) -> should early return
        await device.ClaimAsync(100, TestContext.Current.CancellationToken);

        // Assert
        device.ClaimCallCount.ShouldBe(1); // Still 1

        // Enable
        await device.SetEnabledAsync(true, TestContext.Current.CancellationToken);
        device.EnableCallCount.ShouldBe(1);

        // Act - Claim while enabled -> should early return
        await device.ClaimAsync(100, TestContext.Current.CancellationToken);

        // Assert
        device.ClaimCallCount.ShouldBe(1); // Still 1
    }

    /// <summary>Verifies that ReleaseAsync calls the internal hook method.</summary>
    [Fact]
    public async Task ReleaseAsync_CallsHook()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);
        await device.ClaimAsync(100, TestContext.Current.CancellationToken);
        device.ReleaseCalled.ShouldBeFalse();

        // Act
        await device.ReleaseAsync(TestContext.Current.CancellationToken);

        // Assert
        device.ReleaseCalled.ShouldBeTrue();
    }

    /// <summary>Verifies that SetEnabledAsync returns early if the requested state matches the current state.</summary>
    [Fact]
    public async Task SetEnabledAsync_EarlyReturns_WhenStateMatches()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);
        await device.ClaimAsync(100, TestContext.Current.CancellationToken);

        // Assert - Initial state
        device.EnableCallCount.ShouldBe(0);
        device.DisableCallCount.ShouldBe(0);

        // Act - Enable
        await device.SetEnabledAsync(true, TestContext.Current.CancellationToken);

        // Assert
        device.EnableCalled.ShouldBeTrue();
        device.EnableCallCount.ShouldBe(1);

        // Act - Call again with true (already enabled) -> should early return
        await device.SetEnabledAsync(true, TestContext.Current.CancellationToken);

        // Assert - EnableCallCount should still be 1
        device.EnableCallCount.ShouldBe(1);
        device.DisableCallCount.ShouldBe(0);

        // Act - Disable
        await device.SetEnabledAsync(false, TestContext.Current.CancellationToken);

        // Assert
        device.DisableCalled.ShouldBeTrue();
        device.DisableCallCount.ShouldBe(1);
        device.IsClaimed.ShouldBeTrue();
        device.IsEnabled.ShouldBeFalse();

        // Act - Call again with false (already disabled/claimed) -> should early return
        await device.SetEnabledAsync(false, TestContext.Current.CancellationToken);

        // Assert
        device.DisableCallCount.ShouldBe(1); // Still 1
    }

    /// <summary>Verifies that CheckHealthAsync calls the internal hook method.</summary>
    [Fact]
    public async Task HealthCheck_CallsHook()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);
        await device.ClaimAsync(100, TestContext.Current.CancellationToken);
        await device.SetEnabledAsync(true, TestContext.Current.CancellationToken);

        // Act
        await device.CheckHealthAsync(HealthCheckLevel.Internal, TestContext.Current.CancellationToken);

        // Assert
        device.CheckHealthCalled.ShouldBeTrue();
    }

    /// <summary>Verifies that all asynchronous operations respect the provided CancellationToken.</summary>
    [Fact]
    public async Task Operations_RespectCancellationToken()
    {
        // Arrange
        using var device = new StubUposDevice();
        await device.OpenAsync(TestContext.Current.CancellationToken);
        await device.ClaimAsync(1000, TestContext.Current.CancellationToken);
        await device.SetEnabledAsync(true, TestContext.Current.CancellationToken);

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act & Assert
        await Should.ThrowAsync<OperationCanceledException>(async () => await device.OpenAsync(cts.Token));
        await Should.ThrowAsync<OperationCanceledException>(async () => await device.CloseAsync(cts.Token));
        await Should.ThrowAsync<OperationCanceledException>(async () => await device.ClaimAsync(1, cts.Token));
        await Should.ThrowAsync<OperationCanceledException>(async () => await device.ReleaseAsync(cts.Token));
        await Should.ThrowAsync<OperationCanceledException>(async () => await device.SetEnabledAsync(true, cts.Token));
        await Should.ThrowAsync<OperationCanceledException>(async () =>
            await device.CheckHealthAsync(HealthCheckLevel.Internal, cts.Token)
        );
        await Should.ThrowAsync<OperationCanceledException>(async () =>
            await device.DirectIOAsync(1, 1, string.Empty, cts.Token)
        );
        await Should.ThrowAsync<OperationCanceledException>(async () => await device.ClearInputAsync(cts.Token));
        await Should.ThrowAsync<OperationCanceledException>(async () => await device.ClearOutputAsync(cts.Token));
    }

    /// <summary>Verifies that setting PowerNotify throws UposException when power reporting is not supported.</summary>
    [Fact]
    public void PowerNotify_ThrowsWhenNotSupported()
    {
        // Arrange
        using var device = new StubUposDevice();
        device.TestCapPowerReporting = PowerReporting.None;

        // Act & Assert
        // Setting to Disabled should be allowed even if reporting is None
        Should.NotThrow(() => device.PowerNotify = PowerNotify.Disabled);

        // Setting to Enabled should throw when reporting is None
        var ex = Should.Throw<UposException>(() => device.PowerNotify = PowerNotify.Enabled);
        ex.ErrorCode.ShouldBe(UposErrorCode.Illegal);
    }

    [Fact]
    public void AddDisposable_DisposesWithDevice()
    {
        // Arrange
        var device = new StubUposDevice();
        var disposable = new StubDisposable();
        device.TestAddDisposable(disposable);

        // Act
        device.Dispose();

        // Assert
        disposable.IsDisposed.ShouldBeTrue();
    }

    [Fact]
    public void AddDisposables_DisposesAllWithDevice()
    {
        // Arrange
        var device = new StubUposDevice();
        var d1 = new StubDisposable();
        var d2 = new StubDisposable();
        device.TestAddDisposables(d1, d2);

        // Act
        device.Dispose();

        // Assert
        d1.IsDisposed.ShouldBeTrue();
        d2.IsDisposed.ShouldBeTrue();
    }

    private sealed class StubDisposable : IDisposable
    {
        public bool IsDisposed { get; private set; }
        public void Dispose()
        {
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }
    }

    [Fact]
    public void InitializeCapabilities_UpdatesCapabilitiesProperty()
    {
        // Arrange
        using var device = new StubUposDevice();
        var caps = new Dictionary<string, object>
        {
            { "TestCap", 123 }
        };

        // Act
        device.TestInitializeCapabilities(caps);

        // Assert
        device.Capabilities.ShouldNotBeNull();
        device.Capabilities.AsInt("TestCap").ShouldBe(123);
    }
}
