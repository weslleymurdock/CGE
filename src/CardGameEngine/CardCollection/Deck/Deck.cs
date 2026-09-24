using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class Deck : CardCollection, IDeck
{
    [JsonProperty]
    protected Stack<ICard> cards;

/// <summary>Initializes a new instance of the <see cref="Deck"/> type.</summary>
    public Deck() : this(new Stack<ICard>())
    {
    }

/// <summary>Initializes a new instance of the <see cref="Deck"/> type.</summary>
/// <param name="cards">The cards value.</param>
    [JsonConstructor]
    protected Deck(Stack<ICard> cards)
    {
        this.cards = cards;
    }

    [JsonIgnore]
    public override List<ICard> AllCards => [.. cards];

    [JsonIgnore]
    public override int Size => cards.Count;

    [JsonIgnore]
    public override bool IsEmpty
    {
        get => cards.Count == 0;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        Stack<ICard> cardsClone = new();
        foreach (ICard card in cards.Reverse())
        {
            cardsClone.Push((ICard)card.Clone());
        }
        return new Deck(cardsClone);
    }

/// <summary>Performs the Contains operation.</summary>
/// <param name="card">The card value.</param>
/// <returns>The result of the operation.</returns>
    public override bool Contains(ICard card)
    {
        return cards.Contains(card);
    }
    

/// <summary>Performs the Pop operation.</summary>
/// <returns>The result of the operation.</returns>
    public ICard Pop()
    {
        return cards.Pop();
    }

/// <summary>Performs the Push operation.</summary>
/// <param name="card">The card value.</param>
    public void Push(ICard card)
    {
        cards.Push(card);
    }

/// <summary>Performs the Shuffle operation.</summary>
    public void Shuffle()
    {
        ICard[] tmp = [.. cards];
        cards.Clear();
        foreach (ICard card in tmp.OrderBy(x => new Random().Next()))
        {
            cards.Push(card);
        }
    }
}
