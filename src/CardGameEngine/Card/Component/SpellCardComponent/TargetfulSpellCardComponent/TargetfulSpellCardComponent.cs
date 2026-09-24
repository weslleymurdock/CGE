using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public abstract class TargetfulSpellCardComponent : CardComponent, ITargetfulSpellCardComponent
{
/// <summary>Initializes a new instance of the <see cref="TargetfulSpellCardComponent"/> type.</summary>
    public TargetfulSpellCardComponent(int mana) : base(mana)
    {
    }

    [JsonConstructor]
    protected TargetfulSpellCardComponent(ManaCostStat manaCostStat,
        List<IReaction> reactions)
        : base(manaCostStat, reactions)
    {
    }

/// <summary>Casts this spell using the specified game or target.</summary>
/// <param name="game">The game value.</param>
/// <param name="target">The target value.</param>
    public abstract void Cast(IGame game, ICharacter target);

/// <summary>Gets the characters that can currently be targeted.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public abstract HashSet<ICharacter> GetPotentialTargets(IGameState gameState);
}
