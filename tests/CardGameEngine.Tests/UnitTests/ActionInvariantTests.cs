using CardGameEngine;
using Moq;
using Xunit;

namespace CardGameEngine.Tests.UnitTests;

public class ActionInvariantTests
{
    [Fact]
    public void DrawCardAction_IsNotExecutable_WhenHandIsFull()
    {
        var player = new Mock<IPlayer>();
        var deck = new Mock<IDeck>();
        var hand = new Mock<IHand>();

        deck.SetupGet(d => d.IsEmpty).Returns(false);
        hand.SetupGet(h => h.Size).Returns(10);
        hand.SetupGet(h => h.MaxSize).Returns(10);
        player.SetupGet(p => p.Deck).Returns(deck.Object);
        player.SetupGet(p => p.Hand).Returns(hand.Object);

        var action = new DrawCardAction(player.Object);

        Assert.False(action.IsExecutable(Mock.Of<IGameState>()));
    }

    [Fact]
    public void CastMonsterAction_IsNotExecutable_WhenPlayerIsNotActive()
    {
        var activePlayer = new Mock<IPlayer>();
        var actingPlayer = new Mock<IPlayer>();
        var card = new Mock<IMonsterCard>();
        var hand = new Mock<IHand>();
        var board = new Mock<IBoard>();

        hand.Setup(h => h.Contains(card.Object)).Returns(true);
        board.Setup(b => b.IsFreeSlot(0)).Returns(true);
        card.Setup(c => c.IsSummonable(It.IsAny<IGameState>())).Returns(true);
        actingPlayer.SetupGet(p => p.Hand).Returns(hand.Object);
        actingPlayer.SetupGet(p => p.Board).Returns(board.Object);

        var gameState = new Mock<IGameState>();
        gameState.SetupGet(g => g.ActivePlayer).Returns(activePlayer.Object);

        var action = new CastMonsterAction(actingPlayer.Object, card.Object, 0);

        Assert.False(action.IsExecutable(gameState.Object));
    }

    [Fact]
    public void CastTargetlessSpellAction_IsNotExecutable_WhenSpellIsNotInActingPlayersHand()
    {
        var player = new Mock<IPlayer>();
        var hand = new Mock<IHand>();
        var spell = new Mock<ITargetlessSpellCard>();
        var gameState = new Mock<IGameState>();

        gameState.SetupGet(g => g.ActivePlayer).Returns(player.Object);
        player.SetupGet(p => p.Hand).Returns(hand.Object);
        hand.Setup(h => h.Contains(spell.Object)).Returns(false);
        spell.Setup(s => s.IsCastable(gameState.Object)).Returns(true);

        var action = new CastTargetlessSpellAction(player.Object, spell.Object);

        Assert.False(action.IsExecutable(gameState.Object));
    }

    [Fact]
    public void CastTargetfulSpellAction_IsNotExecutable_WhenTargetIsNotAllowed()
    {
        var player = new Mock<IPlayer>();
        var hand = new Mock<IHand>();
        var spell = new Mock<ITargetfulSpellCard>();
        var target = new Mock<ICharacter>();
        var gameState = new Mock<IGameState>();

        gameState.SetupGet(g => g.ActivePlayer).Returns(player.Object);
        player.SetupGet(p => p.Hand).Returns(hand.Object);
        hand.Setup(h => h.Contains(spell.Object)).Returns(true);
        spell.Setup(s => s.IsCastable(gameState.Object)).Returns(true);
        spell.Setup(s => s.GetPotentialTargets(gameState.Object))
            .Returns(new HashSet<ICharacter>());

        var action = new CastTargetfulSpellAction(player.Object, spell.Object, target.Object);

        Assert.False(action.IsExecutable(gameState.Object));
    }

    [Fact]
    public void AttackAction_IsNotExecutable_WhenAttackerIsNotOnActivePlayersBoard()
    {
        var activePlayer = new Mock<IPlayer>();
        var board = new Mock<IBoard>();
        var attacker = new Mock<IMonsterCard>();
        var target = new Mock<ICharacter>();
        var gameState = new Mock<IGameState>();

        gameState.SetupGet(g => g.ActivePlayer).Returns(activePlayer.Object);
        activePlayer.SetupGet(p => p.Board).Returns(board.Object);
        board.Setup(b => b.Contains(attacker.Object)).Returns(false);
        attacker.SetupGet(a => a.IsReadyToAttack).Returns(true);
        attacker.Setup(a => a.GetPotentialTargets(gameState.Object))
            .Returns(new HashSet<ICharacter> { target.Object });

        var action = new AttackAction(attacker.Object, target.Object);

        Assert.False(action.IsExecutable(gameState.Object));
    }
}
