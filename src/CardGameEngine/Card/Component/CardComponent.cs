using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class CardComponent : Reaction, ICardComponent
{
    [JsonProperty]
    protected ManaCostStat manaCostStat;

    public List<IReaction> Reactions { get; }

/// <summary>Initializes a new instance of the <see cref="CardComponent"/> type.</summary>
/// <param name="mana">The mana value.</param>
    public CardComponent(int mana)
        : this(new ManaCostStat(mana, mana), [])
    {
    }

/// <summary>Initializes a new instance of the <see cref="CardComponent"/> type.</summary>
/// <param name="manaValue">The manaValue value.</param>
/// <param name="manaBaseValue">The manaBaseValue value.</param>
    public CardComponent(int manaValue, int manaBaseValue)
        : this(new ManaCostStat(manaValue, manaBaseValue), [])
    {
    }

    [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="CardComponent"/> type.</summary>
/// <param name="manaCostStat">The manaCostStat value.</param>
/// <param name="reactions">The reactions value.</param>
    protected CardComponent(ManaCostStat manaCostStat, List<IReaction> reactions)
    {
        this.manaCostStat = manaCostStat;
        Reactions = reactions;
    }

    [JsonIgnore]
    public int ManaValue {
        get => manaCostStat.Value;
        set => manaCostStat.Value = value;
    }

    [JsonIgnore]
    public int ManaBaseValue {
        get => manaCostStat.BaseValue;
        set => manaCostStat.BaseValue = value;
    }

/// <summary>Gets all reactions associated with this object.</summary>
/// <returns>The result of the operation.</returns>
    public List<IReaction> AllReactions()
    {
        return [.. Reactions];
    }

/// <summary>Reacts to the specified action event.</summary>
/// <param name="game">The game value.</param>
/// <param name="actionEvent">The actionEvent value.</param>
    public override void ReactTo(IGame game, IActionEvent actionEvent)
    {
        AllReactions().ForEach(r => r.ReactTo(game, actionEvent));
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

        return new CardComponent(
            (ManaCostStat)manaCostStat.Clone(),
            reactionsClone
        );
    }

/// <summary>Performs the FindCard operation.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public ICard FindCard(IGameState gameState)
    {
        foreach (ICard card in gameState.AllCards)
        {
            foreach (ICardComponent cardComponent in card.Components)
            {
                if (cardComponent == this)
                {
                    return card;
                }
            }
        }
        throw new CardGameEngineException("Card not found for the given component.");
    }
}
