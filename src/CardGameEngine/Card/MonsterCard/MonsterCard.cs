using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class MonsterCard : Card, IMonsterCard
{
    public bool IsReadyToAttack { get; set; }
    public ManaPoolStat ManaStat { get; private set; }
    public LifeStat LifeStat { get; private set; }
    public AttackStat AttackStat { get; private set; }
    public MonsterCard()
        : this(new List<IMonsterCardComponent>(), default!)
    {
    }

    /// <summary>
    /// Represents a certain type of Card that is played
    /// onto the Player's Board.
    /// </summary>
    /// <param name="mana"></param>
    /// <param name="attack"></param>
    /// <param name="life"></param>
    public MonsterCard(int mana, int attack, int life, IPlayer owner = default!, string name = "")
        : this(new List<IMonsterCardComponent> { new MonsterCardComponent(mana, attack, life) }, owner)
    {

        this.ManaStat = new ManaPoolStat(mana, 0);
        this.LifeStat = new LifeStat(life);
        this.AttackStat = new AttackStat(attack);
    }

    /// <summary>
    /// Represents a certain type of Card that is played
    /// onto the Player's Board.
    /// </summary>
    /// <param name="components"></param>
    public MonsterCard(List<IMonsterCardComponent> components, IPlayer owner)
        : this(components, false, owner)
    {

       
    }

    public MonsterCard(
        List<IMonsterCardComponent> components,
        bool isReadyToAttack,
        IPlayer owner
        ) : this(components.ConvertAll(c => (ICardComponent)c), new List<IReaction>(), isReadyToAttack, owner)
    {
        Reactions.Add(new SetReadyToAttackOnStartOfTurnEventReaction());
    }


    public MonsterCard(
        List<ICardComponent> components,
        List<IReaction> reactions,
        bool isReadyToAttack,
        IPlayer owner
        ) : this(components, reactions, isReadyToAttack, owner, Guid.CreateVersion7().ToString())
    {
        IsReadyToAttack = isReadyToAttack;
        Owner = owner;
    }


    [JsonConstructor]
    public MonsterCard(
        List<ICardComponent> components,
        List<IReaction> reactions,
        bool isReadyToAttack, 
        IPlayer owner,
        string name = ""
        ) : base(components, reactions, owner, name)
    {
        IsReadyToAttack = isReadyToAttack;
        Owner = owner;
    }

    [JsonIgnore]
    public bool IsAlive => LifeValue > 0;

    [JsonIgnore]
    public int AttackValue
    {
        get => Math.Max(0, GetSum(c => c.AttackValue));
        set
        {
            Components.Add(new MonsterCardComponent(0, 0, value - GetSum(c => c.AttackValue), 0, 0, 0));
        }
    }

    [JsonIgnore]
    public int AttackBaseValue
    {
        get => Math.Max(0, GetSum(c => c.AttackBaseValue));
        set
        {
            Components.Add(new MonsterCardComponent(0, 0, 0, value - GetSum(c => c.AttackBaseValue), 0, 0));
        }
    }

    [JsonIgnore]
    public int LifeValue
    {
        get => Math.Max(0, GetSum(c => c.LifeValue));
        set
        {
            Components.Add(new MonsterCardComponent(0, 0, 0, 0, value - GetSum(c => c.LifeValue), 0));
        }
    }

    [JsonIgnore]
    public int LifeBaseValue
    {
        get => Math.Max(0, GetSum(c => c.LifeBaseValue));
        set
        {
            Components.Add(new MonsterCardComponent(0, 0, 0, 0, 0, value - GetSum(c => c.LifeBaseValue)));
        }
    }

    public override IPlayer Owner { get; set; }

    private int GetSum(Func<IMonsterCardComponent, int> GetValue)
    {
        return Components.Where(c => c is IMonsterCardComponent).Sum(c => GetValue((IMonsterCardComponent)c));
    }

    public void Attack(IGame game, ICharacter target)
    {
        if(!IsReadyToAttack)
        {
            throw new CardGameEngineException("Failed to attack with a MonsterCard " +
                "that is not ready to attack!");
        }
        if(!GetPotentialTargets(game).Contains(target))
        {
            throw new CardGameEngineException("Cannot attack a target character " +
                "that is not specified in the list of potential targets!");
        }

        game.Execute(new AttackAction(this, target));
    }

    public virtual HashSet<ICharacter> GetPotentialTargets(IGameState gameState)
    {
        if (Components.Count == 0)
        {
            return new HashSet<ICharacter>();
        }

        //Compute the intersection of all potential targets
        HashSet<ICharacter> potentialTargets = ((ITargetful)Components[0]).GetPotentialTargets(gameState);
        foreach (ICardComponent component in Components)
        {
            potentialTargets.IntersectWith(((ITargetful)component).GetPotentialTargets(gameState));
        }
        return potentialTargets;
    }

    public bool IsSummonable(IGameState gameState)
    {
        IBoard board = gameState.ActivePlayer.Board;
        return base.IsCastable(gameState)
                && board.AllCards.Count < board.MaxSize;
    }

    public override object Clone()
    {
        List<ICardComponent> componentsClone = new List<ICardComponent>();
        Components.ForEach(c => componentsClone.Add((ICardComponent)c.Clone()));

        List<IReaction> reactionsClone = new List<IReaction>();
        Reactions.ForEach(r => reactionsClone.Add((IReaction)r.Clone()));

        return new MonsterCard(
            componentsClone,
            reactionsClone,
            IsReadyToAttack,
            Owner
        );
    }
}
