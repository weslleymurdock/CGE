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

    public CompoundCard(List<ICard> components, string Name)
        : base([.. components.SelectMany(x => x.Components)], [.. components.SelectMany(x => x.Reactions)], ((Card)components[0]).Owner, Name)
    {
        this.Components = components;
    }

    public CompoundCard(ICard card) : this(new List<ICard> { card }, ((Card)card).Name)
    {
    }

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
