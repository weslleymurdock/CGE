using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine
{
    [Serializable]
    public class Player : IPlayer
    {
        [JsonProperty]
        protected ManaPoolStat manaPoolStat;

        [JsonProperty]
        protected AttackStat attackStat;

        [JsonProperty]
        protected LifeStat lifeStat;

        public List<IReaction> Reactions { get; }

        public IDeck Deck { get; protected set; }
        public IHand Hand { get; protected set; }
        public IBoard Board { get; protected set; }
        public IDeck Graveyard { get; protected set; }

        /// <summary>
        /// Represents a Player and all his/her associated Cards.
        /// </summary>
        public Player() : this(new Deck())
        {
        }

        /// <summary>
        /// Represents a Player and all his/her associated Cards.
        /// </summary>
        /// <param name="deck"></param>
        public Player(IDeck deck)
            : this(deck, new Hand(), new Board(), new Deck(),
                  new ManaPoolStat(0, 0), new AttackStat(0), new LifeStat(0),
                  [])
        {
        }

        [JsonConstructor]
        protected Player(IDeck deck, IHand hand, IBoard board, IDeck graveyard,
            ManaPoolStat manaPoolStat, AttackStat attackStat, LifeStat lifeStat,
            List<IReaction> reactions)
        {
            Deck = deck;
            Hand = hand;
            Board = board;
            Graveyard = graveyard;

            this.manaPoolStat = manaPoolStat;
            this.attackStat = attackStat;
            this.lifeStat = lifeStat;
            Reactions = reactions;
        }

        [JsonIgnore]
        public bool IsAlive => lifeStat.Value > 0;

        [JsonIgnore]
        public List<ICard> AllCards
        {
            get
            {
                List<ICard> allCards =
                [
                    .. Deck.AllCards,
                    .. Hand.AllCards,
                    .. Board.AllCards,
                    .. Graveyard.AllCards,
                ];
                return allCards;
            }
        }

        [JsonIgnore]
        public int AttackValue
        {
            get => attackStat.Value;
            set => attackStat.Value = Math.Max(0, value);
        }

        [JsonIgnore]
        public int AttackBaseValue
        {
            get => attackStat.BaseValue;
            set => attackStat.BaseValue = Math.Max(0, value);
        }

        [JsonIgnore]
        public int LifeValue
        {
            get => lifeStat.Value;
            set => lifeStat.Value = Math.Max(0, value);
        }

        [JsonIgnore]
        public int LifeBaseValue
        {
            get => lifeStat.BaseValue;
            set => lifeStat.BaseValue = Math.Max(0, value);
        }

        [JsonIgnore]
        public int ManaValue
        {
            get => manaPoolStat.Value;
            set => manaPoolStat.Value = Math.Max(0, value);
        }

        [JsonIgnore]
        public int ManaBaseValue
        {
            get => manaPoolStat.BaseValue;
            set => manaPoolStat.BaseValue = Math.Max(0, value);
        }

        [JsonIgnore]
        public List<ICharacter> Characters
        {
            get
            {
                List<ICharacter> characters =

                [
                    this
                ];
                Board.AllCards.ForEach(c => characters.Add((ICharacter)c));
                return characters;
            }
        }

/// <summary>Gets all reactions associated with this object.</summary>
/// <returns>The result of the operation.</returns>
        public List<IReaction> AllReactions()
        {
            List<IReaction> allReactions = [.. Reactions];
            AllCards.ForEach(c => allReactions.AddRange(c.AllReactions()));
            return allReactions;
        }

/// <summary>Draws a card for the player.</summary>
/// <param name="game">The game value.</param>
        public void DrawCard(IGame game)
        {
            game.Execute(new DrawCardAction(this));
        }

/// <summary>Performs the CastMonster operation.</summary>
/// <param name="game">The game value.</param>
/// <param name="monsterCard">The monsterCard value.</param>
/// <param name="boardIndex">The boardIndex value.</param>
        public void CastMonster(IGame game, IMonsterCard monsterCard, int boardIndex)
        {
            if (!monsterCard.IsSummonable(game))
            {
                throw new CardGameEngineException("Tried to play a card that is " +
                    "not playable!");
            }

            if (!Board.IsFreeSlot(boardIndex))
            {
                throw new CardGameEngineException("Slot with index " + boardIndex +
                    " is already occupied!");
            }

            game.Execute(new CastMonsterAction(this, monsterCard, boardIndex));
        }

/// <summary>Performs the CastSpell operation.</summary>
/// <param name="game">The game value.</param>
/// <param name="spellCard">The spellCard value.</param>
        public void CastSpell(IGame game, ITargetlessSpellCard spellCard)
        {
            if (!spellCard.IsCastable(game))
            {
                throw new CardGameEngineException("Tried to play a card that is " +
                    "not playable!");
            }

            game.Execute(new CastTargetlessSpellAction(this, spellCard));
        }

/// <summary>Performs the CastSpell operation.</summary>
/// <param name="game">The game value.</param>
/// <param name="spellCard">The spellCard value.</param>
/// <param name="target">The target value.</param>
        public void CastSpell(IGame game, ITargetfulSpellCard spellCard, ICharacter target)
        {
            if (!spellCard.IsCastable(game))
            {
                throw new CardGameEngineException("Tried to play a card that is " +
                    "not playable!");
            }

            game.Execute(new CastTargetfulSpellAction(this, spellCard, target));
        }

/// <summary>Gets the characters that can currently be targeted.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
        public HashSet<ICharacter> GetPotentialTargets(IGameState gameState)
        {
            return [];
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
        public virtual object Clone()
        {
            List<IReaction> reactionsClone = [];
            foreach (IReaction reaction in Reactions)
            {
                reactionsClone.Add((IReaction)reaction.Clone());
            }

            return new Player(
                (IDeck)Deck.Clone(),
                (IHand)Hand.Clone(),
                (IBoard)Board.Clone(),
                (IDeck)Graveyard.Clone(),
                (ManaPoolStat)manaPoolStat.Clone(),
                (AttackStat)attackStat.Clone(),
                (LifeStat)lifeStat.Clone(),
                reactionsClone
            );
        }

/// <summary>Finds the parent card in the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
        public ICard FindParentCard(IGameState gameState)
        {
            throw new CardGameEngineException("Cannot use method 'FindParentCard' on " +
                "instance of type 'Player'");
        }

/// <summary>Finds the parent player in the specified game state.</summary>
/// <param name="gameState">The gameState value.</param>
/// <returns>The result of the operation.</returns>
        public IPlayer FindParentPlayer(IGameState gameState)
        {
            return this;
        }

/// <summary>Creates a player from the supplied game components.</summary>
/// <param name="deck">The deck value.</param>
/// <param name="hand">The hand value.</param>
/// <param name="board">The board value.</param>
/// <param name="graveyard">The graveyard value.</param>
/// <param name="mana">The mana value.</param>
/// <param name="attack">The attack value.</param>
/// <param name="life">The life value.</param>
/// <param name="reactions">The reactions value.</param>
/// <returns>The result of the operation.</returns>
        public static Player NewPlayer(IDeck deck, IHand hand, IBoard board, IDeck graveyard, ManaPoolStat mana, AttackStat attack, LifeStat life, List<IReaction> reactions)
        {
            return new Player(deck, hand, board, graveyard, mana, attack, life, reactions);
        }
    }
}
