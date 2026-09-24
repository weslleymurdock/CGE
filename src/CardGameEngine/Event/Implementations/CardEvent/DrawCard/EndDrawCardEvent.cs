using System;

namespace CardGameEngine
{
    [Serializable]
    public class EndDrawCardEvent : CardEvent
    {
/// <summary>Initializes a new instance of the <see cref="EndDrawCardEvent"/> type.</summary>
        public EndDrawCardEvent(ICard card) : base(card)
        {
        }

/// <summary>Initializes a new instance of the <see cref="EndDrawCardEvent"/> type.</summary>
        public EndDrawCardEvent(Func<ICard> getCard) : base(getCard)
        {
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override object Clone()
        {
            return new EndDrawCardEvent((ICard)this.Card.Clone());
        }
    }
}
