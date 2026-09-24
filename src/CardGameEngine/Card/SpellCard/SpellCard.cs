using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public abstract class SpellCard : Card, ISpellCard
{
/// <summary>Initializes a new instance of the <see cref="SpellCard"/> type.</summary>
/// <param name="owner">The owner value.</param>
/// <param name="name">The name value.</param>
    public SpellCard(IPlayer owner = default!, string name = "")
        : this([], owner, name)
    {
    }

    /// <summary>
    /// Represents a certain type of Card that has an
    /// immediate effect on the Game's state.
    /// </summary>
    /// <param name="components"></param>
    /// <param name="owner"></param>-
    /// <param name="name"></param>-
    public SpellCard(List<ISpellCardComponent> components, IPlayer owner, string name)
        : this(components.ConvertAll(c => (ICardComponent)c), [], owner, name)
    {
    }

/// <summary>Initializes a new instance of the <see cref="SpellCard"/> type.</summary>
/// <param name="components">The components value.</param>
/// <param name="reactions">The reactions value.</param>
/// <param name="owner">The owner value.</param>
/// <param name="name">The name value.</param>
    [JsonConstructor]
    public SpellCard(List<ICardComponent> components, List<IReaction> reactions, IPlayer owner, string name)
        : base(components, reactions, owner, name)
    {
    }
}
