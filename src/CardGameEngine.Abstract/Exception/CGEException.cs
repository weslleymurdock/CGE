namespace CardGameEngine;

public class CGEException : Exception
{
    protected const string MessagePrefix = "C# Battle Card Game Framework EXCEPTION: ";

    /// <summary>
    /// Library specific Exception. Usually thrown when the Game's
    /// mechanics are being violated.
    /// </summary>
    public CGEException()
    {
    }

    /// <summary>
    /// Library specific Exception. Usually thrown when the Game's
    /// mechanics are being violated.
    /// </summary>
    /// <param name="message"></param>
    public CGEException(string message)
        : base(MessagePrefix + message)
    {
    }

    /// <summary>
    /// Library specific Exception. Usually thrown when the Game's
    /// mechanics are being violated.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public CGEException(string message, Exception inner)
        : base(MessagePrefix + message, inner)
    {
    }
}
