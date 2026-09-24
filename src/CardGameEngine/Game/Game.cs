using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CardGameEngine
{
    [Serializable]
    public class Game : IGame
    {
        /// <summary>
        /// Index of the active Player. Refers to the Players array.
        /// Also see the ActivePlayer accessor.
        /// </summary>
        [JsonProperty]
        protected int activePlayerIndex;

        [JsonProperty]
        protected ActionQueue actionQueue;

        public List<IReaction> Reactions { get; }

        public List<IPlayer> Players { get; protected set; }

        /// <summary>
        /// Represent the current Game state and provides methods to alter
        /// this Game state.
        /// </summary>
        public Game() : this([])
        {
        }

        /// <summary>
        /// Represent the current Game state and provides methods to alter
        /// this Game state.
        /// </summary>
        /// <param name="players"></param>
        public Game(List<IPlayer> players)
            : this(players, new Random().Next(players.Count), new ActionQueue(false), [])
        {
            Reactions.Add(new ModifyActivePlayerOnEndOfTurnEventReaction());
            Reactions.Add(new ModifyManaOnStartOfTurnEventReaction());
            Reactions.Add(new DrawCardOnStartOfTurnEventReaction());
        }

/// <summary>Initializes a new instance of the <see cref="Game"/> type.</summary>
/// <param name="players">The players value.</param>
/// <param name="activePlayerIndex">The activePlayerIndex value.</param>
/// <param name="actionQueue">The actionQueue value.</param>
/// <param name="reactions">The reactions value.</param>
        [JsonConstructor]
        public Game(List<IPlayer> players, int activePlayerIndex, ActionQueue actionQueue, List<IReaction> reactions)
        {
            Players = players;
            this.activePlayerIndex = activePlayerIndex;
            this.actionQueue = actionQueue;
            Reactions = reactions;
        }

        [JsonIgnore]
        public IPlayer ActivePlayer
        {
            get => Players[activePlayerIndex];
            set
            {
                activePlayerIndex = Players.IndexOf(value);
            }
        }

        [JsonIgnore]
        public List<IPlayer> NonActivePlayers
        {
            get
            {
                return [.. Players.Where(p => p != ActivePlayer)];
            }
        }

        [JsonIgnore]
        public List<ICard> AllCards
        {
            get
            {
                List<ICard> allCards = [];
                foreach (IPlayer player in Players)
                {
                    allCards.AddRange(player.AllCards);
                }
                return allCards;
            }
        }

        [JsonIgnore]
        public List<ICard> AllCardsOnTheBoard
        {
            get
            {
                List<ICard> allCards = [];
                foreach (IPlayer player in Players)
                {
                    allCards.AddRange(player.Board.AllCards);
                }
                return allCards;
            }
        }

/// <summary>Gets all reactions associated with this object.</summary>
/// <returns>The result of the operation.</returns>
        public List<IReaction> AllReactions()
        {
            List<IReaction> allReactions = [.. Reactions];
            Players.ForEach(p => allReactions.AddRange(p.AllReactions()));
            return allReactions;
        }

/// <summary>Initializes the game and starts the first turn.</summary>
/// <param name="initialHandSize">The initialHandSize value.</param>
/// <param name="initialPlayerLife">The initialPlayerLife value.</param>
        public void StartGame(int initialHandSize = 4, int initialPlayerLife = 30)
        {
            //Do not trigger any reactions during setup
            actionQueue.ExecuteReactions = false;

            foreach (IPlayer player in Players)
            {
                player.ManaValue = 0;
                player.ManaBaseValue = 0;
                player.LifeValue = initialPlayerLife;
                player.LifeBaseValue = initialPlayerLife;

                for (int i = 0; i < initialHandSize; ++i)
                {
                    player.DrawCard(this);
                }
            }

            actionQueue.ExecuteReactions = true;

            Execute(new StartOfGameEvent());
            Execute(new StartOfTurnEvent());
        }

/// <summary>Advances the game to the next turn.</summary>
        public void NextTurn()
        {
            Execute(new EndOfTurnEvent());
            Execute(new StartOfTurnEvent());
        }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="action">The action value.</param>
        public void Execute(IAction action)
        {
            actionQueue.Execute(this, action);
        }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="actions">The actions value.</param>
        public void Execute(List<IAction> actions)
        {
            actions.ForEach(a => Execute(a));
        }

/// <summary>Reacts to the specified action event.</summary>
/// <param name="game">The game value.</param>
/// <param name="actionEvent">The actionEvent value.</param>
        public void ReactTo(IGame game, IActionEvent actionEvent)
        {
            AllReactions().ForEach(r => r.ReactTo(game, actionEvent));
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public object Clone()
        {
            List<IPlayer> playersClone = [];
            foreach (IPlayer player in Players)
            {
                playersClone.Add((IPlayer)player.Clone());
            }

            List<IReaction> reactionsClone = [];
            foreach (IReaction reaction in Reactions)
            {
                reactionsClone.Add((IReaction)reaction.Clone());
            }

            return new Game(
                playersClone,
                activePlayerIndex,
                (ActionQueue)actionQueue.Clone(),
                reactionsClone
            );
        }

/// <summary>Finds the parent card in the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
        public ICard FindParentCard(IGameState gameState)
        {
            throw new CardGameEngineException("Cannot use method 'FindParentCard' on " +
                "instance of type 'Game'");
        }

/// <summary>Finds the parent player in the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
        public IPlayer FindParentPlayer(IGameState gameState)
        {
            throw new CardGameEngineException("Cannot use method 'FindParentPlayer' on " +
                "instance of type 'Game'");
        }
    }
}
