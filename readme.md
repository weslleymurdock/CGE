# CardGameEngine

CardGameEngine (CGE) is a .NET 10 framework for implementing turn-based card games in C#.

This document describes the API that exists in the `fix/misc` branch and is intended to be merged into `main`. Examples below use only types and members present in that implementation.

## Requirements

- .NET 10 SDK
- A .NET application that can reference the `CardGameEngine` NuGet package

## Install the package

The repository is prepared to consume the published `CardGameEngine 1.0.0` package:

```bash
dotnet add package CardGameEngine --version 1.0.0
```

Or:

```xml
<PackageReference Include="CardGameEngine" Version="1.0.0" />
```

## The execution model

CGE represents a game as a mutable `Game` state. State transitions are performed through `IAction` instances passed to `Game.Execute(...)`.

An action has two important methods:

- `IsExecutable(IGameState)` — verifies that the action is still valid for the current state.
- `Execute(IGame)` — performs the transition by issuing engine actions.

The action queue checks `IsExecutable` immediately before execution. If an action is not executable, it is not executed and its action reactions are not triggered.

```csharp
game.Execute(new DrawCardAction(player));
```

Do not assume that directly changing an engine-owned collection is equivalent to executing an action. The built-in game operations use the action pipeline.

## Game and players

A game is created from a list of players:

```csharp
using CardGameEngine;

var player = new Player();
var opponent = new Player();

var game = new Game(new List<IPlayer>
{
    player,
    opponent
});

game.ActivePlayer = player;
```

`Game` exposes:

- `Players`
- `ActivePlayer`
- `NonActivePlayers`
- `AllCards`
- `AllCardsOnTheBoard`
- `StartGame(...)`
- `NextTurn()`
- `Execute(IAction)`
- `Clone()`

Start a game with initial hand size and life:

```csharp
game.StartGame(
    initialHandSize: 2,
    initialPlayerLife: 20);
```

During `StartGame`, CGE initializes player life and mana, draws the initial hands, then executes `StartOfGameEvent` and `StartOfTurnEvent`. The built-in game reactions handle turn-start behavior such as drawing a card and updating mana.

## Player collections

Each `Player` owns:

- `Deck`
- `Hand`
- `Board`
- `Graveyard`

A `Deck` is a stack. Cards are inserted with `Push` and drawn with `Pop`.

```csharp
player.Deck.Push(new MonsterCard(1, 2, 3, player, "Scout"));
player.Deck.Push(new MonsterCard(2, 5, 6, player, "Knight"));
```

Because `Pop()` removes the top card, the console demo pushes cards in reverse of the desired draw order.

A `Hand` has a fixed maximum size of 10 cards. `DrawCardAction.IsExecutable` verifies both that the deck is not empty and that the hand has capacity.

## Monster cards

`MonsterCard` represents a card that can be placed on a player's board.

The public constructor accepts mana, attack, life, owner and an optional name:

```csharp
var knight = new MonsterCard(
    mana: 2,
    attack: 5,
    life: 6,
    owner: player,
    name: "Knight");
```

A game-specific card can derive from it:

```csharp
public sealed class KnightCard : MonsterCard
{
    public KnightCard(IPlayer owner)
        : base(2, 5, 6, owner, "Knight")
    {
    }
}
```

A monster is played with:

```csharp
player.CastMonster(game, knight, boardIndex: 0);
```

The underlying `CastMonsterAction` is executable only when:

- the player is the active player;
- the card is in that player's hand;
- the card is summonable;
- the requested board slot is free.

## Monster components

Monster statistics can also be represented by `MonsterCardComponent`. A default monster created with the `MonsterCard(int mana, int attack, int life, ...)` constructor receives a `MonsterCardComponent` containing those values.

The component exposes:

- `ManaValue` / `ManaBaseValue`
- `AttackValue` / `AttackBaseValue`
- `LifeValue` / `LifeBaseValue`
- `GetPotentialTargets(IGameState)`

## Spells

CGE has two concrete spell card types.

### TargetlessSpellCard

A targetless spell receives one or more `ITargetlessSpellCardComponent` components:

```csharp
var heal = new TargetlessSpellCard(
    new HealComponent(1),
    player,
    "Heal");
```

A component derives from `TargetlessSpellCardComponent` and must implement `Cast(IGame)`:

```csharp
internal sealed class HealComponent : TargetlessSpellCardComponent
{
    public HealComponent(int mana) : base(mana)
    {
    }

    public override void Cast(IGame game)
    {
        game.Execute(
            new ModifyLifeStatAction(
                game.ActivePlayer,
                3));
    }

    public override object Clone() =>
        new HealComponent(ManaValue);
}
```

Cast it through the player:

```csharp
player.CastSpell(game, heal);
```

### TargetfulSpellCard

A targetful spell receives an `ISpellCardComponent` and requires a target when cast:

```csharp
var lightning = new TargetfulSpellCard(
    new LightningComponent(2),
    player,
    "Lightning");
```

A targetful component must implement both `Cast(IGame, ICharacter)` and `GetPotentialTargets(IGameState)`:

```csharp
internal sealed class LightningComponent : TargetfulSpellCardComponent
{
    public LightningComponent(int mana) : base(mana)
    {
    }

    public override void Cast(IGame game, ICharacter target)
    {
        game.Execute(new ModifyLifeStatAction(target, -4));
    }

    public override HashSet<ICharacter> GetPotentialTargets(
        IGameState gameState)
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

    public override object Clone() =>
        new LightningComponent(ManaValue);
}
```

