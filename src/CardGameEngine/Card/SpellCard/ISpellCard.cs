namespace CardGameEngine;

public interface ISpellCard : ICard
{
    /// <summary>Determines whether the card can currently be cast for the specified game state.</summary>
    bool IsCastable(IGameState gameState);
}
