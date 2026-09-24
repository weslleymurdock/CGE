using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class DrawCardAction : Action
{
    [JsonProperty]
    public IPlayer Player;

    [JsonProperty]
    public ICard DrawnCard;

    [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="DrawCardAction"/> type.</summary>
/// <param name="player">The player value.</param>
/// <param name="isAborted">The isAborted value.</param>
    public DrawCardAction(IPlayer player, bool isAborted = false)
        : base(isAborted)
    {
        Player = player;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new DrawCardAction(
            null, // otherwise circular dependencies
            IsAborted
        );
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        RemoveCardFromDeckAction removeAction = new(Player.Deck);
        game.Execute(removeAction);
        DrawnCard = removeAction.Card;
        game.Execute(new AddCardToHandAction(Player.Hand, DrawnCard));
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        // Drawing is a composite action: every subsequent action must be
        // executable before the card is removed from the deck.
        return !Player.Deck.IsEmpty
            && Player.Hand.Size < Player.Hand.MaxSize;
    }
}
