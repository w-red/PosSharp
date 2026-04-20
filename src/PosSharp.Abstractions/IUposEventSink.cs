// Copyright (c) PosSharp Project. All rights reserved.
// Licensed under the MIT License.

namespace PosSharp.Abstractions;

/// <summary>
/// Defines an interface for a component that produces UPOS events.
/// </summary>
public interface IUposEventSink
{
    /// <summary>Gets a stream of DataEvents.</summary>
    IObservable<UposDataEventArgs> DataEvents { get; }

    /// <summary>Gets a stream of ErrorEvents.</summary>
    IObservable<UposErrorEventArgs> ErrorEvents { get; }

    /// <summary>Gets a stream of StatusUpdateEvents.</summary>
    IObservable<UposStatusUpdateEventArgs> StatusUpdateEvents { get; }

    /// <summary>Gets a stream of DirectIOEvents.</summary>
    IObservable<UposDirectIoEventArgs> DirectIoEvents { get; }

    /// <summary>Gets a stream of OutputCompleteEvents.</summary>
    IObservable<UposOutputCompleteEventArgs> OutputCompleteEvents { get; }
}
