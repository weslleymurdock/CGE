using System;

namespace CardGameEngine;

[Serializable]
public class NotImplementedYetException : CardGameEngineException
{
    private const string MessagePrefix = "This feature is not yet implemented: ";

/// <summary>Initializes a new instance of the <see cref="NotImplementedYetException"/> type.</summary>
    public NotImplementedYetException()
    {
    }

/// <summary>Initializes a new instance of the <see cref="NotImplementedYetException"/> type.</summary>
/// <param name="message">The message value.</param>
    public NotImplementedYetException(string message) : base(MessagePrefix + message)
    {
    }

/// <summary>Initializes a new instance of the <see cref="NotImplementedYetException"/> type.</summary>
/// <param name="message">The message value.</param>
/// <param name="message">The message value.</param>
/// <param name="innerException">The innerException value.</param>
    public NotImplementedYetException(string message, Exception innerException) : base(MessagePrefix + message, innerException)
    {
    }
}
