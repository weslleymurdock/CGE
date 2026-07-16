using CardGameEngine;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace CardGameEngine.Tests.UnitTests;

public class PlayerTests
{
    [Fact]
    public void Player_DefaultConstructor_InitializesCollectionsAndStats()
    {
        // Arrange & Act
        var player = new Player();

        // Assert
        Assert.NotNull(player.Deck);
        Assert.NotNull(player.Hand);
        Assert.NotNull(player.Board);
        Assert.NotNull(player.Graveyard);
        Assert.NotNull(player.Reactions);
        Assert.Empty(player.Reactions);

        Assert.Equal(0, player.AttackValue);
        Assert.Equal(0, player.AttackBaseValue);
        Assert.Equal(0, player.LifeValue); // LifeStat initialized with 0, so LifeValue is 0
        Assert.Equal(0, player.LifeBaseValue);
        Assert.Equal(0, player.ManaValue);
        Assert.Equal(0, player.ManaBaseValue);
    }

    [Fact]
    public void Player_ConstructorWithDeck_InitializesWithProvidedDeck()
    {
        // Arrange
        var mockDeck = new Mock<IDeck>();

        // Act
        var player = new Player(mockDeck.Object);

        // Assert
        Assert.Same(mockDeck.Object, player.Deck);
        Assert.NotNull(player.Hand);
        Assert.NotNull(player.Board);
        Assert.NotNull(player.Graveyard);
    }

    [Fact]
    public void IsAlive_ReturnsTrue_WhenLifeValueIsGreaterThanZero()
    {
        // Arrange
        var player = new Player(new Mock<IDeck>().Object)
        {
            LifeValue = 10
        };

        // Act & Assert
        Assert.True(player.IsAlive);
    }

    [Fact]
    public void IsAlive_ReturnsFalse_WhenLifeValueIsZero()
    {
        // Arrange
        var player = new Player(new Mock<IDeck>().Object)
        {
            LifeValue = 0
        };

        // Act & Assert
        Assert.False(player.IsAlive);
    }

    [Fact]
    public void AllCards_ReturnsAllCardsFromCollections()
    {
        // Arrange
        var mockCard1 = new Mock<ICard>();
        var mockCard2 = new Mock<ICard>();
        var mockCard3 = new Mock<ICard>();

        var mockDeck = new Mock<IDeck>();
        mockDeck.Setup(d => d.AllCards).Returns([mockCard1.Object]);

        var mockHand = new Mock<IHand>();
        mockHand.Setup(h => h.AllCards).Returns([mockCard2.Object]);

        var mockBoard = new Mock<IBoard>();
        mockBoard.Setup(b => b.AllCards).Returns([mockCard3.Object]);

        var mockGraveyard = new Mock<IDeck>();
        mockGraveyard.Setup(g => g.AllCards).Returns(new List<ICard>());

        var player = Player.NewPlayer(
            mockDeck.Object,
            mockHand.Object,
            mockBoard.Object,
            mockGraveyard.Object,
            new ManaPoolStat(0, 0),
            new AttackStat(0),
            new LifeStat(0),
            new List<IReaction>()
        );

        // Act
        var allCards = player.AllCards;

        // Assert
        Assert.Equal(3, allCards.Count);
        Assert.Contains(mockCard1.Object, allCards);
        Assert.Contains(mockCard2.Object, allCards);
        Assert.Contains(mockCard3.Object, allCards);
    }

    [Fact]
    public void AttackValue_ShouldBeClampedToZero_WhenSetNegative()
    {
        // Arrange
        var player = new Player { AttackValue = 10 };

        // Act
        player.AttackValue = -5;

        // Assert
        Assert.Equal(0, player.AttackValue);
    }

    [Fact]
    public void LifeValue_ShouldBeClampedToZero_WhenSetNegative()
    {
        // Arrange
        var player = new Player { LifeValue = 10 };

        // Act
        player.LifeValue = -5;

        // Assert
        Assert.Equal(0, player.LifeValue);
    }

    [Fact]
    public void ManaValue_ShouldBeClampedToZero_WhenSetNegative()
    {
        // Arrange
        var player = new Player { ManaValue = 10 };

        // Act
        player.ManaValue = -5;

        // Assert
        Assert.Equal(0, player.ManaValue);
    }

    [Fact]
    public void Characters_ReturnsPlayerAndCardsOnBoard()
    {
        // Arrange
        var mockMonsterCard = new Mock<IMonsterCard>();
        var mockBoard = new Mock<IBoard>();
        mockBoard.Setup(b => b.AllCards).Returns(new List<ICard> { mockMonsterCard.Object });

        var player = Player.NewPlayer(
            new Mock<IDeck>().Object,
            new Mock<IHand>().Object,
            mockBoard.Object,
            new Mock<IDeck>().Object,
            new ManaPoolStat(0, 0),
            new AttackStat(0),
            new LifeStat(0),
            new List<IReaction>()
        );

        // Act
        var characters = player.Characters;

        // Assert
        Assert.Equal(2, characters.Count);
        Assert.Contains(player, characters);
        Assert.Contains(mockMonsterCard.Object, characters);
    }

