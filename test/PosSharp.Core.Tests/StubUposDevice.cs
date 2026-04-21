// Copyright (c) PosSharp Project. All rights reserved.
// Licensed under the MIT License.

using PosSharp.Abstractions;
using PosSharp.Core;
using PosSharp.Core.Lifecycle;

namespace PosSharp.Core.Tests;

/// <summary>
/// Minimal concrete implementation of <see cref="UposDeviceBase"/> used in unit tests.
/// All hook methods are no-ops that complete immediately.
/// </summary>
internal class StubUposDevice : UposDeviceBase
{
    /// <summary>Gets the CapPowerReporting capability for testing.</summary>
    public override PowerReporting CapPowerReporting => TestCapPowerReporting;

    /// <summary>Gets or sets the CapPowerReporting capability for testing.</summary>
    public PowerReporting TestCapPowerReporting { get; set; } = PowerReporting.None;

    /// <summary>Gets the protected Lifecycle manager for testing.</summary>
    public new UposLifecycleManager Lifecycle => base.Lifecycle;

    /// <summary>Gets the last DirectIO command.</summary>
    public int LastDirectIOCommand { get; private set; }

    /// <summary>Gets the last DirectIO data.</summary>
    public int LastDirectIOData { get; private set; }

    /// <summary>Gets the last DirectIO object.</summary>
    public object? LastDirectIOObject { get; private set; }

    /// <summary>Gets a value indicating whether OnOpenAsync was called.</summary>
    public bool OpenCalled { get; private set; }

    /// <summary>Gets a value indicating whether OnCloseAsync was called.</summary>
    public bool CloseCalled { get; private set; }

    /// <summary>Gets a value indicating whether OnClaimAsync was called.</summary>
    public bool ClaimCalled { get; private set; }

    /// <summary>Gets a value indicating whether OnReleaseAsync was called.</summary>
    public bool ReleaseCalled { get; private set; }

    /// <summary>Gets a value indicating whether OnEnableAsync was called.</summary>
    public bool EnableCalled { get; private set; }

    /// <summary>Gets a value indicating whether OnDisableAsync was called.</summary>
    public bool DisableCalled { get; private set; }

    /// <summary>Gets a value indicating whether OnCheckHealthAsync was called.</summary>
    public bool CheckHealthCalled { get; private set; }

    /// <summary>Gets a value indicating whether OnDirectIOAsync was called.</summary>
    public bool DirectIOCalled { get; private set; }

    /// <summary>Gets a value indicating whether OnClearInputAsync was called.</summary>
    public bool ClearInputCalled { get; private set; }

    /// <summary>Gets a value indicating whether OnClearOutputAsync was called.</summary>
    public bool ClearOutputCalled { get; private set; }

    /// <summary>Exposes the protected BeginOperation method for testing.</summary>
    /// <returns>A disposable to end the operation.</returns>
    public IDisposable TestBeginOperation() => BeginOperation();

    /// <summary>Exposes the protected PublishDataEvent method for testing.</summary>
    /// <param name="args">Event args.</param>
    public void TestPublishDataEvent(UposDataEventArgs args) => PublishDataEvent(args);

    /// <summary>Exposes the protected PublishErrorEvent method for testing.</summary>
    /// <param name="args">Event args.</param>
    public void TestPublishErrorEvent(UposErrorEventArgs args) => PublishErrorEvent(args);

    /// <summary>Exposes the protected UpdatePowerState method for testing.</summary>
    /// <param name="newState">New power state.</param>
    public void TestUpdatePowerState(PowerState newState) => UpdatePowerState(newState);

    /// <summary>Updates the last error for testing.</summary>
    /// <param name="code">Error code.</param>
    public void TestUpdateError(UposErrorCode code) => Mediator.ReportError(code);

    /// <inheritdoc/>
    protected override Task<string> OnCheckHealthAsync(HealthCheckLevel level, CancellationToken ct)
    {
        CheckHealthCalled = true;
        return Task.FromResult("Internal:OK");
    }

    /// <inheritdoc/>
    protected override Task OnDirectIOAsync(int command, int data, object obj, CancellationToken ct)
    {
        DirectIOCalled = true;
        LastDirectIOCommand = command;
        LastDirectIOData = data;
        LastDirectIOObject = obj;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    protected override Task OnClearInputAsync(CancellationToken ct)
    {
        ClearInputCalled = true;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    protected override Task OnClearOutputAsync(CancellationToken ct)
    {
        ClearOutputCalled = true;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    protected override Task OnOpenAsync(CancellationToken ct)
    {
        OpenCalled = true;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    protected override Task OnCloseAsync(CancellationToken ct)
    {
        CloseCalled = true;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    protected override Task OnClaimAsync(int timeout, CancellationToken ct)
    {
        ClaimCalled = true;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    protected override Task OnReleaseAsync(CancellationToken ct)
    {
        ReleaseCalled = true;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    protected override Task OnEnableAsync(CancellationToken ct)
    {
        EnableCalled = true;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    protected override Task OnDisableAsync(CancellationToken ct)
    {
        DisableCalled = true;
        return Task.CompletedTask;
    }
}
