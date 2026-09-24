using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class RemoveCardFromDeckAction : Action
{
    [JsonProperty]
    public ICard Card;

    [JsonProperty]
    public readonly IDeck Deck;

    [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="RemoveCardFromDeckAction"/> type.</summary>
/// <param name="deck">The deck value.</param>
/// <param name="card">The card value.</param>
/// <param name="isAborted">The isAborted value.</param>
    public RemoveCardFromDeckAction(IDeck deck, ICard card = null, bool isAborted = false)
        : base(isAborted)
    {
        Deck = deck;
        Card = card;
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        Card = Deck.Pop();
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        return !Deck.IsEmpty;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new RemoveCardFromDeckAction(
            (IDeck)Deck.Clone(),
            (ICard)Card.Clone(),
            IsAborted
        );
    }
}
