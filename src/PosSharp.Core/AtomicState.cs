namespace PosSharp.Core;

/// <summary>
/// Compare-And-Swap (CAS) ベースのアトミックな状態管理を提供します。
/// <para>
/// immutable な参照型と組み合わせることで、Lock を使用せずに
/// スレッドセーフな状態遷移を実現します。
/// </para>
/// </summary>
/// <typeparam name="TState">状態の型。参照型に限定されます。</typeparam>
public sealed class AtomicState<TState> where TState : class
{
    private volatile TState current;

    /// <summary>
    /// <see cref="AtomicState{TState}"/> の新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="initial">初期状態。</param>
    /// <exception cref="ArgumentNullException"><paramref name="initial"/> が null の場合。</exception>
    public AtomicState(TState initial)
    {
        ArgumentNullException.ThrowIfNull(initial);
        current = initial;
    }

    /// <summary>現在の状態を取得します。</summary>
    public TState Current => current;

    /// <summary>
    /// アトミックに状態を遷移させます。
    /// <para>
    /// <paramref name="transform"/> は CAS の競合時に複数回呼び出される可能性があるため、
    /// 副作用のない純粋関数である必要があります。
    /// </para>
    /// </summary>
    /// <param name="transform">
    /// 現在の状態を受け取り、新しい状態を返す変換関数。
    /// 同一参照を返した場合、遷移は行われません。
    /// </param>
    /// <returns>遷移結果。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="transform"/> が null の場合。</exception>
    public StateTransitionResult<TState> Transition(Func<TState, TState> transform)
    {
        ArgumentNullException.ThrowIfNull(transform);

        TState oldState, newState;
        do
        {
            oldState = current;
            newState = transform(oldState);
            if (ReferenceEquals(oldState, newState))
            {
                return new(oldState, newState, Changed: false);
            }
        } while (Interlocked.CompareExchange(ref current, newState, oldState) != oldState);

        return new(oldState, newState, Changed: true);
    }

    /// <summary>
    /// アトミックに状態を無条件で置換します。
    /// </summary>
    /// <param name="newState">新しい状態。</param>
    /// <returns>置換前の状態。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="newState"/> が null の場合。</exception>
    public TState Exchange(TState newState)
    {
        ArgumentNullException.ThrowIfNull(newState);
        return Interlocked.Exchange(ref current, newState);
    }
}
