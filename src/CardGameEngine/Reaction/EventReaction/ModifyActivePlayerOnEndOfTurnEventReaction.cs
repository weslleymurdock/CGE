using System;
using System.Linq;

namespace CardGameEngine
{
    [Serializable]
    public class ModifyActivePlayerOnEndOfTurnEventReaction : Reaction
    {
/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override object Clone()
        {
            return new ModifyActivePlayerOnEndOfTurnEventReaction();
        }

/// <summary>Reacts to the specified action event.</summary>
/// <param name="game">The game value.</param>
/// <param name="actionEvent">The actionEvent value.</param>
        public override void ReactTo(IGame game, IActionEvent actionEvent)
        {
            if (actionEvent.IsAfter(typeof(EndOfTurnEvent)))
            {
                int playerIndex = game.Players.ToList().IndexOf(game.ActivePlayer);
                playerIndex = (playerIndex + 1) % game.Players.Count;
                game.Execute(new ModifyActivePlayerAction(game.Players[playerIndex]));
            }
        }
    }
}
