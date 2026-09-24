using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class CastTargetlessSpellAction : CastSpellAction
{
    [JsonConstructor]
    public CastTargetlessSpellAction(
        IPlayer player,
        ITargetlessSpellCard spellCard,
        bool isAborted = false
        ) : base(player, spellCard, isAborted)
    {
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new CastTargetlessSpellAction(
            null, // otherwise circular dependencies
            (ITargetlessSpellCard)SpellCard.Clone(),
            IsAborted
        );
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        game.Execute(new ModifyManaStatAction(Player, -SpellCard.ManaValue, 0));
        game.Execute(new RemoveCardFromHandAction(Player.Hand, SpellCard));
        ((ITargetlessSpellCard)SpellCard).Cast(game);
        game.Execute(new AddCardToGraveyardAction(Player.Graveyard, SpellCard));
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        return Player == gameState.ActivePlayer
            && Player.Hand.Contains(SpellCard)
            && SpellCard.IsCastable(gameState);
    }
}
