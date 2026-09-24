using System;
namespace CardGameEngine
{
    [Serializable]
    public class EndPlaySpellCardEvent : EndPlayCardEvent
    {
/// <summary>Initializes a new instance of the <see cref="EndPlaySpellCardEvent"/> type.</summary>
        public EndPlaySpellCardEvent(ISpellCard spellCard) : base(spellCard)
        {
        }

/// <summary>Initializes a new instance of the <see cref="EndPlaySpellCardEvent"/> type.</summary>
        public EndPlaySpellCardEvent(Func<ISpellCard> getSpellCard) : base(getSpellCard)
        {
        }
    }
}
