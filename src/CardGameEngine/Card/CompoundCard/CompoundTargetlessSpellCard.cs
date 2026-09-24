using System;
using System.Collections.Generic;

namespace CardGameEngine;

[Serializable]
public class CompoundTargetlessSpellCard : CompoundCard, ITargetlessSpellCard
{
/// <summary>Initializes a new instance of the <see cref="CompoundTargetlessSpellCard"/> type.</summary>
/// <param name="components">The components value.</param>
/// <param name="name">The name value.</param>
    public CompoundTargetlessSpellCard(List<ITargetlessSpellCard> components, string name)
        : base([], name)
    {
        components.ForEach(c => Components.Add(c));
    }

/// <summary>Initializes a new instance of the <see cref="CompoundTargetlessSpellCard"/> type.</summary>
/// <param name="spellCard">The spellCard value.</param>
/// <param name="name">The name value.</param>
    public CompoundTargetlessSpellCard(ITargetlessSpellCard spellCard, string name)
        : this([spellCard], name)
    {
    }
/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new CompoundTargetlessSpellCard(Components.ConvertAll(c => (ITargetlessSpellCard)c.Clone()), Name);
    }
/// <summary>Casts this spell using the specified game or target.</summary>
/// <param name="game">The game value.</param>
    public void Cast(IGame game)
    {
        Components.ForEach(c => ((ITargetlessSpellCard)c).Cast(game));
    }
}
