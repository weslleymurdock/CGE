using CardGameEngine;
using Moq;
using Xunit;

namespace CardGameEngine.Tests.UnitTests;

public class CompoundMonsterCardTests
{
    [Fact]
    public void IsSummonable_ReturnsTrue_WhenCardIsInActivePlayersHandAndBoardHasSpace()
    {
        var player = new Player();
        player.ManaValue = 20;

        var first = new MonsterCard(2, 2, 2, player, "First");
        var second = new MonsterCard(2, 3, 3, player, "Second");
        var compound = new CompoundMonsterCard([first, second]);
        player.Hand.Add(compound);

        var gameState = new Mock<IGameState>();
        gameState.SetupGet(x => x.ActivePlayer).Returns(player);
        gameState.SetupGet(x => x.Players).Returns([player]);

        Assert.True(compound.IsSummonable(gameState.Object));
    }

    [Fact]
    public void GetPotentialTargets_ReturnsIntersectionOfComponentTargets()
    {
        var opponent = new Mock<IPlayer>();
        var target = new Mock<ICharacter>();
        opponent.SetupGet(x => x.Characters).Returns([target.Object]);

        var gameState = new Mock<IGameState>();
        gameState.SetupGet(x => x.NonActivePlayers).Returns([opponent.Object]);

        var first = new MonsterCard(1, 1, 1);
        var second = new MonsterCard(1, 1, 1);
        var compound = new CompoundMonsterCard([first, second]);

        var targets = compound.GetPotentialTargets(gameState.Object);

        Assert.Contains(target.Object, targets);
    }
}
