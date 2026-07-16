using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class TargetfulSpellCard : SpellCard, ITargetfulSpellCard
{
    public override IPlayer Owner { get; set; }

    public TargetfulSpellCard(IPlayer owner = default!, string name = "")
        : this(new List<ISpellCardComponent>(), owner, name)
    {
    }

    public TargetfulSpellCard(ISpellCardComponent component, IPlayer owner, string name)
        : this(new List<ISpellCardComponent> { component }, owner, name)
    { 
    }

    public TargetfulSpellCard(List<ISpellCardComponent> components, IPlayer owner, string name)
        : this(components.ConvertAll(c => (ICardComponent)c), new List<IReaction>(), owner, name)
    {
    }

    [JsonConstructor]
    public TargetfulSpellCard(List<ICardComponent> components, List<IReaction> reactions, IPlayer owner, string name)
        : base(components, reactions, owner, name)
    {
        Owner = owner;
    }

    public HashSet<ICharacter> GetPotentialTargets(IGameState gameState)
    {
        //Compute the intersection of all potential targets
        HashSet<ICharacter> potentialTargets = default!;
        foreach (ICardComponent component in Components.FindAll(c => c is ITargetful))
        {
            if (potentialTargets == null)
            {
                potentialTargets = ((ITargetful)component).GetPotentialTargets(gameState);
            }
            else
            {
                potentialTargets.IntersectWith(((ITargetful)component).GetPotentialTargets(gameState));
            }
        }
        return potentialTargets ?? new HashSet<ICharacter>();
    }

    public void Cast(IGame game, ICharacter target)
    {
        if (!GetPotentialTargets(game).Contains(target))
        {
            throw new CardGameEngineException("Tried to play a TargetfulSpellCard " +
                "on an invalid target character!");
        }

        foreach (ICardComponent component in Components)
        {
            if (component is ITargetlessSpellCardComponent targetlessComponent)
            {
                targetlessComponent.Cast(game);
            }
            else if (component is ITargetfulSpellCardComponent targetfulComponent)
            {
                targetfulComponent.Cast(game, target);
            }
        }
    }

    public override object Clone()
    {
        List<ICardComponent> componentsClone = new List<ICardComponent>();
        Components.ForEach(c => componentsClone.Add((ICardComponent)c.Clone()));

        List<IReaction> reactionsClone = new List<IReaction>();
        Reactions.ForEach(r => reactionsClone.Add((IReaction)r.Clone()));

        return new TargetfulSpellCard(
            componentsClone,
            reactionsClone,
            Owner,
            Name
        );
    }
}
