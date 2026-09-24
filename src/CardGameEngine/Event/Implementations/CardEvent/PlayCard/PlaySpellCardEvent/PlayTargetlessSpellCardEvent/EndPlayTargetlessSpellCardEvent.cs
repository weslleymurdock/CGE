using System;

namespace CardGameEngine
{
    [Serializable]
    public class EndPlayTargetlessSpellCardEvent : EndPlaySpellCardEvent
    {
/// <summary>Initializes a new instance of the <see cref="EndPlayTargetlessSpellCardEvent"/> type.</summary>
/// <param name="spellCard">The spellCard value.</param>
        public EndPlayTargetlessSpellCardEvent(ITargetlessSpellCard spellCard)
            : base(spellCard)
        {
        }

/// <summary>Initializes a new instance of the <see cref="EndPlayTargetlessSpellCardEvent"/> type.</summary>
/// <param name="getSpellCard">The getSpellCard value.</param>
        public EndPlayTargetlessSpellCardEvent(Func<ITargetlessSpellCard> getSpellCard)
            : base(getSpellCard)
        {
        }
    }
}
