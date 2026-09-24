using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class TargetfulSpellCard : SpellCard, ITargetfulSpellCard
{
    public override IPlayer Owner { get; set; }

/// <summary>Initializes a new instance of the <see cref="TargetfulSpellCard"/> type.</summary>
/// <param name="owner">The owner value.</param>
/// <param name="name">The name value.</param>
    public TargetfulSpellCard(IPlayer owner = default!, string name = "")
        : this([], owner, name)
    {
    }

/// <summary>Initializes a new instance of the <see cref="TargetfulSpellCard"/> type.</summary>
/// <param name="component">The component value.</param>
/// <param name="owner">The owner value.</param>
/// <param name="name">The name value.</param>
    public TargetfulSpellCard(ISpellCardComponent component, IPlayer owner, string name)
        : this([component], owner, name)
    { 
    }

/// <summary>Initializes a new instance of the <see cref="TargetfulSpellCard"/> type.</summary>
/// <param name="components">The components value.</param>
/// <param name="owner">The owner value.</param>
/// <param name="name">The name value.</param>
    public TargetfulSpellCard(List<ISpellCardComponent> components, IPlayer owner, string name)
        : this(components.ConvertAll(c => (ICardComponent)c), [], owner, name)
    {
    }

/// <summary>Initializes a new instance of the <see cref="TargetfulSpellCard"/> type.</summary>
/// <param name="components">The components value.</param>
/// <param name="reactions">The reactions value.</param>
/// <param name="owner">The owner value.</param>
/// <param name="name">The name value.</param>
    [JsonConstructor]
    public TargetfulSpellCard(List<ICardComponent> components, List<IReaction> reactions, IPlayer owner, string name)
        : base(components, reactions, owner, name)
    {
        Owner = owner;
    }

/// <summary>Gets the characters that can currently be targeted.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
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
        return potentialTargets ?? [];
    }

/// <summary>Casts this spell using the specified game or target.</summary>
/// <param name="game">The game value.</param>
/// <param name="target">The target value.</param>
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

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        List<ICardComponent> componentsClone = [];
        Components.ForEach(c => componentsClone.Add((ICardComponent)c.Clone()));

        List<IReaction> reactionsClone = [];
        Reactions.ForEach(r => reactionsClone.Add((IReaction)r.Clone()));

        return new TargetfulSpellCard(
            componentsClone,
            reactionsClone,
            Owner,
            Name
        );
    }
}
