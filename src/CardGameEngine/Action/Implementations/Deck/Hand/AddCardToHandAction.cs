using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class AddCardToHandAction : Action
{
    [JsonProperty]
    public readonly IHand Hand;

    [JsonProperty]
    public ICard Card;

    [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="AddCardToHandAction"/> type.</summary>
/// <param name="hand">The hand value.</param>
/// <param name="card">The card value.</param>
/// <param name="isAborted">The isAborted value.</param>
    public AddCardToHandAction(IHand hand, ICard card, bool isAborted = false)
        : base(isAborted)
    {
        Hand = hand;
        Card = card;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new AddCardToHandAction(
            (IHand)Hand.Clone(),
            (ICard)Card.Clone(),
            IsAborted
        );
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        Hand.Add(Card);
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        return Card != null && Hand.Size < Hand.MaxSize;
    }
}
