using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class Board : CardCollection, IBoard
{
    /// <summary>
    /// Data container.
    /// </summary>
    [JsonProperty]
    protected ICard[] cards;

    [JsonProperty]
    protected const int MaximumCapacity = 6;

    /// <summary>
    /// Represents all Cards on a Player's Board.
    /// </summary>
    public Board() : this(new ICard[MaximumCapacity])
    {
        for (int i = 0; i < cards.Length; ++i)
        {
            cards[i] = null;
        }
    }

/// <summary>Initializes a new instance of the <see cref="Board"/> type.</summary>
/// <param name="cards">The cards value.</param>
    [JsonConstructor]
    protected Board(ICard[] cards)
    {
        this.cards = cards;
    }

    [JsonIgnore]
    public int MaxSize { get => MaximumCapacity; }

    [JsonIgnore]
    public override List<ICard> AllCards
    {
        get
        {
            List<ICard> allCards = [];
            foreach (ICard card in cards)
            {
                if (card != null)
                {
                    allCards.Add(card);
                }
            }
            return allCards;
        }
    }

    [JsonIgnore]
    public override bool IsEmpty
    {
        get
        {
            foreach (ICard card in cards)
            {
                if (card != null)
                {
                    return false;
                }
            }
            return true;
        }
    }

    [JsonIgnore]
    public override int Size
    {
        get
        {
            int size = 0;
            foreach (ICard card in cards)
            {
                if (card != null)
                {
                    ++size;
                }
            }
            return size;
        }
    }

    public ICard this[int index]
    {
        get => cards[index];
    }

/// <summary>Performs the Contains operation.</summary>
/// <param name="card">The card value.</param>
/// <returns>The result of the operation.</returns>
    public override bool Contains(ICard card)
    {
        foreach (ICard c in cards)
        {
            if (c == card)
            {
                return true;
            }
        }
        return false;
    }

/// <summary>Performs the AddAt operation.</summary>
/// <param name="index">The index value.</param>
/// <param name="card">The card value.</param>
    public void AddAt(int index, ICard card)
    {
        if(!IsFreeSlot(index))
        {
            throw new CardGameEngineException("Cannot add card to board, because " +
                "position " + index + " is already occupied!");
        }
        cards[index] = card;
    }

/// <summary>Performs the Remove operation.</summary>
/// <param name="card">The card value.</param>
    public void Remove(ICard card)
    {
        for(int i=0; i<cards.Length; ++i)
        {
            if(cards[i] == card)
            {
                cards[i] = null;
            }
        }
    }

/// <summary>Performs the IsFreeSlot operation.</summary>
/// <param name="index">The index value.</param>
/// <returns>The result of the operation.</returns>
    public bool IsFreeSlot(int index)
    {
        return cards[index] == null;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        ICard[] cardsClone = new ICard[cards.Length];
        for (int i = 0; i < cards.Length; ++i)
            cardsClone[i] = cards[i] == null ? default! : (ICard)cards[i].Clone();
        return new Board(cardsClone);
    }
}
