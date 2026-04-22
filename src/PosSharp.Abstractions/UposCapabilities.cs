using System.Collections.Frozen;

namespace PosSharp.Abstractions;

/// <summary>
/// Represents a set of frozen device capabilities for high-performance lookup.
/// Used to store static device information that does not change after initialization.
/// </summary>
public sealed class UposCapabilities
{
    private readonly FrozenDictionary<string, object> storage;

    /// <summary>Gets an empty set of capabilities.</summary>
    public static UposCapabilities Empty { get; } = new(new Dictionary<string, object>());

    /// <summary>Initializes a new instance of the <see cref="UposCapabilities"/> class.</summary>
    /// <param name="capabilities">The dictionary of capabilities to freeze.</param>
    public UposCapabilities(IDictionary<string, object> capabilities)
    {
        storage = capabilities.ToFrozenDictionary();
    }

    /// <summary>Gets a capability value by its key.</summary>
    /// <typeparam name="T">The type of the capability value.</typeparam>
    /// <param name="key">The key of the capability.</param>
    /// <param name="defaultValue">The value to return if the key is not found.</param>
    /// <returns>The capability value or the default value.</returns>
    public T Get<T>(string key, T defaultValue = default!)
    {
        return storage.TryGetValue(key, out var value) ? (T)value : defaultValue;
    }
}
