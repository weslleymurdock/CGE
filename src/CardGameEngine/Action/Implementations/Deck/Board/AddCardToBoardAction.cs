using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class AddCardToBoardAction : Action
{
    [JsonProperty]
    public readonly IBoard Board;

    [JsonProperty]
    public ICard Card;

    [JsonProperty]
    public int BoardIndex;

    [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="AddCardToBoardAction"/> type.</summary>
/// <param name="board">The board value.</param>
/// <param name="card">The card value.</param>
/// <param name="boardIndex">The boardIndex value.</param>
/// <param name="isAborted">The isAborted value.</param>
    public AddCardToBoardAction(IBoard board, ICard card, int boardIndex, bool isAborted = false)
        : base(isAborted)
    {
        Board = board;
        Card = card;
        BoardIndex = boardIndex;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new AddCardToBoardAction(
            (IBoard)Board.Clone(),
            (ICard)Card.Clone(),
            BoardIndex,
            IsAborted
        );
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
    public override void Execute(IGame game)
    {
        Board.AddAt(BoardIndex, Card);
    }

/// <summary>Determines whether this operation can be executed for the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsExecutable(IGameState gameState)
    {
        return Card != null
            && Board.IsFreeSlot(BoardIndex);
    }
}
