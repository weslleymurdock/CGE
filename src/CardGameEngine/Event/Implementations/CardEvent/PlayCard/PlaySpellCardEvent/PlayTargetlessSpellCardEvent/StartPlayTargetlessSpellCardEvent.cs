using System;

namespace CardGameEngine
{
    [Serializable]
    public class StartPlayTargetlessSpellCardEvent : StartPlaySpellCardEvent
    {
/// <summary>Initializes a new instance of the <see cref="StartPlayTargetlessSpellCardEvent"/> type.</summary>
/// <param name="spellCard">The spellCard value.</param>
        public StartPlayTargetlessSpellCardEvent(ITargetlessSpellCard spellCard)
            : base(spellCard)
        {
        }

/// <summary>Initializes a new instance of the <see cref="StartPlayTargetlessSpellCardEvent"/> type.</summary>
/// <param name="getSpellCard">The getSpellCard value.</param>
        public StartPlayTargetlessSpellCardEvent(Func<ITargetlessSpellCard> getSpellCard)
            : base(getSpellCard)
        {
        }
    }
}
