using System;

namespace CardGameEngine
{
    [Serializable]
    public class StartPlayTargetlessSpellCardEvent : StartPlaySpellCardEvent
    {
        public StartPlayTargetlessSpellCardEvent(ITargetlessSpellCard spellCard)
            : base(spellCard)
        {
        }

        public StartPlayTargetlessSpellCardEvent(Func<ITargetlessSpellCard> getSpellCard)
            : base(getSpellCard)
        {
        }
    }
}
