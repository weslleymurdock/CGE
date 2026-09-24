using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class ModifyAttackStatAction : Action
{
    [JsonProperty]
    public IAttacking Attacking;

    [JsonProperty]
    public int Delta;

/// <summary>Initializes a new instance of the <see cref="ModifyAttackStatAction"/> type.</summary>
/// <param name="attacking">The attacking value.</param>
/// <param name="delta">The delta value.</param>
/// <param name="isAborted">The isAborted value.</param>
    [JsonConstructor]
    public ModifyAttackStatAction(IAttacking attacking, int delta, bool isAborted = false)
        : base(isAborted)
    {
        Attacking = attacking;
        Delta = delta;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new ModifyAttackStatAction((IAttacking)Attacking.Clone(), Delta, IsAborted);
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        Attacking.AttackValue += Delta;
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        return true;
    }
}
