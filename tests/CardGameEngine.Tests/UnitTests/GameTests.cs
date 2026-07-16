using CardGameEngine;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace CardGameEngine.Tests.UnitTests;

public class GameTests
{
    [Fact]
    public void Game_DefaultConstructor_InitializesEmptyPlayersAndReactions()
    {
        // Arrange & Act
        var game = new Game();

        // Assert
        Assert.NotNull(game.Players);
        Assert.Empty(game.Players);
        Assert.NotNull(game.Reactions);
        Assert.NotEmpty(game.Reactions);
        Assert.Equal(3, game.Reactions.Count); // Default reactions added
    }

    [Fact]
    public void Game_ConstructorWithPlayers_InitializesPlayersAndReactions()
    {
        // Arrange
        var mockPlayer1 = new Mock<IPlayer>();
        var mockPlayer2 = new Mock<IPlayer>();
        var players = new List<IPlayer> { mockPlayer1.Object, mockPlayer2.Object };

        // Act
        var game = new Game(players);

        // Assert
        Assert.Equal(2, game.Players.Count);
        Assert.Contains(mockPlayer1.Object, game.Players);
        Assert.Contains(mockPlayer2.Object, game.Players);
        Assert.NotNull(game.Reactions);
        Assert.NotEmpty(game.Reactions);
    }

    [Fact]
    public void ActivePlayer_ReturnsCorrectPlayer()
    {
        // Arrange
        var mockPlayer1 = new Mock<IPlayer>();
        var mockPlayer2 = new Mock<IPlayer>();
        var players = new List<IPlayer> { mockPlayer1.Object, mockPlayer2.Object };
        var game = new Game(players);

        // Note: Active player index is random in constructor, so we set it directly for predictable testing
        game.ActivePlayer = mockPlayer1.Object;

        // Act & Assert
        Assert.Same(mockPlayer1.Object, game.ActivePlayer);
    }

    [Fact]
    public void ActivePlayer_Set_ChangesActivePlayerIndex()
    {
        // Arrange
        var mockPlayer1 = new Mock<IPlayer>();
        var mockPlayer2 = new Mock<IPlayer>();
        var players = new List<IPlayer> { mockPlayer1.Object, mockPlayer2.Object };
        var game = new Game(players);

        // Act
        game.ActivePlayer = mockPlayer2.Object;

        // Assert
        Assert.Same(mockPlayer2.Object, game.ActivePlayer);
    }

    [Fact]
    public void NonActivePlayers_ReturnsAllPlayersExceptActivePlayer()
    {
        // Arrange
        var mockPlayer1 = new Mock<IPlayer>();
        var mockPlayer2 = new Mock<IPlayer>();
        var mockPlayer3 = new Mock<IPlayer>();
        var players = new List<IPlayer> { mockPlayer1.Object, mockPlayer2.Object, mockPlayer3.Object };
        var game = new Game(players);
        game.ActivePlayer = mockPlayer1.Object; // Set Player1 as active

        // Act
        var nonActivePlayers = game.NonActivePlayers;

        // Assert
        Assert.Equal(2, nonActivePlayers.Count);
        Assert.Contains(mockPlayer2.Object, nonActivePlayers);
        Assert.Contains(mockPlayer3.Object, nonActivePlayers);
        Assert.DoesNotContain(mockPlayer1.Object, nonActivePlayers);
    }

    [Fact]
    public void AllCards_ReturnsAllCardsFromAllPlayers()
    {
        // Arrange
        var mockCard1 = new Mock<ICard>();
        var mockCard2 = new Mock<ICard>();
        var mockCard3 = new Mock<ICard>();

        var mockPlayer1 = new Mock<IPlayer>();
        mockPlayer1.Setup(p => p.AllCards).Returns(new List<ICard> { mockCard1.Object, mockCard2.Object });

        var mockPlayer2 = new Mock<IPlayer>();
        mockPlayer2.Setup(p => p.AllCards).Returns(new List<ICard> { mockCard3.Object });

        var players = new List<IPlayer> { mockPlayer1.Object, mockPlayer2.Object };
        var game = new Game(players);

        // Act
        var allCards = game.AllCards;

        // Assert
        Assert.Equal(3, allCards.Count);
        Assert.Contains(mockCard1.Object, allCards);
        Assert.Contains(mockCard2.Object, allCards);
        Assert.Contains(mockCard3.Object, allCards);
    }

    [Fact]
    public void AllCardsOnTheBoard_ReturnsAllCardsFromAllPlayersBoards()
    {
        // Arrange
        var mockCard1 = new Mock<ICard>();
        var mockCard2 = new Mock<ICard>();

        var mockBoard1 = new Mock<IBoard>();
        mockBoard1.Setup(b => b.AllCards).Returns(new List<ICard> { mockCard1.Object });
        var mockPlayer1 = new Mock<IPlayer>();
        mockPlayer1.Setup(p => p.Board).Returns(mockBoard1.Object);

        var mockBoard2 = new Mock<IBoard>();
        mockBoard2.Setup(b => b.AllCards).Returns(new List<ICard> { mockCard2.Object });
        var mockPlayer2 = new Mock<IPlayer>();
        mockPlayer2.Setup(p => p.Board).Returns(mockBoard2.Object);

        var players = new List<IPlayer> { mockPlayer1.Object, mockPlayer2.Object };
        var game = new Game(players);

        // Act
        var allCardsOnBoard = game.AllCardsOnTheBoard;

        // Assert
        Assert.Equal(2, allCardsOnBoard.Count);
        Assert.Contains(mockCard1.Object, allCardsOnBoard);
        Assert.Contains(mockCard2.Object, allCardsOnBoard);
    }

