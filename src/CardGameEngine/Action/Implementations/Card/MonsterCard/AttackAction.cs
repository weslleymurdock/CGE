using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class AttackAction : Action
{
    [JsonProperty]
    public IMonsterCard Attacker;

    [JsonProperty]
    public ICharacter Target;

/// <summary>Initializes a new instance of the <see cref="AttackAction"/> type.</summary>
/// <param name="attacker">The attacker value.</param>
/// <param name="target">The target value.</param>
/// <param name="isAborted">The isAborted value.</param>
    [JsonConstructor]
    public AttackAction(IMonsterCard attacker, ICharacter target, bool isAborted = false)
        : base(isAborted)
    {
        Attacker = attacker;
        Target = target;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new AttackAction(
            (IMonsterCard)Attacker.Clone(),
            (ICharacter)Target.Clone(),
            IsAborted
        );
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        game.Execute(new ModifyLifeStatAction(Target, -Attacker.AttackValue));
        game.Execute(new ModifyLifeStatAction(Attacker, -Target.AttackValue));
        game.Execute(new ModifyReadyToAttackAction(Attacker, false));
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        return Attacker != null
            && Target != null
            && gameState.ActivePlayer.Board.Contains(Attacker)
            && Attacker.IsReadyToAttack
            && Attacker.GetPotentialTargets(gameState).Contains(Target);
    }
}
