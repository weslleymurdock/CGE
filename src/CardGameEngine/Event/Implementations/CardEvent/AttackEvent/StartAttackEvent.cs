using System;
namespace CardGameEngine
{
    [Serializable]
    public class StartAttackEvent : AttackEvent
    {
/// <summary>Initializes a new instance of the <see cref="StartAttackEvent"/> type.</summary>
/// <param name="getAttacker">The getAttacker value.</param>
/// <param name="getTarget">The getTarget value.</param>
        public StartAttackEvent(Func<IMonsterCard> getAttacker, Func<ICharacter> getTarget)
            : base(getAttacker, getTarget)
        {
        }

/// <summary>Initializes a new instance of the <see cref="StartAttackEvent"/> type.</summary>
/// <param name="attacker">The attacker value.</param>
/// <param name="target">The target value.</param>
        public StartAttackEvent(IMonsterCard attacker, ICharacter target)
            : this(() => attacker, () => target)
        {
        }

/// <summary>Initializes a new instance of the <see cref="StartAttackEvent"/> type.</summary>
/// <param name="getAttacker">The getAttacker value.</param>
/// <param name="target">The target value.</param>
        public StartAttackEvent(Func<IMonsterCard> getAttacker, ICharacter target)
            : this(getAttacker, () => target)
        {
        }

/// <summary>Initializes a new instance of the <see cref="StartAttackEvent"/> type.</summary>
/// <param name="attacker">The attacker value.</param>
/// <param name="getTarget">The getTarget value.</param>
        public StartAttackEvent(IMonsterCard attacker, Func<ICharacter> getTarget)
            : this(() => attacker, getTarget)
        {
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override StartAttackEvent Clone()
        {
            return new StartAttackEvent((IMonsterCard)this.Attacker.Clone(), (ICharacter)this.Target.Clone());
        }
    }
}
