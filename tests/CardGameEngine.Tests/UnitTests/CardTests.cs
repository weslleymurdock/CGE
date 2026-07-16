using CardGameEngine;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace CardGameEngine.Tests.UnitTests;

// Helper class to test the abstract Card class
public class TestCard : Card
{
    public override IPlayer Owner { get; set; }

    public TestCard(IPlayer owner, string name = "") : base(new List<ICardComponent>(), new List<IReaction>(), owner, name)
    {
        Owner = owner;
    }

    public TestCard(List<ICardComponent> components, List<IReaction> reactions, IPlayer owner, string name = "")
        : base(components, reactions, owner, name)
    {
        Owner = owner;
    }

    public override object Clone()
    {
        return new TestCard(new List<ICardComponent>(Components.Select(c => (ICardComponent)c.Clone())),
                            new List<IReaction>(Reactions.Select(r => (IReaction)r.Clone())),
                            Owner,
                            Name);
    }
}

public class CardTests
{
    [Fact]
    public void Card_Constructor_InitializesProperties()
    {
        // Arrange
        var mockPlayer = new Mock<IPlayer>();
        string cardName = "Fireball";

        // Act
        var card = new TestCard(mockPlayer.Object, cardName);

        // Assert
        Assert.Equal(mockPlayer.Object, card.Owner);
        Assert.Equal(cardName, card.Name);
        Assert.NotNull(card.Components);
        Assert.Empty(card.Components);
        Assert.NotNull(card.Reactions);
        Assert.Empty(card.Reactions);
    }

    [Fact]
    public void Card_Constructor_GeneratesGuidNameIfEmpty()
    {
        // Arrange
        var mockPlayer = new Mock<IPlayer>();

        // Act
        var card = new TestCard(mockPlayer.Object, "");

        // Assert
        Assert.False(string.IsNullOrEmpty(card.Name));
        Assert.True(Guid.TryParse(card.Name, out _));
    }

    [Fact]
    public void ManaValue_CalculatesSumOfComponentManaValues()
    {
        // Arrange
        var mockPlayer = new Mock<IPlayer>();
        var cardComponent1 = new Mock<ICardComponent>();
        cardComponent1.Setup(c => c.ManaValue).Returns(3);
        var cardComponent2 = new Mock<ICardComponent>();
        cardComponent2.Setup(c => c.ManaValue).Returns(2);

        var card = new TestCard(new List<ICardComponent> { cardComponent1.Object, cardComponent2.Object }, new List<IReaction>(), mockPlayer.Object);

        // Act & Assert
        Assert.Equal(5, card.ManaValue);
    }

    [Fact]
    public void ManaValue_SetsValueByAddingNewComponent()
    {
        // Arrange
        var mockPlayer = new Mock<IPlayer>();
        var card = new TestCard(mockPlayer.Object);
        card.Components.Add(new CardComponent(2, 0)); // Initial mana of 2

        // Act
        card.ManaValue = 5; // Set desired total mana to 5

        // Assert
        // A new component should be added to make total ManaValue = 5
        Assert.Equal(2, card.Components.Count);
        Assert.Equal(5, card.ManaValue);
    }

    [Fact]
    public void IsCastable_ReturnsTrue_WhenConditionsMet()
    {
        // Arrange
        var mockPlayer = new Mock<IPlayer>();
        var mockHand = new Mock<IHand>();
        var mockGameState = new Mock<IGameState>();
        var mockManaPoolStat = new Mock<IManaful>(); // Mock IManaful for ActivePlayer

        var card = new TestCard(mockPlayer.Object) { Name = "Test Card" };
        card.Components.Add(new CardComponent(3, 0)); // Card costs 3 mana
        mockPlayer.Setup(p => p.AllCards).Returns([card]);
        mockPlayer.Setup(p => p.Hand).Returns(mockHand.Object);
        mockPlayer.Setup(p => p.ManaValue).Returns(5); // Player has 5 mana
        mockHand.Setup(h => h.Contains(card)).Returns(true);

        mockGameState.Setup(gs => gs.ActivePlayer).Returns(mockPlayer.Object);
        mockGameState.Setup(gs => gs.Players).Returns(new List<IPlayer> { mockPlayer.Object });

        // Act
        bool isCastable = card.IsCastable(mockGameState.Object);

        // Assert
        Assert.True(isCastable);
    }

