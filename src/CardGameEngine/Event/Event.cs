using System;
using Newtonsoft.Json;

namespace CardGameEngine
{
    [Serializable]
    public abstract class Event : Action
    {
/// <summary>Initializes a new instance of the <see cref="Event"/> type.</summary>
        [JsonConstructor]
        public Event()
        {
        }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
        public override void Execute(IGame game)
        {
            // An event should not alter the game state.
        }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
        public override bool IsExecutable(IGameState gameState)
        {
            return true;
        }
    }
}
