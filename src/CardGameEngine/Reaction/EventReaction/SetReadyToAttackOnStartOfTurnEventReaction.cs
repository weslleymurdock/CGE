using System;

namespace CardGameEngine
{
    [Serializable]
    public class SetReadyToAttackOnStartOfTurnEventReaction : Reaction
    {
/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override object Clone()
        {
            return new SetReadyToAttackOnStartOfTurnEventReaction();
        }

/// <summary>Reacts to the specified action event.</summary>
/// <param name="game">The game value.</param>
/// <param name="actionEvent">The actionEvent value.</param>
        public override void ReactTo(IGame game, IActionEvent actionEvent)
        {
            if(actionEvent.IsAfter(typeof(StartOfTurnEvent)))
            {
                IMonsterCard monsterCard = (IMonsterCard)FindParentCard(game);
                IPlayer owner = monsterCard.FindParentPlayer(game);
                bool isReadyToAttack = owner == game.ActivePlayer
                    && game.ActivePlayer.Board.Contains(monsterCard);

                game.Execute(new ModifyReadyToAttackAction(monsterCard, isReadyToAttack));
            }
        }
    }
}
