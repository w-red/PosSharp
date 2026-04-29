namespace PosSharp.Core;

/// <summary>
/// スレッドセーフな状態遷移の結果を表します。
/// </summary>
/// <typeparam name="TState">状態の型。参照型に限定されます。</typeparam>
/// <param name="OldState">遷移前の状態。</param>
/// <param name="NewState">遷移後の状態。</param>
/// <param name="Changed">状態が実際に変更されたかどうか。</param>
public readonly record struct StateTransitionResult<TState>(
    TState OldState,
    TState NewState,
    bool Changed) where TState : class;
