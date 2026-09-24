using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public abstract class TargetlessSpellCardComponent : CardComponent, ITargetlessSpellCardComponent
{
/// <summary>Initializes a new instance of the <see cref="TargetlessSpellCardComponent"/> type.</summary>
    public TargetlessSpellCardComponent(int mana) : base(mana)
    {
    }

    [JsonConstructor]
    protected TargetlessSpellCardComponent(ManaCostStat manaCostStat,
        List<IReaction> reactions)
        : base(manaCostStat, reactions)
    {
    }

/// <summary>Casts this spell using the specified game or target.</summary>
/// <param name="game">The game value.</param>
    public abstract void Cast(IGame game);
}
