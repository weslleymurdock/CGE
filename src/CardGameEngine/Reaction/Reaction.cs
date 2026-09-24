namespace CardGameEngine
{
    public abstract class Reaction : IReaction
    {
/// <summary>Finds the parent card in the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
        public ICard FindParentCard(IGameState gameState)
        {
            foreach (ICard card in gameState.AllCards)
            {
                if (card.Reactions.Contains(this))
                {
                    return card;
                }
            }
            throw new CardGameEngineException("Card not found for the given component.");
        }

/// <summary>Finds the parent player in the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
        public IPlayer FindParentPlayer(IGameState gameState)
        {
            foreach (IPlayer player in gameState.Players)
            {
                if (player.Reactions.Contains(this))
                {
                    return player;
                }
            }
            throw new CardGameEngineException("Player not found for the given component.");
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public abstract object Clone();

/// <summary>Reacts to the specified action event.</summary>
/// <param name="game">The game value.</param>
/// <param name="actionEvent">The actionEvent value.</param>
        public abstract void ReactTo(IGame game, IActionEvent actionEvent);
    }
}
