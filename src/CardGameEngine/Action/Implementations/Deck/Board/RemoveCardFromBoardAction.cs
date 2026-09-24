using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class RemoveCardFromBoardAction : Action
{
    [JsonProperty]
    public readonly IBoard Board;

    [JsonProperty]
    public ICard Card;

/// <summary>Initializes a new instance of the <see cref="RemoveCardFromBoardAction"/> type.</summary>
/// <param name="board">The board value.</param>
/// <param name="card">The card value.</param>
/// <param name="isAborted">The isAborted value.</param>
    [JsonConstructor]
    public RemoveCardFromBoardAction(IBoard board, ICard card, bool isAborted = false)
        : base(isAborted)
    {
        Board = board;
        Card = card;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new RemoveCardFromBoardAction(
            (IBoard)Board.Clone(),
            (ICard)Card.Clone(),
            IsAborted
        );
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        Board.Remove(Card);
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        return Board.Contains(Card);
    }
}
