namespace CardGameEngine
{
    public abstract class Reaction : IReaction
    {
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

        public abstract object Clone();

        public abstract void ReactTo(IGame game, IActionEvent actionEvent);
    }
}
