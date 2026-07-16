using System;
using System.Collections.Generic;

namespace CardGameEngine;

[Serializable]
public class CompoundTargetlessSpellCard : CompoundCard, ITargetlessSpellCard
{
    public CompoundTargetlessSpellCard(List<ITargetlessSpellCard> components, string name)
        : base(new List<ICard>(), name)
    {
        components.ForEach(c => Components.Add(c));
    }

    public CompoundTargetlessSpellCard(ITargetlessSpellCard spellCard, string name)
        : this(new List<ITargetlessSpellCard> { spellCard }, name)
    {
    }
    public override object Clone()
    {
        return new CompoundTargetlessSpellCard(Components.ConvertAll(c => (ITargetlessSpellCard)c.Clone()), Name);
    }
    public void Cast(IGame game)
    {
        Components.ForEach(c => ((ITargetlessSpellCard)c).Cast(game));
    }
}
