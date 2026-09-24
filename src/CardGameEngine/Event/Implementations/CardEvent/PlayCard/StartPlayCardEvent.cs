using System;

namespace CardGameEngine
{
    [Serializable]
    public class StartPlayCardEvent : CardEvent
    {
/// <summary>Initializes a new instance of the <see cref="StartPlayCardEvent"/> type.</summary>
/// <param name="base(card">The base(card value.</param>
        public StartPlayCardEvent(ICard card) : base(card)
        {
        }

/// <summary>Initializes a new instance of the <see cref="StartPlayCardEvent"/> type.</summary>
/// <param name="base(getCard">The base(getCard value.</param>
        public StartPlayCardEvent(Func<ICard> getCard) : base(getCard)
        {
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override StartPlayCardEvent Clone()
        {
            return new StartPlayCardEvent((ICard)Card.Clone());
        }
    }
}
