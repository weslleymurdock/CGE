using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class CastMonsterAction : Action
{
    [JsonProperty]
    public IPlayer Player;

    [JsonProperty]
    public IMonsterCard MonsterCard;

    [JsonProperty]
    public int BoardIndex;

    [JsonConstructor]
    public CastMonsterAction(IPlayer player, IMonsterCard monsterCard,
        int boardIndex, bool isAborted = false
        ) : base(isAborted)
    {
        Player = player;
        MonsterCard = monsterCard;
        BoardIndex = boardIndex;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new CastMonsterAction(
            null, // otherwise circular dependencies
            (IMonsterCard)MonsterCard.Clone(),
            BoardIndex,
            IsAborted
            );
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        game.Execute(new ModifyManaStatAction(Player, -MonsterCard.ManaValue, 0));
        game.Execute(new RemoveCardFromHandAction(Player.Hand, MonsterCard));
        game.Execute(new AddCardToBoardAction(Player.Board, MonsterCard, BoardIndex));
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        return Player == gameState.ActivePlayer
            && Player.Hand.Contains(MonsterCard)
            && MonsterCard.IsSummonable(gameState)
            && Player.Board.IsFreeSlot(BoardIndex);
    }
}
