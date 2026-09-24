using System;
namespace CardGameEngine
{
    [Serializable]
    public class EndAttackEvent : AttackEvent
    {
/// <summary>Initializes a new instance of the <see cref="EndAttackEvent"/> type.</summary>
/// <param name="getAttacker">The getAttacker value.</param>
/// <param name="getTarget">The getTarget value.</param>
        public EndAttackEvent(Func<IMonsterCard> getAttacker, Func<ICharacter> getTarget)
            : base(getAttacker, getTarget)
        {
        }

/// <summary>Initializes a new instance of the <see cref="EndAttackEvent"/> type.</summary>
/// <param name="attacker">The attacker value.</param>
/// <param name="target">The target value.</param>
        public EndAttackEvent(IMonsterCard attacker, ICharacter target)
            : this(() => attacker, () => target)
        {
        }

/// <summary>Initializes a new instance of the <see cref="EndAttackEvent"/> type.</summary>
/// <param name="getAttacker">The getAttacker value.</param>
/// <param name="target">The target value.</param>
        public EndAttackEvent(Func<IMonsterCard> getAttacker, ICharacter target)
            : this(getAttacker, () => target)
        {
        }

/// <summary>Initializes a new instance of the <see cref="EndAttackEvent"/> type.</summary>
/// <param name="attacker">The attacker value.</param>
/// <param name="getTarget">The getTarget value.</param>
        public EndAttackEvent(IMonsterCard attacker, Func<ICharacter> getTarget)
            : this(() => attacker, getTarget)
        {
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override object Clone()
        {
            return new EndAttackEvent((IMonsterCard)this.Attacker.Clone(), (ICharacter)this.Target.Clone());
        }
    }
}
