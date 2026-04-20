// Copyright (c) PosSharp Project. All rights reserved.
// Licensed under the MIT License.

namespace PosSharp.Core.Tests;

/// <summary>
/// A version of <see cref="StubUposDevice"/> that supports cancellation in its hooks.
/// </summary>
internal sealed class CancellableStubUposDevice : StubUposDevice
{
    /// <inheritdoc/>
    protected override async Task OnOpenAsync(CancellationToken ct)
    {
        await Task.Delay(100, ct);
        ct.ThrowIfCancellationRequested();
    }

    /// <inheritdoc/>
    protected override async Task OnClaimAsync(int timeout, CancellationToken ct)
    {
        await Task.Delay(100, ct);
        ct.ThrowIfCancellationRequested();
    }
}
