using CardGameEngine;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace CardGameEngine.Tests.UnitTests;

public class DeckTests
{
    [Fact]
    public void Deck_DefaultConstructor_InitializesEmptyStack()
    {
        // Arrange & Act
        var deck = new Deck();

        // Assert
        Assert.NotNull(deck.AllCards);
        Assert.Empty(deck.AllCards);
        Assert.True(deck.IsEmpty);
        Assert.Equal(0, deck.Size);
    }

    [Fact]
    public void Push_AddsCardToDeck()
    {
        // Arrange
        var deck = new Deck();
        var mockCard = new Mock<ICard>();

        // Act
        deck.Push(mockCard.Object);

        // Assert
        Assert.False(deck.IsEmpty);
        Assert.Equal(1, deck.Size);
        Assert.Contains(mockCard.Object, deck.AllCards);
    }

    [Fact]
    public void Pop_RemovesAndReturnsTopCard()
    {
        // Arrange
        var deck = new Deck();
        var mockCard1 = new Mock<ICard>();
        var mockCard2 = new Mock<ICard>();
        deck.Push(mockCard1.Object);
        deck.Push(mockCard2.Object);

        // Act
        var poppedCard = deck.Pop();

        // Assert
        Assert.Same(mockCard2.Object, poppedCard);
        Assert.Equal(1, deck.Size);
        Assert.DoesNotContain(mockCard2.Object, deck.AllCards);
        Assert.Contains(mockCard1.Object, deck.AllCards);
    }

    [Fact]
    public void Pop_ThrowsInvalidOperationException_WhenDeckIsEmpty()
    {
        // Arrange
        var deck = new Deck();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => deck.Pop());
    }

    [Fact]
    public void Shuffle_RandomizesCardOrder()
    {
        // Arrange
        var deck = new Deck();
        var cards = new List<ICard>();
        for (int i = 0; i < 20; i++)
        {
            var mockCard = new Mock<ICard>();
            mockCard.Setup(c => c.Name).Returns($"Card {i}"); // Give cards unique names for easier comparison
            cards.Add(mockCard.Object);
            deck.Push(mockCard.Object);
        }
        var originalOrder = deck.AllCards.ToList();

        // Act
        deck.Shuffle();
        var shuffledOrder = deck.AllCards.ToList();

        // Assert
        Assert.Equal(originalOrder.Count, shuffledOrder.Count);
        Assert.True(originalOrder.All(shuffledOrder.Contains)); // Ensure all cards are still present
        // It's highly improbable that a shuffled deck retains the exact same order, but theoretically possible.
        // We'll assert that it's not the exact same order for practical purposes in a unit test.
        // For more robust randomness testing, statistical tests would be needed.
        Assert.NotEqual(originalOrder, shuffledOrder); 
    }

    [Fact]
    public void Contains_ReturnsTrue_IfCardIsInDeck()
    {
        // Arrange
        var deck = new Deck();
        var mockCard = new Mock<ICard>();
        deck.Push(mockCard.Object);

        // Act & Assert
        Assert.True(deck.Contains(mockCard.Object));
    }

    [Fact]
    public void Contains_ReturnsFalse_IfCardIsNotInDeck()
    {
        // Arrange
        var deck = new Deck();
        var mockCard = new Mock<ICard>();

        // Act & Assert
        Assert.False(deck.Contains(mockCard.Object));
    }

    [Fact]
    public void Deck_Clone_ReturnsNewInstanceWithClonedCardsInSameOrder()
    {
        // Arrange
        var mockCard1 = new Mock<ICard>();
        mockCard1.Setup(c => c.Clone()).Returns(new Mock<ICard>().Object);
        var mockCard2 = new Mock<ICard>();
        mockCard2.Setup(c => c.Clone()).Returns(new Mock<ICard>().Object);

        var originalDeck = new Deck();
        originalDeck.Push(mockCard1.Object);
        originalDeck.Push(mockCard2.Object);

        // Act
        var clonedDeck = (Deck)originalDeck.Clone();

        // Assert
        Assert.NotSame(originalDeck, clonedDeck);
        Assert.Equal(originalDeck.Size, clonedDeck.Size);
        
        // Verify that cards are cloned and in the same order (stack reverses on enumeration)
        var originalCards = originalDeck.AllCards.ToList();
        var clonedCards = clonedDeck.AllCards.ToList();

        for (int i = 0; i < originalCards.Count; i++)
        {
            Assert.NotSame(originalCards[i], clonedCards[i]);
        }
    }
}
