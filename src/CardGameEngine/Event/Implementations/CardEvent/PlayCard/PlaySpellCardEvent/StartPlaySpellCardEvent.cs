using System;

namespace CardGameEngine
{
    [Serializable]
    public class StartPlaySpellCardEvent : StartPlayCardEvent
    {
/// <summary>Initializes a new instance of the <see cref="StartPlaySpellCardEvent"/> type.</summary>
/// <param name="base(spellCard">The base(spellCard value.</param>
        public StartPlaySpellCardEvent(ISpellCard spellCard) : base(spellCard)
        {
        }

/// <summary>Initializes a new instance of the <see cref="StartPlaySpellCardEvent"/> type.</summary>
/// <param name="base(getSpellCard">The base(getSpellCard value.</param>
        public StartPlaySpellCardEvent(Func<ISpellCard> getSpellCard) : base(getSpellCard)
        {
        }
    }
}