    [Fact]
    public void AllReactions_ReturnsAllReactionsFromGameAndPlayers()
    {
        // Arrange
        var mockReaction1 = new Mock<IReaction>(); // Game reaction
        var mockReaction2 = new Mock<IReaction>(); // Player1 reaction
        var mockReaction3 = new Mock<IReaction>(); // Player2 reaction

        var mockPlayer1 = new Mock<IPlayer>();
        mockPlayer1.Setup(p => p.AllReactions()).Returns(new List<IReaction> { mockReaction2.Object });

        var mockPlayer2 = new Mock<IPlayer>();
        mockPlayer2.Setup(p => p.AllReactions()).Returns(new List<IReaction> { mockReaction3.Object });

        var players = new List<IPlayer> { mockPlayer1.Object, mockPlayer2.Object };
        var game = new Game(players);
        game.Reactions.Clear(); // Clear default reactions for this test
        game.Reactions.Add(mockReaction1.Object);

        // Act
        var allReactions = game.AllReactions();

        // Assert
        Assert.Equal(3, allReactions.Count);
        Assert.Contains(mockReaction1.Object, allReactions);
        Assert.Contains(mockReaction2.Object, allReactions);
        Assert.Contains(mockReaction3.Object, allReactions);
    }

    [Fact]
    public void StartGame_InitializesPlayersAndExecutesEvents()
    {
        // Arrange
        var mockPlayer1 = new Mock<IPlayer>();
        var mockPlayer2 = new Mock<IPlayer>();
        var players = new List<IPlayer> { mockPlayer1.Object, mockPlayer2.Object };
        var mockActionQueue = new Mock<ActionQueue>(false);
        var cloneMock = new Mock<ActionQueue>(false);
        mockActionQueue.Setup(aq => aq.Clone()).Returns(cloneMock.Object);
        var game = new Game(players, 0, mockActionQueue.Object, new List<IReaction>()); // Inject mocked ActionQueue
        
        // Act
        game.StartGame(initialHandSize: 2, initialPlayerLife: 25);

        // Assert
        mockPlayer1.VerifySet(p => p.ManaValue = 0, Times.Once);
        mockPlayer1.VerifySet(p => p.ManaBaseValue = 0, Times.Once);
        mockPlayer1.VerifySet(p => p.LifeValue = 25, Times.Once);
        mockPlayer1.VerifySet(p => p.LifeBaseValue = 25, Times.Once);
        mockPlayer1.Verify(p => p.DrawCard(game), Times.Exactly(2));

        mockPlayer2.VerifySet(p => p.ManaValue = 0, Times.Once);
        mockPlayer2.VerifySet(p => p.ManaBaseValue = 0, Times.Once);
        mockPlayer2.VerifySet(p => p.LifeValue = 25, Times.Once);
        mockPlayer2.VerifySet(p => p.LifeBaseValue = 25, Times.Once);
        mockPlayer2.Verify(p => p.DrawCard(game), Times.Exactly(2));

        // Verify that StartOfGameEvent and StartOfTurnEvent are executed
        mockActionQueue.Verify(aq => aq.Execute(game, It.IsAny<StartOfGameEvent>()), Times.Once);
        mockActionQueue.Verify(aq => aq.Execute(game, It.IsAny<StartOfTurnEvent>()), Times.Once);
        Assert.True(mockActionQueue.Object.ExecuteReactions); // Reactions should be enabled after setup
    }

    [Fact]
    public void NextTurn_ExecutesEndOfTurnAndStartOfTurnEvents()
    {
        // Arrange
        var mockPlayer = new Mock<IPlayer>();
        var players = new List<IPlayer> { mockPlayer.Object };
        var cloneMock = new Mock<ActionQueue>(false);
        var mockActionQueue = new Mock<ActionQueue>(false);
        mockActionQueue.Setup(aq => aq.Clone()).Returns(cloneMock.Object);
        var game = new Game(players, 0, (ActionQueue)mockActionQueue.Object, new List<IReaction>());

        // Act
        game.NextTurn();

        // Assert
        mockActionQueue.Verify(aq => aq.Execute(game, It.IsAny<EndOfTurnEvent>()), Times.Once);
        mockActionQueue.Verify(aq => aq.Execute(game, It.IsAny<StartOfTurnEvent>()), Times.Once);
    }

