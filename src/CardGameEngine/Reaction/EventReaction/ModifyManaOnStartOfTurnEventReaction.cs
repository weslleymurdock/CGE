using System;

namespace CardGameEngine
{
    [Serializable]
    public class ModifyManaOnStartOfTurnEventReaction : Reaction
    {
/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override object Clone()
        {
            return new ModifyManaOnStartOfTurnEventReaction();
        }

/// <summary>Reacts to the specified action event.</summary>
/// <param name="game">The game value.</param>
/// <param name="actionEvent">The actionEvent value.</param>
        public override void ReactTo(IGame game, IActionEvent actionEvent)
        {
            if (actionEvent.IsAfter(typeof(StartOfTurnEvent)))
            {
                int manaDelta = game.ActivePlayer.ManaBaseValue + 1 - game.ActivePlayer.ManaValue;
                game.Execute(new ModifyManaStatAction(game.ActivePlayer, manaDelta, 1));
            }
        }
    }
}
