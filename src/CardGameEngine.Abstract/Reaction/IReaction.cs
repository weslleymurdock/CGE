namespace CardGameEngine;

public interface IReaction
{
    /// <summary>
    /// React before a given IAction.
    /// </summary>
    /// <param name="game"></param>
    /// <param name="action"></param>
    void ReactBefore(IEngine game, IAction action);

    /// <summary>
    /// React after a given IAction.
    /// </summary>
    /// <param name="game"></param>
    /// <param name="action"></param>
    void ReactAfter(IEngine game, IAction action);
}

public interface IReaction<T, TGame, TAction> : IReaction
    where T : IEngineState
    where TGame : IEngine<T>
    where TAction : IAction<T>
{
    /// <summary>
    /// React before a given IAction.
    /// </summary>
    /// <param name="game"></param>
    /// <param name="action"></param>
    void ReactBefore(TGame game, TAction action);

    /// <summary>
    /// React after a given IAction.
    /// </summary>
    /// <param name="game"></param>
    /// <param name="action"></param>
    void ReactAfter(TGame game, TAction action);
}