    [Fact]
    public void IsCastable_ReturnsFalse_WhenNotActivePlayer()
    {
        // Arrange
        var mockPlayer = new Mock<IPlayer>();
        var mockOtherPlayer = new Mock<IPlayer>();
        var mockHand = new Mock<IHand>();
        var mockGameState = new Mock<IGameState>();
        var mockManaPoolStat = new Mock<IManaful>();

        var card = new TestCard(mockPlayer.Object) { Name = "Test Card" };
        card.Components.Add(new CardComponent(3, 0));
        mockPlayer.Setup(p => p.AllCards).Returns([card]);
        mockPlayer.Setup(p => p.Hand).Returns(mockHand.Object);
        mockPlayer.Setup(p => p.ManaValue).Returns(5);
        mockHand.Setup(h => h.Contains(card)).Returns(true);

        mockGameState.Setup(gs => gs.ActivePlayer).Returns(mockOtherPlayer.Object); // Not the owner
        mockGameState.Setup(gs => gs.Players).Returns(new List<IPlayer> { mockPlayer.Object, mockOtherPlayer.Object });

        // Act
        bool isCastable = card.IsCastable(mockGameState.Object);

        // Assert
        Assert.False(isCastable);
    }

    [Fact]
    public void IsCastable_ReturnsFalse_WhenNotEnoughMana()
    {
        // Arrange
        var mockPlayer = new Mock<IPlayer>();
        var mockHand = new Mock<IHand>();
        var mockGameState = new Mock<IGameState>();
        var mockManaPoolStat = new Mock<IManaful>();

        var card = new TestCard(mockPlayer.Object) { Name = "Test Card" };
        card.Components.Add(new CardComponent(5, 0)); // Card costs 5 mana
        mockPlayer.Setup(p => p.AllCards).Returns([card]);
        mockHand.Setup(h => h.AllCards).Returns([card]);
        mockPlayer.Setup(p => p.Hand).Returns(mockHand.Object);
        mockPlayer.Setup(p => p.ManaValue).Returns(3); // Player has only 3 mana
        mockHand.Setup(h => h.Contains(card)).Returns(true);

        mockGameState.Setup(gs => gs.ActivePlayer).Returns(mockPlayer.Object);
        mockGameState.Setup(gs => gs.Players).Returns(new List<IPlayer> { mockPlayer.Object });

        // Act
        bool isCastable = card.IsCastable(mockGameState.Object);

        // Assert
        Assert.False(isCastable);
    }

    [Fact]
    public void IsCastable_ReturnsFalse_WhenCardNotInHand()
    {
        // Arrange
        var mockPlayer = new Mock<IPlayer>();
        var mockHand = new Mock<IHand>();
        var mockGameState = new Mock<IGameState>();
        var mockManaPoolStat = new Mock<IManaful>();

        var card = new TestCard(mockPlayer.Object) { Name = "Test Card" };
        card.Components.Add(new CardComponent(3, 0));
        mockPlayer.Setup(p => p.AllCards).Returns([card]);
        mockPlayer.Setup(p => p.Hand).Returns(mockHand.Object);
        mockPlayer.Setup(p => p.ManaValue).Returns(5);
        mockHand.Setup(h => h.Contains(card)).Returns(false); // Card not in hand

        mockGameState.Setup(gs => gs.ActivePlayer).Returns(mockPlayer.Object);
        mockGameState.Setup(gs => gs.Players).Returns(new List<IPlayer> { mockPlayer.Object });

        // Act
        bool isCastable = card.IsCastable(mockGameState.Object);

        // Assert
        Assert.False(isCastable);
    }

