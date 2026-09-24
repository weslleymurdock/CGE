using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public abstract class CastSpellAction : Action
{
    [JsonProperty]
    public IPlayer Player;

    [JsonProperty]
    public ISpellCard SpellCard;

/// <summary>Initializes a new instance of the <see cref="CastSpellAction"/> type.</summary>
/// <param name="player">The player value.</param>
/// <param name="spellCard">The spellCard value.</param>
/// <param name="isAborted">The isAborted value.</param>
    [JsonConstructor]
    public CastSpellAction(IPlayer player, ISpellCard spellCard, bool isAborted = false)
        : base(isAborted)
    {
        Player = player;
        SpellCard = spellCard;
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override abstract void Execute(IGame game);

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override abstract bool IsExecutable(IGameState gameState);
}
