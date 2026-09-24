using System;
namespace CardGameEngine
{
    [Serializable]
    public class EndPlaySpellCardEvent : EndPlayCardEvent
    {
/// <summary>Initializes a new instance of the <see cref="EndPlaySpellCardEvent"/> type.</summary>
/// <param name="base(spellCard">The base(spellCard value.</param>
        public EndPlaySpellCardEvent(ISpellCard spellCard) : base(spellCard)
        {
        }

/// <summary>Initializes a new instance of the <see cref="EndPlaySpellCardEvent"/> type.</summary>
/// <param name="base(getSpellCard">The base(getSpellCard value.</param>
        public EndPlaySpellCardEvent(Func<ISpellCard> getSpellCard) : base(getSpellCard)
        {
        }
    }
}
