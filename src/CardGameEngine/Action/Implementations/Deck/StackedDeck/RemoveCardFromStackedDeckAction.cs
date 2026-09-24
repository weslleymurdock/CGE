using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class RemoveCardFromStackedDeckAction : Action
{
    [JsonProperty]
    public ICard Card;

    [JsonProperty]
    public readonly IStackedDeck deck;

    [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="RemoveCardFromStackedDeckAction"/> type.</summary>
/// <param name="deck">The deck value.</param>
/// <param name="card">The card value.</param>
/// <param name="isAborted">The isAborted value.</param>
    public RemoveCardFromStackedDeckAction(IStackedDeck deck, ICard card, bool isAborted = false)
    {
        this.deck = deck;
        this.Card = card;
        IsAborted = isAborted;
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        Card = deck.Pop();
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        return !deck.IsEmpty;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new RemoveCardFromStackedDeckAction(
            (IStackedDeck)deck.Clone(),
            (ICard)Card.Clone(),
            IsAborted
        );
    }
}
