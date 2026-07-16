using CardGameEngine;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace CardGameEngine.Tests.UnitTests;

public class BoardTests
{
    [Fact]
    public void Board_DefaultConstructor_InitializesEmptySlots()
    {
        // Arrange & Act
        var board = new Board();

        // Assert
        Assert.NotNull(board.AllCards);
        Assert.Empty(board.AllCards);
        Assert.True(board.IsEmpty);
        Assert.Equal(0, board.Size);
        Assert.Equal(6, board.MaxSize);

        for (int i = 0; i < board.MaxSize; i++)
        {
            Assert.Null(board[i]);
        }
    }

    [Fact]
    public void AddAt_AddsCardToSpecificSlot_WhenSlotIsFree()
    {
        // Arrange
        var board = new Board();
        var mockCard = new Mock<ICard>();
        int index = 2;

        // Act
        board.AddAt(index, mockCard.Object);

        // Assert
        Assert.False(board.IsEmpty);
        Assert.Equal(1, board.Size);
        Assert.Same(mockCard.Object, board[index]);
    }

    [Fact]
    public void AddAt_ThrowsException_WhenSlotIsOccupied()
    {
        // Arrange
        var board = new Board();
        var mockCard1 = new Mock<ICard>();
        var mockCard2 = new Mock<ICard>();
        int index = 1;
        board.AddAt(index, mockCard1.Object);

        // Act & Assert
        var exception = Assert.Throws<CardGameEngineException>(() => board.AddAt(index, mockCard2.Object));
        Assert.Contains($"Cannot add card to board, because position {index} is already occupied!", exception.Message);
    }

    [Fact]
    public void AddAt_ThrowsException_WhenIndexIsOutOfRange()
    {
        // Arrange
        var board = new Board();
        var mockCard = new Mock<ICard>();

        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => board.AddAt(board.MaxSize, mockCard.Object));
        Assert.Throws<IndexOutOfRangeException>(() => board.AddAt(-1, mockCard.Object));
    }

    [Fact]
    public void Remove_RemovesCardFromBoard()
    {
        // Arrange
        var board = new Board();
        var mockCard1 = new Mock<ICard>();
        var mockCard2 = new Mock<ICard>();
        board.AddAt(0, mockCard1.Object);
        board.AddAt(1, mockCard2.Object);

        // Act
        board.Remove(mockCard1.Object);

        // Assert
        Assert.Equal(1, board.Size);
        Assert.Null(board[0]);
        Assert.Contains(mockCard2.Object, board.AllCards);
    }

    [Fact]
    public void Remove_DoesNothing_IfCardNotInBoard()
    {
        // Arrange
        var board = new Board();
        var mockCard1 = new Mock<ICard>();
        var mockCard2 = new Mock<ICard>();
        board.AddAt(0, mockCard1.Object);

        // Act
        board.Remove(mockCard2.Object);

        // Assert
        Assert.Equal(1, board.Size);
        Assert.NotNull(board[0]);
        Assert.Contains(mockCard1.Object, board.AllCards);
    }

    [Fact]
    public void IsFreeSlot_ReturnsTrue_WhenSlotIsNull()
    {
        // Arrange
        var board = new Board();

        // Act & Assert
        Assert.True(board.IsFreeSlot(3));
    }

    [Fact]
    public void IsFreeSlot_ReturnsFalse_WhenSlotIsOccupied()
    {
        // Arrange
        var board = new Board();
        board.AddAt(3, new Mock<ICard>().Object);

        // Act & Assert
        Assert.False(board.IsFreeSlot(3));
    }

    [Fact]
    public void IsFreeSlot_ThrowsException_WhenIndexOutOfRange()
    {
        // Arrange
        var board = new Board();

        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => board.IsFreeSlot(board.MaxSize));
        Assert.Throws<IndexOutOfRangeException>(() => board.IsFreeSlot(-1));
    }

    [Fact]
    public void Contains_ReturnsTrue_IfCardIsInBoard()
    {
        // Arrange
        var board = new Board();
        var mockCard = new Mock<ICard>();
        board.AddAt(0, mockCard.Object);

        // Act & Assert
        Assert.True(board.Contains(mockCard.Object));
    }

    [Fact]
    public void Contains_ReturnsFalse_IfCardIsNotInBoard()
    {
        // Arrange
        var board = new Board();
        var mockCard = new Mock<ICard>();

        // Act & Assert
        Assert.False(board.Contains(mockCard.Object));
    }

    [Fact]
    public void Indexer_ReturnsCorrectCard()
    {
        // Arrange
        var board = new Board();
        var mockCard = new Mock<ICard>();
        board.AddAt(2, mockCard.Object);

        // Act
        var cardAtIndex = board[2];

        // Assert
        Assert.Same(mockCard.Object, cardAtIndex);
    }

    [Fact]
    public void Indexer_ReturnsNull_ForEmptySlot()
    {
        // Arrange
        var board = new Board();

        // Act
        var cardAtIndex = board[0];

        // Assert
        Assert.Null(cardAtIndex);
    }

    [Fact]
    public void Board_Clone_ReturnsNewInstanceWithClonedCardsAndSameOrder()
    {
        // Arrange
        var mockCard1 = new Mock<ICard>();
        mockCard1.Setup(c => c.Clone()).Returns(new Mock<ICard>().Object);
        var mockCard2 = new Mock<ICard>();
        mockCard2.Setup(c => c.Clone()).Returns(new Mock<ICard>().Object);

        var originalBoard = new Board();
        originalBoard.AddAt(1, mockCard1.Object);
        originalBoard.AddAt(3, mockCard2.Object);

        // Act
        var clonedBoard = (Board)originalBoard.Clone();

        // Assert
        Assert.NotSame(originalBoard, clonedBoard);
        Assert.Equal(originalBoard.Size, clonedBoard.Size);
        Assert.Equal(originalBoard.MaxSize, clonedBoard.MaxSize);

        for (int i = 0; i < originalBoard.MaxSize; i++)
        {
            if (originalBoard[i] != null)
            {
                Assert.NotNull(clonedBoard[i]);
                Assert.NotSame(originalBoard[i], clonedBoard[i]); // Cloned card should be a new instance
            }
            else
            {
                Assert.Null(clonedBoard[i]);
            }
        }
    }
}
