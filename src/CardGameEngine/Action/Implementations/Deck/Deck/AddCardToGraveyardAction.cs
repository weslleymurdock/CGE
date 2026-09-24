using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class AddCardToGraveyardAction : Action
{
    [JsonProperty]
    public readonly IDeck Graveyard;

    [JsonProperty]
    public ICard Card;

/// <summary>Initializes a new instance of the <see cref="AddCardToGraveyardAction"/> type.</summary>
/// <param name="graveyard">The graveyard value.</param>
/// <param name="card">The card value.</param>
/// <param name="isAborted">The isAborted value.</param>
    [JsonConstructor]
    public AddCardToGraveyardAction(IDeck graveyard, ICard card, bool isAborted = false)
        : base(isAborted)
    {
        Graveyard = graveyard;
        Card = card;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new AddCardToGraveyardAction(
            (IDeck)Graveyard.Clone(),
            (ICard)Card.Clone(),
            IsAborted
        );
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        Graveyard.Push(Card);
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        return Card != null && !Graveyard.Contains(Card);
    }
}