    [Fact]
    public void Execute_Action_CallsActionQueueExecute()
    {
        // Arrange
        var mockAction = new Mock<IAction>();
        var mockPlayer = new Mock<IPlayer>();
        var players = new List<IPlayer> { mockPlayer.Object };
        var mockActionQueue = new Mock<ActionQueue>(false);
        var cloneMock = new Mock<ActionQueue>(false); 
        mockActionQueue.Setup(aq => aq.Clone()).Returns(cloneMock.Object);
        var game = new Game(players, 0, mockActionQueue.Object, new List<IReaction>());

        // Act
        game.Execute(mockAction.Object);

        // Assert
        mockActionQueue.Verify(aq => aq.Execute(game, mockAction.Object), Times.Once);
    }

    [Fact]
    public void Execute_ListOfActions_CallsActionQueueExecuteForEachAction()
    {
        // Arrange
        var mockAction1 = new Mock<IAction>();
        var mockAction2 = new Mock<IAction>();
        var actions = new List<IAction> { mockAction1.Object, mockAction2.Object };
        var mockPlayer = new Mock<IPlayer>();
        var players = new List<IPlayer> { mockPlayer.Object };
        var mockActionQueue = new Mock<ActionQueue>(false);  
        var cloneMock = new Mock<ActionQueue>(false); 
        mockActionQueue.Setup(aq => aq.Clone()).Returns(cloneMock.Object);
        var game = new Game(players, 0, mockActionQueue.Object, new List<IReaction>());

        // Act
        game.Execute(actions);

        // Assert
        mockActionQueue.Verify(aq => aq.Execute(game, mockAction1.Object), Times.Once);
        mockActionQueue.Verify(aq => aq.Execute(game, mockAction2.Object), Times.Once);
    }

    [Fact]
    public void ReactTo_CallsAllReactionsReactTo()
    {
        // Arrange
        var mockReaction1 = new Mock<IReaction>();
        var mockReaction2 = new Mock<IReaction>();
        var mockPlayer = new Mock<IPlayer>();
        mockPlayer.Setup(p => p.AllReactions()).Returns(new List<IReaction> { mockReaction2.Object });

        var players = new List<IPlayer> { mockPlayer.Object };
        var game = new Game(players);
        game.Reactions.Clear(); // Clear default reactions for this test
        game.Reactions.Add(mockReaction1.Object);

        var mockActionEvent = new Mock<IActionEvent>();

        // Act
        game.ReactTo(game, mockActionEvent.Object);

        // Assert
        mockReaction1.Verify(r => r.ReactTo(game, mockActionEvent.Object), Times.Once);
        mockReaction2.Verify(r => r.ReactTo(game, mockActionEvent.Object), Times.Once);
    }

    [Fact]
    public void Clone_ReturnsNewInstanceWithClonedProperties()
    {
        // Arrange
        var mockPlayer1 = new Mock<IPlayer>();
        mockPlayer1.Setup(p => p.Clone()).Returns(new Mock<IPlayer>().Object);
        var mockPlayer2 = new Mock<IPlayer>();
        mockPlayer2.Setup(p => p.Clone()).Returns(new Mock<IPlayer>().Object);
        var originalPlayers = new List<IPlayer> { mockPlayer1.Object, mockPlayer2.Object };

        var mockReaction = new Mock<IReaction>();
        mockReaction.Setup(r => r.Clone()).Returns(new Mock<IReaction>().Object);
        var originalReactions = new List<IReaction> { mockReaction.Object };

        var mockActionQueue = new Mock<ActionQueue>(false);  
        var cloneMock = new Mock<ActionQueue>(false);
         
        mockActionQueue.Setup(aq => aq.Clone()).Returns(cloneMock.Object);
        var originalGame = new Game(originalPlayers, 0, (ActionQueue)mockActionQueue.Object, originalReactions);

        // Act
        var clonedGame = (Game)originalGame.Clone();

        // Assert
        Assert.NotSame(originalGame, clonedGame);
        Assert.Equal(originalGame.Players.Count, clonedGame.Players.Count);
        Assert.NotSame(originalGame.Players[0], clonedGame.Players[0]); // Players should be cloned
        Assert.NotSame(originalGame.Reactions[0], clonedGame.Reactions[0]); // Reactions should be cloned
        Assert.NotSame(originalGame.Reactions, clonedGame.Reactions);
    }

    [Fact]
    public void FindParentCard_ThrowsCardGameEngineException()
    {
        // Arrange
        var game = new Game();
        var mockGameState = new Mock<IGameState>();

        // Act & Assert
        var exception = Assert.Throws<CardGameEngineException>(() => game.FindParentCard(mockGameState.Object));
        Assert.Contains("Cannot use method 'FindParentCard' on instance of type 'Game'", exception.Message);
    }

    [Fact]
    public void FindParentPlayer_ThrowsCardGameEngineException()
    {
        // Arrange
        var game = new Game();
        var mockGameState = new Mock<IGameState>();

        // Act & Assert
        var exception = Assert.Throws<CardGameEngineException>(() => game.FindParentPlayer(mockGameState.Object));
        Assert.Contains("Cannot use method 'FindParentPlayer' on instance of type 'Game'", exception.Message);
    }
}
