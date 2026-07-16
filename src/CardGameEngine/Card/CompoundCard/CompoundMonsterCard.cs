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

    public CompoundMonsterCard(IMonsterCard monsterCard)
        : this(new List<IMonsterCard> { monsterCard })
    {
    }

    public CompoundMonsterCard(int mana, int attack, int life)
        : this(new MonsterCard(mana, attack, life))
    {
    }

    public void Attack(IGame game, ICharacter targetCharacter)
    {
        game.Execute(new ModifyLifeStatAction(targetCharacter, -this.AttackValue));
        game.Execute(new ModifyLifeStatAction(this, -targetCharacter.AttackValue));
        game.Execute(new ModifyReadyToAttackAction(this, IsReadyToAttack));
        game.NextTurn();
    }

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

    public override void AddComponent(ICard card)
    {
        ((IMonsterCard)card).IsReadyToAttack = this.IsReadyToAttack;
        base.AddComponent(card);
    }

    public bool IsSummonable(IGameState gameState)
    {
        throw new NotImplementedException();
    }

    public HashSet<ICharacter> GetPotentialTargets(IGameState gameState)
    {
        throw new NotImplementedException();
    }

    public override CompoundMonsterCard Clone()
    {
        return new(Components.ConvertAll(c => (IMonsterCard)c.Clone()));
    }
}
