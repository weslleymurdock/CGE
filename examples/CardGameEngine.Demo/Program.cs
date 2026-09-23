using CardGameEngine;

namespace CardGameEngine.Demo;

internal static class Program
{
    private static void Main()
    {
        var player = new Player();
        var opponent = new Player();

        var monster = new MonsterCard(2, 5, 6, player, "Knight");
        var bolt = new TargetfulSpellCard(
            new LightningComponent(2),
            player,
            "Lightning"
        );
        var heal = new TargetlessSpellCard(
            new HealComponent(1),
            player,
            "Heal"
        );

        var opponentMonster = new MonsterCard(1, 3, 4, opponent, "Goblin");

        // Deck.Pop() returns the top card, so push in reverse draw order.
        player.Deck.Push(new MonsterCard(1, 2, 3, player, "Scout"));
        player.Deck.Push(new MonsterCard(1, 2, 3, player, "Guard"));
        player.Deck.Push(heal);
        player.Deck.Push(bolt);
        player.Deck.Push(monster);

        opponent.Deck.Push(new MonsterCard(1, 2, 3, opponent, "Minion"));
        opponent.Deck.Push(opponentMonster);
        opponent.Deck.Push(new MonsterCard(1, 2, 3, opponent, "Squire"));

        var game = new Game(new List<IPlayer> { player, opponent });
        game.ActivePlayer = player;

        Console.WriteLine("=== CardGameEngine console demo ===");
        Console.WriteLine("Starting game...");
        game.StartGame(initialHandSize: 2, initialPlayerLife: 20);

        Console.WriteLine($"Active player: {PlayerName(game, game.ActivePlayer)}");
        Console.WriteLine($"Player hand: {player.Hand.Size}, deck: {player.Deck.Size}");
        Console.WriteLine($"Player mana: {player.ManaValue}/{player.ManaBaseValue}");

        // Explicit action: increase the available mana for the demonstration.
        game.Execute(new ModifyManaStatAction(player, 4, 4));

        Console.WriteLine("\n1. Cast a monster card");
        player.CastMonster(game, monster, 0);
        PrintState(game, player, opponent);

        Console.WriteLine("\n2. Cast a targetful spell");
        player.CastSpell(game, bolt, opponent);
        PrintState(game, player, opponent);

        Console.WriteLine("\n3. Cast a targetless spell");
        player.CastSpell(game, heal);
        PrintState(game, player, opponent);

        Console.WriteLine("\n4. End the turn and activate the opponent");
        game.NextTurn();
        Console.WriteLine($"Active player: {PlayerName(game, game.ActivePlayer)}");

        Console.WriteLine("\n5. End the opponent turn and reactivate the player");
        game.NextTurn();
        Console.WriteLine($"Active player: {PlayerName(game, game.ActivePlayer)}");

        Console.WriteLine("\n6. Attack with the monster");
        monster.Attack(game, opponent);
        PrintState(game, player, opponent);

        Console.WriteLine("\n7. Clone the game state");
        var clone = (Game)game.Clone();
        Console.WriteLine($"Clone players: {clone.Players.Count}");
        Console.WriteLine($"Clone active player: {PlayerName(clone, clone.ActivePlayer)}");

        Console.WriteLine("\n8. End the game through an action");
        game.Execute(new ModifyLifeStatAction(opponent, -opponent.LifeValue));
        Console.WriteLine($"Opponent alive: {opponent.IsAlive}");

        Console.WriteLine("\n9. Attempt an action after game over");
        var deckBefore = player.Deck.Size;
        game.Execute(new DrawCardAction(player));
        Console.WriteLine($"Deck unchanged after game over: {deckBefore == player.Deck.Size}");

        Console.WriteLine("\nDemo completed.");
    }

    private static void PrintState(Game game, IPlayer player, IPlayer opponent)
    {
        Console.WriteLine($"Player: life={player.LifeValue}, mana={player.ManaValue}, hand={player.Hand.Size}, board={player.Board.Size}, graveyard={player.Graveyard.Size}");
        Console.WriteLine($"Opponent: life={opponent.LifeValue}, mana={opponent.ManaValue}, hand={opponent.Hand.Size}, board={opponent.Board.Size}, graveyard={opponent.Graveyard.Size}");
        Console.WriteLine($"All cards on board: {game.AllCardsOnTheBoard.Count}");
    }

    private static string PlayerName(Game game, IPlayer player) =>
        player == null ? "<none>" : $"Player {game.Players.IndexOf(player) + 1}";
}

internal sealed class HealComponent : TargetlessSpellCardComponent
{
    public HealComponent(int mana) : base(mana)
    {
    }

    public override void Cast(IGame game)
    {
        game.Execute(new ModifyLifeStatAction(game.ActivePlayer, 3));
    }

    public override object Clone() => new HealComponent(ManaValue);
}

internal sealed class LightningComponent : TargetfulSpellCardComponent
{
    public LightningComponent(int mana) : base(mana)
    {
    }

    public override void Cast(IGame game, ICharacter target)
    {
        game.Execute(new ModifyLifeStatAction(target, -4));
    }

    public override HashSet<ICharacter> GetPotentialTargets(IGameState gameState)
    {
        var targets = new HashSet<ICharacter>();

        foreach (var opponent in gameState.NonActivePlayers)
        {
            foreach (var character in opponent.Characters)
            {
                targets.Add(character);
            }
        }

        return targets;
    }

    public override object Clone() => new LightningComponent(ManaValue);
}
