using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class ModifyReadyToAttackAction : Action
{
    [JsonProperty]
    public IMonsterCard MonsterCard;

    [JsonProperty]
    public bool IsReadyToAttack;

    [JsonConstructor]
    public ModifyReadyToAttackAction(
        IMonsterCard monsterCard,
        bool isReadyToAttack,
        bool isAborted = false
        ) : base(isAborted)
    {
        MonsterCard = monsterCard;
        IsReadyToAttack = isReadyToAttack;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new ModifyReadyToAttackAction(
            (IMonsterCard)MonsterCard.Clone(),
            IsReadyToAttack,
            IsAborted
        );
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        MonsterCard.IsReadyToAttack = IsReadyToAttack;
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        return MonsterCard.IsReadyToAttack != IsReadyToAttack
            && gameState.AllCardsOnTheBoard.Contains(MonsterCard);
    }
}
