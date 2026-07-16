using System;

namespace CardGameEngine
{
    [Serializable]
    public class EndDrawCardEvent : CardEvent
    {
        public EndDrawCardEvent(ICard card) : base(card)
        {
        }

        public EndDrawCardEvent(Func<ICard> getCard) : base(getCard)
        {
        }

        public override object Clone()
        {
            return new EndDrawCardEvent((ICard)this.Card.Clone());
        }
    }
}
