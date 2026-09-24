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
/// <summary>Initializes a new instance of the <see cref="Card"/> type.</summary>
/// <param name="name">The name value.</param>
/// <param name="[]">The [] value.</param>
/// <param name="name">The name value.</param>
    public Card(string name = "") : this([], [], default!, name)
    {
    }

/// <summary>Initializes a new instance of the <see cref="Card"/> type.</summary>
/// <param name="components">The components value.</param>
/// <param name="reactions">The reactions value.</param>
/// <param name="owner">The owner value.</param>
/// <param name="name">The name value.</param>
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

/// <summary>Determines whether this card can currently be cast.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public virtual bool IsCastable(IGameState gameState)
    {
        IPlayer owner = FindParentPlayer(gameState);
        return owner != null
            && owner == gameState.ActivePlayer
            && owner.Hand.Contains(this)
            && ManaValue <= gameState.ActivePlayer.ManaValue;
    }

/// <summary>Finds the parent card in the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override ICard FindParentCard(IGameState gameState)
    {
        return this;
    }

/// <summary>Finds the parent player in the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
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
