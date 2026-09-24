using System;
using System.Collections.Generic;

namespace CardGameEngine;

[Serializable]
public class CompoundTargetlessSpellCard : CompoundCard, ITargetlessSpellCard
{
    public CompoundTargetlessSpellCard(List<ITargetlessSpellCard> components, string name)
        : base([], name)
    {
        components.ForEach(c => Components.Add(c));
    }

    public CompoundTargetlessSpellCard(ITargetlessSpellCard spellCard, string name)
        : this([spellCard], name)
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
