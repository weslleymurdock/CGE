using System;

namespace CardGameEngine
{
    [Serializable]
    public class StartPlayTargetfulSpellCardEvent : StartPlaySpellCardEvent
    {
/// <summary>Initializes a new instance of the <see cref="StartPlayTargetfulSpellCardEvent"/> type.</summary>
/// <param name="spellCard">The spellCard value.</param>
/// <param name="target">The target value.</param>
        public StartPlayTargetfulSpellCardEvent(ITargetfulSpellCard spellCard, ICharacter target)
            : base(spellCard)
        {
            this.target = target;
        }

/// <summary>Initializes a new instance of the <see cref="StartPlayTargetfulSpellCardEvent"/> type.</summary>
/// <param name="getSpellCard">The getSpellCard value.</param>
/// <param name="target">The target value.</param>
        public StartPlayTargetfulSpellCardEvent(Func<ITargetfulSpellCard> getSpellCard, ICharacter target)
            : base(getSpellCard)
        {
            this.target = target;
        }

        public ICharacter target { get; protected set; }
    }
}
