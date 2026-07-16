using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public abstract class Card : ReactiveCompound, ICard
{
    public abstract IPlayer Owner { get; set; }
    public string Name { get; set; } 
    public Card(string name = "") : this(new List<ICardComponent>(), new List<IReaction>(), default!, name)
    {
    }

    [JsonConstructor]
    protected Card(List<ICardComponent> components, List<IReaction> reactions, IPlayer owner, string name)
        : base(components, reactions)
    {
        this.Owner = owner;
        this.Name = name == "" ? Guid.CreateVersion7().ToString() : name;
    }

    [JsonIgnore]
    public int ManaValue
    {
        get => Math.Max(0, Components.Sum(c => c.ManaValue));
        set
        {
            Components.Add(new CardComponent(value - Components.Sum(c => c.ManaValue), 0));
        }
    }

    [JsonIgnore]
    public int ManaBaseValue
    {
        get => Math.Max(0, Components.Sum(c => c.ManaBaseValue));
        set
        {
            Components.Add(new CardComponent(0, value - Components.Sum(c => c.ManaBaseValue)));
        }
    }

    public virtual bool IsCastable(IGameState gameState)
    {
        IPlayer owner = FindParentPlayer(gameState);
        return owner != null
            && owner == gameState.ActivePlayer
            && owner.Hand.Contains(this)
            && ManaValue <= gameState.ActivePlayer.ManaValue;
    }

    public override ICard FindParentCard(IGameState gameState)
    {
        return this;
    }

    public override IPlayer FindParentPlayer(IGameState gameState)
    {
        foreach (IPlayer player in gameState.Players)
        {
            if (player.AllCards.Contains(this))
            {
                return player;
            }
        }
        throw new CardGameEngineException($"Player not found for card {this}");
    }
}
