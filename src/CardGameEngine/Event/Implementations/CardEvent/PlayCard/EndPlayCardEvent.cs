using System;

namespace CardGameEngine
{
    [Serializable]
    public class EndPlayCardEvent : CardEvent
    {
/// <summary>Initializes a new instance of the <see cref="EndPlayCardEvent"/> type.</summary>
/// <param name="base(card">The base(card value.</param>
        public EndPlayCardEvent(ICard card) : base(card)
        {
        }

/// <summary>Initializes a new instance of the <see cref="EndPlayCardEvent"/> type.</summary>
/// <param name="base(getCard">The base(getCard value.</param>
        public EndPlayCardEvent(Func<ICard> getCard) : base(getCard)
        {
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override EndPlayCardEvent Clone()
        {
            return new EndPlayCardEvent((ICard)Card.Clone());
        }
    }
}
