using System;

namespace CardGameEngine
{
    [Serializable]
    public class EndPlayTargetfulSpellCardEvent : EndPlaySpellCardEvent
    {
/// <summary>Initializes a new instance of the <see cref="EndPlayTargetfulSpellCardEvent"/> type.</summary>
/// <param name="spellCard">The spellCard value.</param>
/// <param name="target">The target value.</param>
        public EndPlayTargetfulSpellCardEvent(ITargetfulSpellCard spellCard, ICharacter target)
            : base(spellCard)
        {
            Target = target;
        }

/// <summary>Initializes a new instance of the <see cref="EndPlayTargetfulSpellCardEvent"/> type.</summary>
/// <param name="getSpellCard">The getSpellCard value.</param>
/// <param name="target">The target value.</param>
        public EndPlayTargetfulSpellCardEvent(Func<ITargetfulSpellCard> getSpellCard, ICharacter target)
            : base(getSpellCard)
        {
            Target = target;
        }

        public ICharacter Target { get; protected set; }
    }
}
