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
    public RemoveCardFromStackedDeckAction(IStackedDeck deck, ICard card, bool isAborted = false)
    {
        this.deck = deck;
        this.Card = card;
        IsAborted = isAborted;
    }

    public override void Execute(IGame game)
    {
        Card = deck.Pop();
    }

    public override bool IsExecutable(IGameState gameState)
    {
        return !deck.IsEmpty;
    }

    public override object Clone()
    {
        return new RemoveCardFromStackedDeckAction(
            (IStackedDeck)deck.Clone(),
            (ICard)Card.Clone(),
            IsAborted
        );
    }
}