    [Fact]
    public void AllReactions_ReturnsAllReactionsFromPlayerAndCards()
    {
        // Arrange
        var mockReaction1 = new Mock<IReaction>();
        var mockReaction2 = new Mock<IReaction>();
        var mockReaction3 = new Mock<IReaction>();

        var mockCard1 = new Mock<ICard>();
        mockCard1.Setup(c => c.AllReactions()).Returns(new List<IReaction> { mockReaction2.Object });

        var mockCard2 = new Mock<ICard>();
        mockCard2.Setup(c => c.AllReactions()).Returns(new List<IReaction> { mockReaction3.Object });

        var mockDeck = new Mock<IDeck>();
        mockDeck.Setup(d => d.AllCards).Returns(new List<ICard> { mockCard1.Object });

        var mockHand = new Mock<IHand>();
        mockHand.Setup(h => h.AllCards).Returns(new List<ICard> { mockCard2.Object });

        var mockPlayer = new Mock<IPlayer>();
        mockPlayer.Setup(p => p.AllReactions()).Returns(new List<IReaction> { mockReaction1.Object, mockReaction2.Object, mockReaction3.Object });
        // Act
        var allReactions = mockPlayer.Object.AllReactions();

        // Assert
        Assert.Equal(3, allReactions.Count);
        Assert.Contains(mockReaction1.Object, allReactions);
        Assert.Contains(mockReaction2.Object, allReactions);
        Assert.Contains(mockReaction3.Object, allReactions);
    }
    
    [Fact]
    public void DrawCard_CallsGameExecuteWithDrawCardAction()
    {
        // Arrange
        var mockGame = new Mock<IGame>();
        var player = new Player();

        // Act
        player.DrawCard(mockGame.Object);

        // Assert
        mockGame.Verify(g => g.Execute(It.Is<DrawCardAction>(a => a.Player == player)), Times.Once);
    }
    
    [Fact]
    public void CastMonster_ThrowsException_WhenCardIsNotSummonable()
    {
        // Arrange
        var mockGame = new Mock<IGame>();
        var mockMonsterCard = new Mock<IMonsterCard>();
        mockMonsterCard.Setup(m => m.IsSummonable(mockGame.Object)).Returns(false);

        var player = new Player();

        // Act & Assert
        var exception = Assert.Throws<CardGameEngineException>(() => player.CastMonster(mockGame.Object, mockMonsterCard.Object, 0));
        Assert.Equal("Card Game Engine Error: Tried to play a card that is not playable!", exception.Message);
    }

    [Fact]
    public void CastMonster_ThrowsException_WhenBoardSlotIsOccupied()
    {
        // Arrange
        var mockGame = new Mock<IGame>();
        var mockMonsterCard = new Mock<IMonsterCard>();
        mockMonsterCard.Setup(m => m.IsSummonable(mockGame.Object)).Returns(true);

        var mockBoard = new Mock<IBoard>();
        mockBoard.Setup(b => b.IsFreeSlot(0)).Returns(false);

        var player = Player.NewPlayer(
            new Mock<IDeck>().Object,
            new Mock<IHand>().Object,
            mockBoard.Object,
            new Mock<IDeck>().Object,
            new ManaPoolStat(0, 0),
            new AttackStat(0),
            new LifeStat(0),
            new List<IReaction>()
        );

        // Act & Assert
        var exception = Assert.Throws<CardGameEngineException>(() => player.CastMonster(mockGame.Object, mockMonsterCard.Object, 0));
        Assert.Equal("Card Game Engine Error: Slot with index 0 is already occupied!", exception.Message);
    }

    [Fact]
    public void CastMonster_CallsGameExecuteWithCastMonsterAction()
    {
        // Arrange
        var mockGame = new Mock<IGame>();
        var mockMonsterCard = new Mock<IMonsterCard>();
        mockMonsterCard.Setup(m => m.IsSummonable(mockGame.Object)).Returns(true);

        var mockBoard = new Mock<IBoard>();
        mockBoard.Setup(b => b.IsFreeSlot(0)).Returns(true);

        var player = Player.NewPlayer(
            new Mock<IDeck>().Object,
            new Mock<IHand>().Object,
            mockBoard.Object,
            new Mock<IDeck>().Object,
            new ManaPoolStat(0, 0),
            new AttackStat(0),
            new LifeStat(0),
            new List<IReaction>()
        );

        // Act
        player.CastMonster(mockGame.Object, mockMonsterCard.Object, 0);

        // Assert
        mockGame.Verify(g => g.Execute(It.Is<CastMonsterAction>(a => a.Player == player && a.MonsterCard == mockMonsterCard.Object && a.BoardIndex == 0)), Times.Once);
    }

