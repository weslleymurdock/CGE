using System;

namespace CardGameEngine
{
    [Serializable]
    public class EndPlayCardEvent : CardEvent
    {
        public EndPlayCardEvent(ICard card) : base(card)
        {
        }

        public EndPlayCardEvent(Func<ICard> getCard) : base(getCard)
        {
        }

        public override EndPlayCardEvent Clone()
        {
            return new EndPlayCardEvent((ICard)Card.Clone());
        }
    }
}
