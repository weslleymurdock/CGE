using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class DieAction : Action
{
    [JsonProperty]
    public IMonsterCard MonsterCard;

/// <summary>Initializes a new instance of the <see cref="DieAction"/> type.</summary>
/// <param name="monsterCard">The monsterCard value.</param>
/// <param name="isAborted">The isAborted value.</param>
    [JsonConstructor]
    public DieAction(IMonsterCard monsterCard, bool isAborted = false)
        : base(isAborted)
    {
        MonsterCard = monsterCard;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new DieAction((IMonsterCard)MonsterCard.Clone(), IsAborted);
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        IPlayer owner = MonsterCard.FindParentPlayer(game);
        game.Execute(new RemoveCardFromBoardAction(owner.Board, MonsterCard));
        game.Execute(new AddCardToGraveyardAction(owner.Graveyard, MonsterCard));
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        IPlayer owner = MonsterCard.FindParentPlayer(gameState);
        return owner != null
            && owner.Board.Contains(MonsterCard);
    }
}
