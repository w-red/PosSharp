namespace PosSharp.Core.Tests;

/// <summary>
/// A version of <see cref="StubUposDevice"/> that supports cancellation in its hooks.
/// Used to verify that asynchronous operations correctly propagate and respect cancellation tokens.
/// </summary>
internal sealed class CancellableStubUposDevice : StubUposDevice
{
    /// <summary>Simulates a cancellable open operation.</summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    protected override async Task OnOpenAsync(CancellationToken ct)
    {
        await Task.Delay(100, ct);
        ct.ThrowIfCancellationRequested();
    }

    /// <summary>Simulates a cancellable claim operation.</summary>
    /// <param name="timeout">The timeout value.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    protected override async Task OnClaimAsync(int timeout, CancellationToken ct)
    {
        await Task.Delay(100, ct);
        ct.ThrowIfCancellationRequested();
    }
}
