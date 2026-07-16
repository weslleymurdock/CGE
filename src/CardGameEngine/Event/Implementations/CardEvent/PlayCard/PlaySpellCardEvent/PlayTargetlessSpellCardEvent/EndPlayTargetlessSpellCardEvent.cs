using System;

namespace CardGameEngine
{
    [Serializable]
    public class EndPlayTargetlessSpellCardEvent : EndPlaySpellCardEvent
    {
        public EndPlayTargetlessSpellCardEvent(ITargetlessSpellCard spellCard)
            : base(spellCard)
        {
        }

        public EndPlayTargetlessSpellCardEvent(Func<ITargetlessSpellCard> getSpellCard)
            : base(getSpellCard)
        {
        }
    }
}
