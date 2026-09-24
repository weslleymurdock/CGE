using System;

namespace CardGameEngine
{
    [Serializable]
    public class StartPlaySpellCardEvent : StartPlayCardEvent
    {
/// <summary>Initializes a new instance of the <see cref="StartPlaySpellCardEvent"/> type.</summary>
        public StartPlaySpellCardEvent(ISpellCard spellCard) : base(spellCard)
        {
        }

/// <summary>Initializes a new instance of the <see cref="StartPlaySpellCardEvent"/> type.</summary>
        public StartPlaySpellCardEvent(Func<ISpellCard> getSpellCard) : base(getSpellCard)
        {
        }
    }
}
