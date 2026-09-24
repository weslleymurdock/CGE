using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public abstract class ReactiveCompound : Compound, IReactive
{
    public List<IReaction> Reactions { get; }

/// <summary>Initializes a new instance of the <see cref="ReactiveCompound"/> type.</summary>
/// <param name="components">The components value.</param>
    public ReactiveCompound(List<ICardComponent> components)
        : this(components, [])
    {
    }

/// <summary>Initializes a new instance of the <see cref="ReactiveCompound"/> type.</summary>
/// <param name="components">The components value.</param>
/// <param name="reactions">The reactions value.</param>
    [JsonConstructor]
    protected ReactiveCompound(List<ICardComponent> components, List<IReaction> reactions)
        : base(components)
    {
        Reactions = reactions;
    }

/// <summary>Gets all reactions associated with this object.</summary>
/// <returns>The result of the operation.</returns>
    public List<IReaction> AllReactions()
    {
        List<IReaction> allReactions = [.. Reactions];
        Components.ForEach(c => allReactions.AddRange(c.AllReactions()));
        return allReactions;
    }

/// <summary>Reacts to the specified action event.</summary>
/// <param name="game">The game value.</param>
/// <param name="actionEvent">The actionEvent value.</param>
    public virtual void ReactTo(IGame game, IActionEvent actionEvent)
    {
        AllReactions().ForEach(r => r.ReactTo(game, actionEvent));
    }

/// <summary>Finds the parent card in the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public abstract ICard FindParentCard(IGameState gameState);

/// <summary>Finds the parent player in the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public abstract IPlayer FindParentPlayer(IGameState gameState);
}