Cast it with a valid target:

```csharp
player.CastSpell(game, lightning, opponent);
```

The underlying `CastTargetfulSpellAction` checks the active player, card ownership through the hand, mana availability, non-null target and membership in `GetPotentialTargets(IGameState)`.

## Mana and statistics

Players and cards expose current and base statistics.

For a player:

```csharp
player.ManaValue
player.ManaBaseValue
player.LifeValue
player.LifeBaseValue
player.AttackValue
player.AttackBaseValue
```

For a monster:

```csharp
monster.ManaValue
monster.LifeValue
monster.AttackValue
monster.IsReadyToAttack
```

State-changing statistics should normally be changed through the corresponding actions, for example:

```csharp
game.Execute(new ModifyManaStatAction(player, 4, 4));
game.Execute(new ModifyLifeStatAction(player, -3));
```

## Drawing cards

Use `DrawCardAction` or the player convenience method:

```csharp
player.DrawCard(game);

// Equivalent action:
game.Execute(new DrawCardAction(player));
```

The action first verifies that both required transitions are possible:

- the deck contains a card;
- the hand is not full.

This prevents a card from being removed from the deck when it cannot subsequently be added to the hand.

## Turns

Advance the turn with:

```csharp
game.NextTurn();
```

`NextTurn()` executes `EndOfTurnEvent`, then `StartOfTurnEvent`.

The default game reactions include:

- changing the active player at the end of the turn;
- updating the active player's mana at the start of the turn;
- drawing a card at the start of the turn.

Monster cards also receive a built-in reaction that makes them ready to attack when their owner becomes the active player.

## Combat

A monster attacks through:

```csharp
monster.Attack(game, opponent);
```

The underlying `AttackAction` is executable only when:

- an attacker exists;
- a target exists;
- the attacker is on the active player's board;
- the attacker is ready to attack;
- the target is in the attacker's potential targets.

When executed, the action applies damage to the target, applies the target's attack value to the attacker and marks the attacker as no longer ready to attack.

The current `AttackAction` does **not** automatically call `NextTurn()`.

## Events and reactions

CGE uses events as actions that mark lifecycle points. Examples implemented in the framework include:

- `StartOfGameEvent`
- `EndOfGameEvent`
- `StartOfTurnEvent`
- `EndOfTurnEvent`
- start/end draw-card events;
- start/end play-card events;
- start/end attack events.

`IReaction` does not return a collection of actions. Its actual contract is:

```csharp
public interface IReaction : ICloneable
{
    void ReactTo(IGame game, IActionEvent actionEvent);

    ICard FindParentCard(IGameState gameState);

    IPlayer FindParentPlayer(IGameState gameState);
}
```

A custom reaction can therefore execute additional actions directly:

```csharp
public sealed class ExampleReaction : Reaction
{
    public override void ReactTo(
        IGame game,
        IActionEvent actionEvent)
    {
        if (actionEvent.IsAfter(typeof(StartOfTurnEvent)))
        {
            game.Execute(
                new ModifyLifeStatAction(
                    game.ActivePlayer,
                    1));
        }
    }

    public override object Clone() =>
        new ExampleReaction();
}
```

The reaction must be attached to a reactive object such as a player, card or component for `AllReactions()` to discover it.

## End of game

The action queue tracks the game-over state after an `EndOfGameEvent` is executed:

```csharp
game.Execute(new EndOfGameEvent());
```

After that point, subsequent actions are not executed.

Changing a player's life to zero does not, by itself, set the action queue's game-over flag. A game-specific rule must execute `EndOfGameEvent` when the game considers the match finished.

## Cloning

`Game.Clone()` creates a cloned game state:

```csharp
var clone = (Game)game.Clone();
```

The implementation clones players, their collections, reactions and the action queue. This can be used for state snapshots or simulations.

## Multiple actions

`Game.Execute(List<IAction>)` exists and executes each action by calling `Execute(IAction)` sequentially:

```csharp
game.Execute(new List<IAction>
{
    new ModifyLifeStatAction(opponent, -2),
    new ModifyLifeStatAction(player, -1)
});
```

This is convenience sequencing; it is **not** a transaction or a grouped-action mechanism. Each action goes through the normal action queue independently, including its own executable check and reactions.

## Repository demo

The repository contains an executable console example at:

`examples/CardGameEngine.Demo`

The demo is the canonical usage example for this branch. It demonstrates:

- creating players and a game;
- populating decks with `Deck.Push`;
- selecting the active player;
- starting the game;
- executing a direct mana action;
- casting a monster;
- casting a targetful spell;
- casting a targetless spell;
- changing turns;
- attacking;
- cloning;
- explicitly ending the game with `EndOfGameEvent`.

Run it with:

```bash
dotnet run --project examples/CardGameEngine.Demo/CardGameEngine.Demo.csproj
```

## Tests

The test project contains regression coverage for action invariants, including:

- drawing with a full hand;
- casting a monster while not active;
- casting a spell that is not in the player's hand;
- casting a targetful spell against an invalid target;
- attacking with a monster that is not on the active player's board.

Run the test suite with:

```bash
dotnet test CGE.slnx -c Release
```

## Build

```bash
dotnet build CGE.slnx -c Release
```

## Scope of this guide

This README intentionally documents only APIs and behavior verified in the `fix/misc` implementation. It does not describe transactional/grouped actions, automatic game-over detection from life totals, or other behavior that is not implemented by this branch.
