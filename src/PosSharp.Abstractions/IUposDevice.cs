// Copyright (c) PosSharp Project. All rights reserved.
// Licensed under the MIT License.

using R3;

namespace PosSharp.Abstractions;

/// <summary>
/// Basic interface for all UPOS devices, providing common lifecycle and state management.
/// </summary>
public interface IUposDevice : IDisposable
{
    /// <summary>Gets the current logical state of the device.</summary>
    ReadOnlyReactiveProperty<ControlState> State { get; }

    /// <summary>Gets a value indicating whether the device is currently processing an operation.</summary>
    ReadOnlyReactiveProperty<bool> IsBusy { get; }

    /// <summary>Gets the result code of the last completed operation.</summary>
    ReadOnlyReactiveProperty<UposErrorCode> LastError { get; }

    /// <summary>Attempts to open the device for communication.</summary>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OpenAsync(CancellationToken ct = default);

    /// <summary>Closes communication with the device.</summary>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CloseAsync(CancellationToken ct = default);

    /// <summary>Claims exclusive access to the device.</summary>
    /// <param name="timeout">Timeout in milliseconds.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ClaimAsync(int timeout, CancellationToken ct = default);

    /// <summary>Releases exclusive access to the device.</summary>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ReleaseAsync(CancellationToken ct = default);

    /// <summary>Enables or disables the device.</summary>
    /// <param name="enabled"><see langword="true"/> to enable; <see langword="false"/> to disable.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetEnabledAsync(bool enabled, CancellationToken ct = default);
}
