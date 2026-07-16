using System;

namespace CardGameEngine
{
    [Serializable]
    public class StartPlayCardEvent : CardEvent
    {
        public StartPlayCardEvent(ICard card) : base(card)
        {
        }

        public StartPlayCardEvent(Func<ICard> getCard) : base(getCard)
        {
        }

        public override StartPlayCardEvent Clone()
        {
            return new StartPlayCardEvent((ICard)Card.Clone());
        }
    }
}
