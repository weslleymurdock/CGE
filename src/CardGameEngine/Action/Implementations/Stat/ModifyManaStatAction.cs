using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class ModifyManaStatAction : Action
{
    [JsonProperty]
    public IManaful Manaful;

    [JsonProperty]
    public int DeltaValue;

    [JsonProperty]
    public int DeltaBaseValue;

    [JsonConstructor]
    public ModifyManaStatAction(
        IManaful manaful,
        int deltaValue,
        int deltaBaseValue,
        bool isAborted = false
        ) : base(isAborted)
    {
        Manaful = manaful;
        DeltaValue = deltaValue;
        DeltaBaseValue = deltaBaseValue;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new ModifyManaStatAction(
            (IManaful)Manaful.Clone(),
            DeltaValue,
            DeltaBaseValue,
            IsAborted
        );
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        Manaful.ManaBaseValue += DeltaBaseValue;
        Manaful.ManaValue += DeltaValue;
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        return true;
    }
}
