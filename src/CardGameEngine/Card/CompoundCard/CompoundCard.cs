using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGameEngine;

[Serializable]
public abstract class CompoundCard : Card, ICompoundCard
{
    protected new List<ICard> Components;

    public override IPlayer Owner
    {
        get => ((Card)Components[0]).Owner;
        set
        {
            Components.ForEach(c => ((Card)c).Owner = value);
        }
    }

/// <summary>Initializes a new instance of the <see cref="CompoundCard"/> type.</summary>
/// <param name="components">The components value.</param>
/// <param name="Name">The Name value.</param>
    public CompoundCard(List<ICard> components, string Name)
        : base([.. components.SelectMany(x => x.Components)], [.. components.SelectMany(x => x.Reactions)], ((Card)components[0]).Owner, Name)
    {
        this.Components = components;
    }

/// <summary>Initializes a new instance of the <see cref="CompoundCard"/> type.</summary>
/// <param name="this([card]">The this([card] value.</param>
/// <param name="((Card)card).Name">The ((Card)card).Name value.</param>
    public CompoundCard(ICard card) : this([card], ((Card)card).Name)
    {
    }

/// <summary>Adds a component to this object.</summary>
/// <param name="card">The card value.</param>
    public virtual void AddComponent(ICard card)
    {
        if(card is CompoundCard)
        {
            ((CompoundCard)card).Components.ForEach(c => AddComponent(c));
        }
        else
        {
            ((Card)card).Owner = Owner;
            card.Reactions.ForEach(r => Reactions.Add(r));
            Components.Add(card);
        }
    }

/// <summary>Removes a component from this object.</summary>
/// <param name="card">The card value.</param>
    public void RemoveComponent(ICard card)
    {
        if (card is CompoundCard)
        {
            ((CompoundCard)card).Components.ForEach(c => RemoveComponent(c));
        }
        else
        {
            card.Reactions.ForEach(r => Reactions.Remove(r));
            Components.Remove(card);
        }
    }
}
