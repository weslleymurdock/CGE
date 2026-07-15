namespace CardGameEngine;

public interface ICardReaction<T, TGame, TAction> : IReaction<T, TGame, TAction>
    where T : IEngineState
    where TGame : IEngine<T>
    where TAction : IAction<T>
{
    /// <summary>
    /// Returns the parent Card of this IReaction.
    /// </summary>
    ICard ParentCard { get; }
}
