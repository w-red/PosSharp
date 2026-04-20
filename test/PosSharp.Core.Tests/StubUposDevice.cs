// Copyright (c) PosSharp Project. All rights reserved.
// Licensed under the MIT License.

using PosSharp.Abstractions;
using PosSharp.Core;

namespace PosSharp.Core.Tests;

/// <summary>
/// Minimal concrete implementation of <see cref="UposDeviceBase"/> used in unit tests.
/// All hook methods are no-ops that complete immediately.
/// </summary>
internal class StubUposDevice : UposDeviceBase
{
    /// <summary>Exposes the protected BeginOperation method for testing.</summary>
    /// <returns>A disposable to end the operation.</returns>
    public IDisposable TestBeginOperation() => this.BeginOperation();

    /// <summary>Exposes the protected PublishDataEvent method for testing.</summary>
    /// <param name="args">Event args.</param>
    public void TestPublishDataEvent(UposDataEventArgs args) => this.PublishDataEvent(args);

    /// <summary>Exposes the protected PublishErrorEvent method for testing.</summary>
    /// <param name="args">Event args.</param>
    public void TestPublishErrorEvent(UposErrorEventArgs args) => this.PublishErrorEvent(args);

    /// <inheritdoc/>
    protected override Task OnOpenAsync(CancellationToken ct) => Task.CompletedTask;

    /// <inheritdoc/>
    protected override Task OnCloseAsync(CancellationToken ct) => Task.CompletedTask;

    /// <inheritdoc/>
    protected override Task OnClaimAsync(int timeout, CancellationToken ct) => Task.CompletedTask;

    /// <inheritdoc/>
    protected override Task OnReleaseAsync(CancellationToken ct) => Task.CompletedTask;

    /// <inheritdoc/>
    protected override Task OnEnableAsync(CancellationToken ct) => Task.CompletedTask;

    /// <inheritdoc/>
    protected override Task OnDisableAsync(CancellationToken ct) => Task.CompletedTask;
}
