using R3;

namespace PosSharp.Abstractions;

/// <summary>Defines an interface for a component that produces UPOS events.</summary>
public interface IUposEventSink
{
    /// <summary>Gets a stream of DataEvents.</summary>
    Observable<UposDataEventArgs> DataEvents { get; }

    /// <summary>Gets a stream of ErrorEvents.</summary>
    Observable<UposErrorEventArgs> ErrorEvents { get; }

    /// <summary>Gets a stream of StatusUpdateEvents.</summary>
    Observable<UposStatusUpdateEventArgs> StatusUpdateEvents { get; }

    /// <summary>Gets a stream of DirectIOEvents.</summary>
    Observable<UposDirectIoEventArgs> DirectIoEvents { get; }

    /// <summary>Gets a stream of OutputCompleteEvents.</summary>
    Observable<UposOutputCompleteEventArgs> OutputCompleteEvents { get; }
}
