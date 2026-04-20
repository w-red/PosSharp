// Copyright (c) PosSharp Project. All rights reserved.
// Licensed under the MIT License.

namespace PosSharp.Abstractions;

/// <summary>
/// Represents the result of a UPOS command execution.
/// </summary>
/// <param name="ResultCode">The standard UPOS result code.</param>
/// <param name="ExtendedCode">The device-specific extended result code.</param>
public sealed record UposCommandResult(UposErrorCode ResultCode, int ExtendedCode = 0)
{
    /// <summary>Gets a successful command result.</summary>
    public static UposCommandResult Ok { get; } = new(UposErrorCode.Success);

    /// <summary>Gets a value indicating whether the command was successful.</summary>
    public bool IsSuccess => this.ResultCode == UposErrorCode.Success;
}
