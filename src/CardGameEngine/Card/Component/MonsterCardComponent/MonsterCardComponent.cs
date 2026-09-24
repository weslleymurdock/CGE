using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class MonsterCardComponent : CardComponent, IMonsterCardComponent
{
    [JsonProperty]
    protected AttackStat attackStat;

    [JsonProperty]
    protected LifeStat lifeStat;

/// <summary>Initializes a new instance of the <see cref="MonsterCardComponent"/> type.</summary>
/// <param name="mana">The mana value.</param>
/// <param name="attack">The attack value.</param>
/// <param name="life">The life value.</param>
    public MonsterCardComponent(int mana, int attack, int life)
        : this(mana, new AttackStat(attack), new LifeStat(life))
    {
    }

    public MonsterCardComponent(int manaValue, int manaBaseValue,
        int attackValue, int attackBaseValue, int lifeValue, int lifeBaseValue)
        : base(manaValue, manaBaseValue)
    {
        attackStat = new AttackStat(attackValue, attackBaseValue);
        lifeStat = new LifeStat(lifeValue, lifeBaseValue);
    }

/// <summary>Initializes a new instance of the <see cref="MonsterCardComponent"/> type.</summary>
/// <param name="mana">The mana value.</param>
/// <param name="attackStat">The attackStat value.</param>
/// <param name="lifeStat">The lifeStat value.</param>
    public MonsterCardComponent(int mana, AttackStat attackStat, LifeStat lifeStat)
        : base(mana)
    {
        this.attackStat = attackStat;
        this.lifeStat = lifeStat;
    }

    [JsonConstructor]
    protected MonsterCardComponent(
        ManaCostStat manaCostStat,
        AttackStat attackStat,
        LifeStat lifeStat,
        List<IReaction> reactions
        ) : base(manaCostStat, reactions)
    {
        this.attackStat = attackStat;
        this.lifeStat = lifeStat;
    }

    [JsonIgnore]
    public int AttackValue
    {
        get => attackStat.Value;
        set => attackStat.Value = value;
    }

    [JsonIgnore]
    public int AttackBaseValue
    {
        get => attackStat.BaseValue;
        set => attackStat.BaseValue = value;
    }

    [JsonIgnore]
    public int LifeValue
    {
        get => lifeStat.Value;
        set => lifeStat.Value = value;
    }

    [JsonIgnore]
    public int LifeBaseValue
    {
        get => lifeStat.BaseValue;
        set => lifeStat.BaseValue = value;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        List<IReaction> reactionsClone = [];
        foreach (IReaction reaction in Reactions)
        {
            reactionsClone.Add((IReaction)reaction.Clone());
        }

        return new MonsterCardComponent(
            (ManaCostStat)manaCostStat.Clone(),
            (AttackStat)attackStat.Clone(),
            (LifeStat)lifeStat.Clone(),
            reactionsClone
        );
    }

/// <summary>Gets the characters that can currently be targeted.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public HashSet<ICharacter> GetPotentialTargets(IGameState gameState)
    {
        HashSet<ICharacter> potentialTargets = [];
        foreach (IPlayer player in gameState.NonActivePlayers)
        {
            player.Characters.ForEach(c => potentialTargets.Add(c));
        }
        return potentialTargets;
    }
}
