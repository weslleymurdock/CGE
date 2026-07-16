using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public abstract class SpellCard : Card, ISpellCard
{
    public SpellCard(IPlayer owner = default!, string name = "")
        : this(new List<ISpellCardComponent>(), owner, name)
    {
    }

    /// <summary>
    /// Represents a certain type of Card that has an
    /// immediate effect on the Game's state.
    /// </summary>
    /// <param name="components"></param>
    public SpellCard(List<ISpellCardComponent> components, IPlayer owner, string name)
        : this(components.ConvertAll(c => (ICardComponent)c), new List<IReaction>(), owner, name)
    {
    }

    [JsonConstructor]
    public SpellCard(List<ICardComponent> components, List<IReaction> reactions, IPlayer owner, string name)
        : base(components, reactions, owner, name)
    {
    }
}