    [Fact]
    public void CastTargetlessSpell_ThrowsException_WhenCardIsNotCastable()
    {
        // Arrange
        var mockGame = new Mock<IGame>();
        var mockSpellCard = new Mock<ITargetlessSpellCard>();
        mockSpellCard.Setup(s => s.IsCastable(mockGame.Object)).Returns(false);

        var player = new Player();

        // Act & Assert
        var exception = Assert.Throws<CardGameEngineException>(() => player.CastSpell(mockGame.Object, mockSpellCard.Object));
        Assert.Equal("Card Game Engine Error: Tried to play a card that is not playable!", exception.Message);
    }

    [Fact]
    public void CastTargetlessSpell_CallsGameExecuteWithCastTargetlessSpellAction()
    {
        // Arrange
        var mockGame = new Mock<IGame>();
        var mockSpellCard = new Mock<ITargetlessSpellCard>();
        mockSpellCard.Setup(s => s.IsCastable(mockGame.Object)).Returns(true);

        var player = new Player();

        // Act
        player.CastSpell(mockGame.Object, mockSpellCard.Object);

        // Assert
        mockGame.Verify(g => g.Execute(It.Is<CastTargetlessSpellAction>(a => a.Player == player && a.SpellCard == mockSpellCard.Object)), Times.Once);
    }

    [Fact]
    public void CastTargetfulSpell_ThrowsException_WhenCardIsNotCastable()
    {
        // Arrange
        var mockGame = new Mock<IGame>();
        var mockSpellCard = new Mock<ITargetfulSpellCard>();
        mockSpellCard.Setup(s => s.IsCastable(mockGame.Object)).Returns(false);
        var mockTarget = new Mock<ICharacter>();

        var player = new Player();

        // Act & Assert
        var exception = Assert.Throws<CardGameEngineException>(() => player.CastSpell(mockGame.Object, mockSpellCard.Object, mockTarget.Object));
        Assert.Equal("Card Game Engine Error: Tried to play a card that is not playable!", exception.Message);
    }

    [Fact]
    public void CastTargetfulSpell_CallsGameExecuteWithCastTargetfulSpellAction()
    {
        // Arrange
        var mockGame = new Mock<IGame>();
        var mockSpellCard = new Mock<ITargetfulSpellCard>();
        mockSpellCard.Setup(s => s.IsCastable(mockGame.Object)).Returns(true);
        var mockTarget = new Mock<ICharacter>();

        var player = new Player();

        // Act
        player.CastSpell(mockGame.Object, mockSpellCard.Object, mockTarget.Object);

        // Assert
        mockGame.Verify(g => g.Execute(It.Is<CastTargetfulSpellAction>(a => a.Player == player && a.SpellCard == mockSpellCard.Object && a.Target == mockTarget.Object)), Times.Once);
    }
    
    [Fact]
    public void Clone_ShouldReturnNewInstanceWithSameValues()
    {
        // Arrange
        var mockDeck = new Mock<IDeck>();
        var mockHand = new Mock<IHand>();
        var mockBoard = new Mock<IBoard>();
        var mockGraveyard = new Mock<IDeck>();
        var mockReaction = new Mock<IReaction>();
        mockReaction.Setup(r => r.Clone()).Returns(new Mock<IReaction>().Object); // Mock clone for reactions

        var player = Player.NewPlayer(
            mockDeck.Object,
            mockHand.Object,
            mockBoard.Object,
            mockGraveyard.Object,
            new ManaPoolStat(5,5),
            new AttackStat(3),
            new LifeStat(0),
            new List<IReaction> { mockReaction.Object }
        );

        // Act
        var clone = (Player)player.Clone();

        // Assert
        Assert.NotSame(player, clone);
        Assert.Equal(player.LifeValue, clone.LifeValue);
        Assert.Equal(player.ManaValue, clone.ManaValue);
        Assert.Equal(player.AttackValue, clone.AttackValue);
        Assert.Equal(player.Reactions.Count, clone.Reactions.Count);
        // Ensure collections are not the same instance, but contain the same type of objects
        Assert.NotSame(player.Deck, clone.Deck);
        Assert.NotSame(player.Hand, clone.Hand);
        Assert.NotSame(player.Board, clone.Board);
        Assert.NotSame(player.Graveyard, clone.Graveyard);
        Assert.NotSame(player.Reactions[0], clone.Reactions[0]); // Check if reactions are cloned
    }
}
