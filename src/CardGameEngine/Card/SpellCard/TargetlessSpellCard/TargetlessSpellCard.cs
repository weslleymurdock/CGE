using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class TargetlessSpellCard : SpellCard, ITargetlessSpellCard
{
    public TargetlessSpellCard(IPlayer owner = default!, string name = "")
        : this([], owner, name)
    {
    }

    public TargetlessSpellCard(ITargetlessSpellCardComponent component, IPlayer owner, string name)
        : this([component], owner, name)
    {
    }

    public TargetlessSpellCard(List<ITargetlessSpellCardComponent> components, IPlayer owner, string name)
        : this(components.ConvertAll(c => (ICardComponent)c), [], owner, name)
    {
    }

    [JsonConstructor]
    public TargetlessSpellCard(List<ICardComponent> components, List<IReaction> reactions, IPlayer owner, string name)
        : base(components, reactions, owner, name)
    {
        Owner = owner;
    }

    public override IPlayer Owner { get; set; }

    public void Cast(IGame game)
    {
        foreach (ICardComponent component in Components)
        {
            if (component is ITargetlessSpellCardComponent targetlessComponent)
            {
                targetlessComponent.Cast(game);
            }
        }
    }

    public override object Clone()
    {
        List<ICardComponent> componentsClone = [];
        Components.ForEach(c => componentsClone.Add((ICardComponent)c.Clone()));

        List<IReaction> reactionsClone = [];
        Reactions.ForEach(r => reactionsClone.Add((IReaction)r.Clone()));

        return new TargetlessSpellCard(
            componentsClone,
            reactionsClone,
            Owner,
            Name
        );
    }
}
