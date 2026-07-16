using System;

namespace CardGameEngine;

[Serializable]
public class NotImplementedYetException : CardGameEngineException
{
    private const string MessagePrefix = "This feature is not yet implemented: ";

    public NotImplementedYetException()
    {
    }

    public NotImplementedYetException(string message) : base(MessagePrefix + message)
    {
    }

    public NotImplementedYetException(string message, Exception innerException) : base(MessagePrefix + message, innerException)
    {
    }
}
