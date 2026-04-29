using PosSharp.Abstractions;

namespace PosSharp.Core;

/// <summary>
/// UposMediator の内部状態スナップショットを表します。
/// </summary>
/// <param name="State">デバイスの現在の論理状態。</param>
/// <param name="IsBusy">デバイスが操作を実行中かどうか。</param>
/// <param name="LastError">最後に発生したエラーコード。</param>
/// <param name="LastErrorExtended">最後に発生したエラーの詳細コード。</param>
/// <param name="DataCount">キューに入っているデータイベントの数。</param>
public sealed record MediatorSnapshot(
    ControlState State,
    bool IsBusy,
    UposErrorCode LastError,
    int LastErrorExtended,
    int DataCount)
{
    /// <summary>
    /// デフォルトの初期状態を取得します。
    /// </summary>
    public static MediatorSnapshot Initial { get; } = new(
        ControlState.Closed,
        false,
        UposErrorCode.Success,
        0,
        0);
}
