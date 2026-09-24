namespace CardGameEngine;

[Serializable]
public class CompoundMonsterCard : CompoundCard, IMonsterCard
{
    public ManaPoolStat ManaStat { get; }
    public LifeStat LifeStat { get; }
    public AttackStat AttackStat { get; }

    public bool IsReadyToAttack {
        get => ((IMonsterCard)Components[0]).IsReadyToAttack;
        set
        {
            Components.ForEach(c => ((IMonsterCard)c).IsReadyToAttack = value);
        }
    }
    public bool IsAlive => LifeStat.Value > 0;

    public int AttackValue { get; set; }
    public int AttackBaseValue { get; set; }
    public int LifeValue { get; set; }
    public int LifeBaseValue { get; set; }

/// <summary>Initializes a new instance of the <see cref="CompoundMonsterCard"/> type.</summary>
/// <param name="components">The components value.</param>
    public CompoundMonsterCard(List<IMonsterCard> components)
        : base([.. components.Cast<ICard>()], ((Card)components[0]).Name)
    {
        components.ForEach(c => Components.Add(c));

        this.ManaStat = new ManaPoolStat(
            components.Sum(c => c.ManaStat.Value),
            components.Max(c => c.ManaStat.BaseValue)
        );
        this.LifeStat = new LifeStat(
            components.Sum(c => c.LifeStat.Value),
            components.Max(c => c.LifeStat.BaseValue)
        );
        this.AttackStat = new AttackStat(
            components.Sum(c => c.AttackStat.Value),
            components.Max(c => c.AttackStat.BaseValue)
        );
        AttackValue = AttackStat.Value;
        AttackBaseValue = AttackStat.BaseValue;
        LifeValue = LifeStat.Value;
        LifeBaseValue = LifeStat.BaseValue;
        Reactions.Add(new SetReadyToAttackOnStartOfTurnEventReaction());
    }

/// <summary>Initializes a new instance of the <see cref="CompoundMonsterCard"/> type.</summary>
/// <param name="monsterCard">The monsterCard value.</param>
    public CompoundMonsterCard(IMonsterCard monsterCard)
        : this([monsterCard])
    {
    }

/// <summary>Initializes a new instance of the <see cref="CompoundMonsterCard"/> type.</summary>
/// <param name="mana">The mana value.</param>
/// <param name="attack">The attack value.</param>
/// <param name="life">The life value.</param>
    public CompoundMonsterCard(int mana, int attack, int life)
        : this(new MonsterCard(mana, attack, life))
    {
    }

/// <summary>Performs an attack against the specified target.</summary>
/// <param name="game">The game value.</param>
/// <param name="targetCharacter">The targetCharacter value.</param>
    public void Attack(IGame game, ICharacter targetCharacter)
    {
        game.Execute(new ModifyLifeStatAction(targetCharacter, -this.AttackValue));
        game.Execute(new ModifyLifeStatAction(this, -targetCharacter.AttackValue));
        game.Execute(new ModifyReadyToAttackAction(this, IsReadyToAttack));
        game.NextTurn();
    }

/// <summary>Gets the characters that can currently be targeted.</summary>
/// <param name="game">The game value.</param>
/// <returns>The result of the operation.</returns>
    public HashSet<ICharacter> GetPotentialTargets(IGame game)
    {
        //Compute the intersection of all potential targets
        HashSet<ICharacter> potentialTargets = ((IMonsterCard)Components[0]).GetPotentialTargets(game);
        for(int i=1; i<Components.Count; ++i)
        {
            IMonsterCard monsterCard = (IMonsterCard)Components[i];
            HashSet<ICharacter> potTargets = monsterCard.GetPotentialTargets(game);
            potentialTargets.RemoveWhere(t => !potTargets.Contains(t));
        }
        return potentialTargets;
    }

/// <summary>Adds a component to this object.</summary>
/// <param name="card">The card value.</param>
    public override void AddComponent(ICard card)
    {
        ((IMonsterCard)card).IsReadyToAttack = this.IsReadyToAttack;
        base.AddComponent(card);
    }

/// <summary>Determines whether this card can currently be summoned.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public bool IsSummonable(IGameState gameState)
    {
        throw new NotImplementedException();
    }

/// <summary>Gets the characters that can currently be targeted.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public HashSet<ICharacter> GetPotentialTargets(IGameState gameState)
    {
        throw new NotImplementedException();
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override CompoundMonsterCard Clone()
    {
        return new(Components.ConvertAll(c => (IMonsterCard)c.Clone()));
    }
}
