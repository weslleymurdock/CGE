using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class Hand : CardCollection, IHand
{
    /// <summary>
    /// Data container.
    /// </summary>
    [JsonProperty]
    protected List<ICard> cards;

/// <summary>Initializes a new instance of the <see cref="Hand"/> type.</summary>
    public Hand() : this([])
    {
    }

    [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="Hand"/> type.</summary>
/// <param name="cards">The cards value.</param>
    protected Hand(List<ICard> cards)
    {
        this.cards = cards;
    }

    [JsonIgnore]
    public int MaxSize { get => 10; }

    [JsonIgnore]
    public override List<ICard> AllCards => [.. cards];

    [JsonIgnore]
    public override bool IsEmpty
    {
        get => cards.Count == 0;
    }

    [JsonIgnore]
    public override int Size => cards.Count;

/// <summary>Performs the Contains operation.</summary>
/// <param name="card">The card value.</param>
/// <returns>The result of the operation.</returns>
    public override bool Contains(ICard card)
    {
        return cards.Contains(card);
    }

/// <summary>Performs the Add operation.</summary>
/// <param name="card">The card value.</param>
    public void Add(ICard card)
    {
        if(cards.Count < MaxSize)
        {
            cards.Add(card);
        }
    }

/// <summary>Performs the Remove operation.</summary>
/// <param name="card">The card value.</param>
    public void Remove(ICard card)
    {
        cards.Remove(card);
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        List<ICard> cardsClone = [];
        foreach (ICard card in cards)
        {
            cardsClone.Add((ICard)card.Clone());
        }
        return new Hand(cardsClone);
    }

    public ICard this[int index]
    {
        get => cards[index];
    }
}
