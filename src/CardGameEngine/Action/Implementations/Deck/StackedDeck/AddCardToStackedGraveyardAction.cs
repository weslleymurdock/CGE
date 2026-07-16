using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class AddCardToStackedGraveyardAction : Action
{
    [JsonProperty]
    public readonly IStackedDeck Graveyard;

    [JsonProperty]
    public ICard Card;

    [JsonConstructor]
    public AddCardToStackedGraveyardAction(IStackedDeck graveyard, ICard card, bool isAborted = false)
    {
        Graveyard = graveyard;
        Card = card;
        IsAborted = isAborted;
    }
    public override object Clone()
    {
        return new AddCardToStackedGraveyardAction(
            (IStackedDeck)Graveyard.Clone(),
            (ICard)Card.Clone(),
            IsAborted
        );
    }

    public override void Execute(IGame game)
    {
        Graveyard.Push(Card);
    }

    public override bool IsExecutable(IGameState gameState)
    {
        return Card != null && !Graveyard.Contains(Card);
    }
}
