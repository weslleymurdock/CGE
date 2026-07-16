namespace CardGameEngine;

public interface ISpellCard : ICard
{
    bool IsCastable(IGameState gameState);
}
