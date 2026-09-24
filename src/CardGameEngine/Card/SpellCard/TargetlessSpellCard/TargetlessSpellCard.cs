using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class TargetlessSpellCard : SpellCard, ITargetlessSpellCard
{
/// <summary>Initializes a new instance of the <see cref="TargetlessSpellCard"/> type.</summary>
/// <param name="owner">The owner value.</param>
/// <param name="name">The name value.</param>
    public TargetlessSpellCard(IPlayer owner = default!, string name = "")
        : this([], owner, name)
    {
    }

/// <summary>Initializes a new instance of the <see cref="TargetlessSpellCard"/> type.</summary>
/// <param name="component">The component value.</param>
/// <param name="owner">The owner value.</param>
/// <param name="name">The name value.</param>
    public TargetlessSpellCard(ITargetlessSpellCardComponent component, IPlayer owner, string name)
        : this([component], owner, name)
    {
    }

/// <summary>Initializes a new instance of the <see cref="TargetlessSpellCard"/> type.</summary>
/// <param name="components">The components value.</param>
/// <param name="owner">The owner value.</param>
/// <param name="name">The name value.</param>
    public TargetlessSpellCard(List<ITargetlessSpellCardComponent> components, IPlayer owner, string name)
        : this(components.ConvertAll(c => (ICardComponent)c), [], owner, name)
    {
    }

/// <summary>Initializes a new instance of the <see cref="TargetlessSpellCard"/> type.</summary>
/// <param name="components">The components value.</param>
/// <param name="reactions">The reactions value.</param>
/// <param name="owner">The owner value.</param>
/// <param name="name">The name value.</param>
    [JsonConstructor]
    public TargetlessSpellCard(List<ICardComponent> components, List<IReaction> reactions, IPlayer owner, string name)
        : base(components, reactions, owner, name)
    {
        Owner = owner;
    }

    public override IPlayer Owner { get; set; }

/// <summary>Casts this spell using the specified game or target.</summary>
/// <param name="game">The game value.</param>
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

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
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
