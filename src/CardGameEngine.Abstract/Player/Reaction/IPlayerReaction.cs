namespace CardGameEngine;

public interface IPlayerReaction<T, TGame, TAction> : IReaction<T, TGame, TAction>
    where T : IEngineState
    where TGame : IEngine<T>
    where TAction : IAction<T>
{
    /// <summary>
    /// Returns the parent IPlayer of this IReaction.
    /// </summary>
    IPlayer ParentPlayer { get; }
}
