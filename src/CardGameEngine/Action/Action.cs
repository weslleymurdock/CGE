using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public abstract class Action : IAction
{
    public bool IsAborted { get; set; }

    [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="Action"/> type.</summary>
/// <param name="isAborted">The isAborted value.</param>
    public Action(bool isAborted = false)
    {
        IsAborted = isAborted;
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public abstract void Execute(IGame game);
/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public abstract bool IsExecutable(IGameState gameState);
/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public abstract object Clone();
}
