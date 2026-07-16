using CardGameEngine;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace CardGameEngine.Tests.UnitTests;

public class HandTests
{
    [Fact]
    public void Hand_DefaultConstructor_InitializesEmptyList()
    {
        // Arrange & Act
        var hand = new Hand();

        // Assert
        Assert.NotNull(hand.AllCards);
        Assert.Empty(hand.AllCards);
        Assert.True(hand.IsEmpty);
        Assert.Equal(0, hand.Size);
        Assert.Equal(10, hand.MaxSize);
    }

    [Fact]
    public void Add_AddsCardToHand_WhenNotFull()
    {
        // Arrange
        var hand = new Hand();
        var mockCard = new Mock<ICard>();

        // Act
        hand.Add(mockCard.Object);

        // Assert
        Assert.False(hand.IsEmpty);
        Assert.Equal(1, hand.Size);
        Assert.Contains(mockCard.Object, hand.AllCards);
    }

    [Fact]
    public void Add_DoesNotAddCardToHand_WhenFull()
    {
        // Arrange
        var hand = new Hand();
        for (int i = 0; i < hand.MaxSize; i++)
        {
            hand.Add(new Mock<ICard>().Object);
        }
        var overflowCard = new Mock<ICard>();

        // Act
        hand.Add(overflowCard.Object);

        // Assert
        Assert.Equal(hand.MaxSize, hand.Size);
        Assert.DoesNotContain(overflowCard.Object, hand.AllCards);
    }

    [Fact]
    public void Remove_RemovesCardFromHand()
    {
        // Arrange
        var hand = new Hand();
        var mockCard1 = new Mock<ICard>();
        var mockCard2 = new Mock<ICard>();
        hand.Add(mockCard1.Object);
        hand.Add(mockCard2.Object);

        // Act
        hand.Remove(mockCard1.Object);

        // Assert
        Assert.Equal(1, hand.Size);
        Assert.DoesNotContain(mockCard1.Object, hand.AllCards);
        Assert.Contains(mockCard2.Object, hand.AllCards);
    }

    [Fact]
    public void Remove_DoesNothing_IfCardNotInHand()
    {
        // Arrange
        var hand = new Hand();
        var mockCard1 = new Mock<ICard>();
        var mockCard2 = new Mock<ICard>();
        hand.Add(mockCard1.Object);

        // Act
        hand.Remove(mockCard2.Object);

        // Assert
        Assert.Equal(1, hand.Size);
        Assert.Contains(mockCard1.Object, hand.AllCards);
    }

    [Fact]
    public void Contains_ReturnsTrue_IfCardIsInHand()
    {
        // Arrange
        var hand = new Hand();
        var mockCard = new Mock<ICard>();
        hand.Add(mockCard.Object);

        // Act & Assert
        Assert.True(hand.Contains(mockCard.Object));
    }

    [Fact]
    public void Contains_ReturnsFalse_IfCardIsNotInHand()
    {
        // Arrange
        var hand = new Hand();
        var mockCard = new Mock<ICard>();

        // Act & Assert
        Assert.False(hand.Contains(mockCard.Object));
    }

    [Fact]
    public void Indexer_ReturnsCorrectCard()
    {
        // Arrange
        var hand = new Hand();
        var mockCard1 = new Mock<ICard>();
        var mockCard2 = new Mock<ICard>();
        hand.Add(mockCard1.Object);
        hand.Add(mockCard2.Object);

        // Act
        var cardAtIndex1 = hand[1];

        // Assert
        Assert.Same(mockCard2.Object, cardAtIndex1);
    }

    [Fact]
    public void Hand_Clone_ReturnsNewInstanceWithClonedCards()
    {
        // Arrange
        var mockCard1 = new Mock<ICard>();
        mockCard1.Setup(c => c.Clone()).Returns(new Mock<ICard>().Object);
        var mockCard2 = new Mock<ICard>();
        mockCard2.Setup(c => c.Clone()).Returns(new Mock<ICard>().Object);

        var originalHand = new Hand();
        originalHand.Add(mockCard1.Object);
        originalHand.Add(mockCard2.Object);

        // Act
        var clonedHand = (Hand)originalHand.Clone();

        // Assert
        Assert.NotSame(originalHand, clonedHand);
        Assert.Equal(originalHand.Size, clonedHand.Size);
        
        // Verify that cards are cloned and in the same order
        for (int i = 0; i < originalHand.Size; i++)
        {
            Assert.NotSame(originalHand[i], clonedHand[i]);
        }
    }
}
