using System;
namespace CardGameEngine;

[Serializable]
public class CardGameEngineException : Exception
{
    protected const string MessagePrefix = "Card Game Engine Error: ";

    /// <summary>
    /// Library specific Exception. Usually thrown when the Game's
    /// mechanics are being violated.
    /// </summary>
    public CardGameEngineException()
    {
    }

    /// <summary>
    /// Library specific Exception. Usually thrown when the Game's
    /// mechanics are being violated.
    /// </summary>
    /// <param name="message"></param>
    public CardGameEngineException(string message)
        : base(MessagePrefix + message)
    {
    }

    /// <summary>
    /// Library specific Exception. Usually thrown when the Game's
    /// mechanics are being violated.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public CardGameEngineException(string message, Exception inner)
        : base(MessagePrefix + message, inner)
    {
    }
}
