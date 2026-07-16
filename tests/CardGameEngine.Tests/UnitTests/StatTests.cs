using CardGameEngine;

namespace CardGameEngine.Tests.UnitTests;

public class StatTests
{
    [Fact]
    public void Stat_Value_ShouldBeClamped()
    {
        // Arrange
        var attackStat = new AttackStat(10);

        // Act
        attackStat.Value = 200; // Acima do GlobalMax (99)

        // Assert
        Assert.Equal(Stat.GlobalMax, attackStat.Value);

        // Act
        attackStat.Value = -200; // Abaixo do GlobalMin (-99)

        // Assert
        Assert.Equal(Stat.GlobalMin, attackStat.Value);
    }

    [Fact]
    public void ManaPoolStat_Value_ShouldNotBeNegative()
    {
        // Arrange
        var manaStat = new ManaPoolStat(5, 10);

        // Act
        manaStat.Value = -5;

        // Assert
        Assert.Equal(0, manaStat.Value);
    }

    [Fact]
    public void AttackStat_Clone_ShouldReturnNewInstanceWithSameValues()
    {
        // Arrange
        var original = new AttackStat(5, 7);

        // Act
        var clone = (AttackStat)original.Clone();

        // Assert
        Assert.NotSame(original, clone);
        Assert.Equal(original.Value, clone.Value);
        Assert.Equal(original.BaseValue, clone.BaseValue);
    }

    [Fact]
    public void LifeStat_Clone_ShouldReturnNewInstanceWithSameValues()
    {
        // Arrange
        var original = new LifeStat(30);

        // Act
        var clone = (LifeStat)original.Clone();

        // Assert
        Assert.NotSame(original, clone);
        Assert.Equal(original.Value, clone.Value);
        Assert.Equal(original.BaseValue, clone.BaseValue);
    }
}
