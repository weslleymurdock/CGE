using System;
using System.Linq;
using Newtonsoft.Json;

namespace CardGameEngine;

public class ModifyActivePlayerAction : Action
{
    [JsonProperty]
    public IPlayer NewActivePlayer;

    [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="ModifyActivePlayerAction"/> type.</summary>
/// <param name="newActivePlayer">The newActivePlayer value.</param>
/// <param name="isAborted">The isAborted value.</param>
    public ModifyActivePlayerAction(IPlayer newActivePlayer, bool isAborted = false)
        : base(isAborted)
    {
        NewActivePlayer = newActivePlayer;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new ModifyActivePlayerAction(
            null, // otherwise circular dependencies
            IsAborted
        );
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        game.ActivePlayer = NewActivePlayer;
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        if(!gameState.Players.Contains(NewActivePlayer))
        {
            throw new CardGameEngineException("Could not change the active " +
                "player because the specified player is not involved " +
                "in the game!");
        }
        return NewActivePlayer != gameState.ActivePlayer;
    }
}
