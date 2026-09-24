using System;

namespace CardGameEngine
{
    [Serializable]
    public abstract class CardEvent : Event
    {
        public ICard Card { get => GetCard(); }

        protected Func<ICard> GetCard;

/// <summary>Initializes a new instance of the <see cref="CardEvent"/> type.</summary>
        public CardEvent(ICard card) : this(() => card)
        {
        }

/// <summary>Initializes a new instance of the <see cref="CardEvent"/> type.</summary>
/// <param name="getCard">The getCard value.</param>
        public CardEvent(Func<ICard> getCard)
        {
            GetCard = getCard;
        }
    }
}
