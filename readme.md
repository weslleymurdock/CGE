# CardGameEngine

CardGameEngine (CGE) is a .NET 10 framework for building turn-based card games in C#.

The framework provides the game state model, players, cards, decks, hands, boards, actions, events, reactions, statistics and the execution pipeline. A game-specific project supplies the rules and card effects.

The repository contains the framework source, tests, a console demonstration and this documentation site.

## Requirements

- .NET 10 SDK
- C# / .NET application capable of referencing NuGet packages

## Install

The framework is distributed as the NuGet package `CardGameEngine`.

For the published 1.0.0 package:

```bash
dotnet add package CardGameEngine --version 1.0.0
```

Or add the package reference manually:

```xml
<PackageReference Include="CardGameEngine" Version="1.0.0" />
```

## Architecture

CGE separates the generic game engine from game-specific rules.

A typical game project contains:

```text
MyCardGame/
├── Cards/
│   ├── KnightCard.cs
│   ├── FireballCard.cs
│   └── HealCard.cs
├── Components/
│   ├── FireballComponent.cs
│   └── HealComponent.cs
├── Reactions/
│   └── ...
└── Program.cs
```

The engine owns the state and execution pipeline. Your application defines the actual cards and rules.

## Core concepts

### Game

`Game` is the mutable game state and the entry point for executing actions.

It contains the players, active player and board state and provides operations such as:

- starting a game;
- changing turns;
- executing actions;
- cloning the game state.

`IGameState` exposes the state needed by rules without granting unrestricted mutation.

### Player

`Player` represents a participant.

A player owns:

- `Deck` — cards available to draw;
- `Hand` — cards currently held;
- `Board` — cards currently in play;
- `Graveyard` — cards that have left play.

Players are also characters, so they have life and mana statistics.

### Cards

The main card categories are:

- `MonsterCard` — a creature that occupies a board slot and can attack;
- `TargetlessSpellCard` — a spell with no selected target;
- `TargetfulSpellCard` — a spell that requires a valid target.

Cards are assembled from components so that reusable rules can be shared between cards.

### Components

Components encapsulate card-specific behavior.

For spells, derive from:

- `TargetlessSpellCardComponent`
- `TargetfulSpellCardComponent`

A targetful component must expose the valid targets for the current `IGameState`.

This allows the engine to validate an action before its effect is executed.

### Statistics

CGE provides reusable statistics such as:

- mana;
- mana cost;
- life;
- attack.

Statistics expose a current value and a base value.

### Actions

An `IAction` is the unit of state transition.

Game state should be changed through `Game.Execute(...)`, rather than by directly mutating collections or statistics.

Every action has an `IsExecutable(IGameState)` check. The check runs immediately before execution because the game state may have changed since the action was created.

If an action is no longer executable, it is discarded and its reactions are not triggered.

### Events

Events are actions used as markers in the game flow.

Examples include start/end events for:

- turns;
- attacks;
- drawing cards;
- playing monster cards;
- playing targetless spells;
- playing targetful spells.

Events allow reactions to observe meaningful points in the game lifecycle without implementing the state transition themselves.

### Reactions

`IReaction` allows cards and other reactive objects to respond to executed actions.

A reaction can return additional actions. Those actions are processed through the same execution pipeline, allowing rules to be composed without coupling every card directly to the game loop.

## Your first game

Create a console application:

```bash
dotnet new console -n MyCardGame
cd MyCardGame
dotnet add package CardGameEngine --version 1.0.0
```

Create two players:

```csharp
using CardGameEngine;

var player = new Player();
var opponent = new Player();

var game = new Game(new List<IPlayer>
{
    player,
    opponent
});
```

Start the match:

```csharp
game.StartGame(
    initialHandSize: 3,
    initialPlayerLife: 20);
```

At this point the engine owns the game lifecycle. Your code can now define cards and issue player actions.

## Creating a monster card

A monster card needs mana, attack and life information.

Game-specific cards can inherit from `MonsterCard` and expose the behavior appropriate for the game.

Once a monster is in a player's hand, it can be played through the player/game action API.

Conceptually:

```csharp
player.CastMonster(game, monster, boardIndex: 0);
```

The framework validates that the player is active, owns the card, can summon it and has a free board slot before executing the state transition.

## Creating a targetless spell

Create a component derived from `TargetlessSpellCardComponent` and implement its effect by executing engine actions.

For example, a healing component can execute:

```csharp
game.Execute(
    new ModifyLifeStatAction(
        game.ActivePlayer,
        3));
```

Then compose the component into a targetless spell card.

## Creating a targetful spell

A targetful spell derives from `TargetfulSpellCardComponent`.

It must define both:

1. what happens when the spell is cast;
2. which characters are valid targets for the current game state.

For example:

```csharp
public override HashSet<ICharacter> GetPotentialTargets(
    IGameState gameState)
{
    var targets = new HashSet<ICharacter>();

    foreach (var player in gameState.NonActivePlayers)
    {
        foreach (var character in player.Characters)
        {
            targets.Add(character);
        }
    }

    return targets;
}
```

The corresponding action validates the selected target before paying the cost and executing the effect.

## Drawing cards

Drawing is represented by an action:

```csharp
game.Execute(new DrawCardAction(player));
```

The action verifies that the deck contains a card and that the hand has capacity before removing the card from the deck.

This invariant is important because a draw is a composite state transition.

## Turns

Advance the game with:

```csharp
game.NextTurn();
```

Turn-related events and reactions can then perform rules such as refreshing mana, drawing a card or making monsters ready to attack.

## Combat

A monster can attack a valid target through the player/game API.

The engine verifies that:

- the attacker belongs to the active player's board;
- the attacker is ready to attack;
- the target is a valid target.

The resulting state changes are represented by actions, allowing deaths and other consequences to be handled through reactions.

## Executing multiple actions

CGE supports executing a sequence of actions as a group.

This distinction matters when several state changes must happen before reactions are processed.

For example, combat involving two creatures may require both life values to be reduced before death reactions move either creature to a graveyard.

Use the multi-action execution API when the rules require those transitions to be evaluated as one logical operation.

## Cloning

The game state can be cloned:

```csharp
var copy = (Game)game.Clone();
```

Cloning is useful for simulations, AI decision making and state snapshots.

## Design rules

When implementing a game, follow these rules:

1. Treat `Game` as the owner of the mutable game state.
2. Use `Game.Execute` for state transitions.
3. Put preconditions in `IAction.IsExecutable`.
4. Use events to expose meaningful lifecycle points.
5. Use reactions for rules triggered by existing actions.
6. Keep reusable card behavior in components.
7. Calculate target sets from `IGameState`, rather than storing transient target lists.
8. Prefer grouped actions when multiple state changes must occur before reactions.
9. Do not bypass the action pipeline by directly changing engine state.

## Repository examples

The repository contains a complete console example under:

`examples/CardGameEngine.Demo`

It demonstrates:

- game creation;
- initial hands;
- turn processing;
- mana;
- monster cards;
- targetful spells;
- targetless spells;
- combat;
- graveyards;
- cloning;
- game termination;
- action validation.

Use that example as a reference implementation after reading this guide.

## Development

Build:

```bash
dotnet build CGE.slnx -c Release
```

Run tests:

```bash
dotnet test CGE.slnx -c Release
```

Run the console demonstration:

```bash
dotnet run --project examples/CardGameEngine.Demo/CardGameEngine.Demo.csproj
```

## License

See the repository license for the terms applicable to CardGameEngine.