    [Fact]
    public void FindParentCard_ReturnsSelf()
    {
        // Arrange
        var mockPlayer = new Mock<IPlayer>();
        var mockGameState = new Mock<IGameState>();
        var card = new TestCard(mockPlayer.Object);

        // Act
        var parentCard = card.FindParentCard(mockGameState.Object);

        // Assert
        Assert.Same(card, parentCard);
    }

    [Fact]
    public void FindParentPlayer_ReturnsCorrectPlayer_WhenCardIsOwnedByAPlayer()
    {
        // Arrange
        var mockPlayer1 = new Mock<IPlayer>();
        mockPlayer1.Setup(p => p.AllCards).Returns(new List<ICard> { new TestCard(mockPlayer1.Object) });
        var mockPlayer2 = new Mock<IPlayer>();
        var card = new TestCard(mockPlayer1.Object) { Name = "Test Card" };
        mockPlayer1.Setup(p => p.AllCards).Returns(new List<ICard> { card }); // Ensure the card is in AllCards for the owner

        var mockGameState = new Mock<IGameState>();
        mockGameState.Setup(gs => gs.Players).Returns(new List<IPlayer> { mockPlayer1.Object, mockPlayer2.Object });

        // Act
        var parentPlayer = card.FindParentPlayer(mockGameState.Object);

        // Assert
        Assert.Same(mockPlayer1.Object, parentPlayer);
    }

    [Fact]
    public void FindParentPlayer_ThrowsException_WhenPlayerNotFound()
    {
        // Arrange
        var mockPlayer1 = new Mock<IPlayer>();
        mockPlayer1.Setup(p => p.AllCards).Returns(new List<ICard>()); // Player1 does not contain the card
        var mockPlayer2 = new Mock<IPlayer>();
        mockPlayer2.Setup(p => p.AllCards).Returns(new List<ICard>()); // Player2 does not contain the card
        var card = new TestCard(new Mock<IPlayer>().Object) { Name = "Test Card" }; // Card has an owner, but not in any player's AllCards list

        var mockGameState = new Mock<IGameState>();
        mockGameState.Setup(gs => gs.Players).Returns(new List<IPlayer> { mockPlayer1.Object, mockPlayer2.Object });

        // Act & Assert
        var exception = Assert.Throws<CardGameEngineException>(() => card.FindParentPlayer(mockGameState.Object));
        Assert.Contains("Player not found for card", exception.Message);
    }

    [Fact]
    public void Clone_ShouldReturnNewInstanceWithSameValues()
    {
        // Arrange
        var mockPlayer = new Mock<IPlayer>();
        var mockReaction = new Mock<IReaction>();
        mockReaction.Setup(r => r.Clone()).Returns(new Mock<IReaction>().Object);
        var cardComponent = new CardComponent(1, 1);

        var original = new TestCard(new List<ICardComponent> { cardComponent }, new List<IReaction> { mockReaction.Object }, mockPlayer.Object, "Original Card");

        // Act
        var clone = (TestCard)original.Clone();

        // Assert
        Assert.NotSame(original, clone);
        Assert.Equal(original.Name, clone.Name);
        Assert.Same(original.Owner, clone.Owner); // Owner is typically a reference, not deep cloned
        Assert.Equal(original.Components.Count, clone.Components.Count);
        Assert.NotSame(original.Components[0], clone.Components[0]); // Components should be cloned
        Assert.Equal(original.Components[0].ManaValue, clone.Components[0].ManaValue);
        Assert.Equal(original.Reactions.Count, clone.Reactions.Count);
        Assert.NotSame(original.Reactions[0], clone.Reactions[0]); // Reactions should be cloned
    }
}
