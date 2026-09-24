using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class ModifyLifeStatAction : Action
{
    [JsonProperty]
    public ILiving Living;

    [JsonProperty]
    public int Delta;

/// <summary>Initializes a new instance of the <see cref="ModifyLifeStatAction"/> type.</summary>
/// <param name="living">The living value.</param>
/// <param name="delta">The delta value.</param>
/// <param name="isAborted">The isAborted value.</param>
    [JsonConstructor]
    public ModifyLifeStatAction(ILiving living, int delta, bool isAborted = false)
        : base(isAborted)
    {
        Living = living;
        Delta = delta;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new ModifyLifeStatAction((ILiving)Living.Clone(), Delta, IsAborted);
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        Living.LifeValue += Delta;
        if(Living.LifeValue <= 0)
        {
            if (Living is IMonsterCard monsterCard)
            {
                game.Execute(new DieAction(monsterCard));
            }
            else if (Living is IPlayer)
            {
                game.Execute(new EndOfGameEvent());
            }
        }
        
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        return !(Living is ICardComponent)
            && Living.LifeValue > 0;
    }
}
