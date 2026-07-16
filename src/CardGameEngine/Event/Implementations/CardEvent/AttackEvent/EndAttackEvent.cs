using System;
namespace CardGameEngine
{
    [Serializable]
    public class EndAttackEvent : AttackEvent
    {
        public EndAttackEvent(Func<IMonsterCard> getAttacker, Func<ICharacter> getTarget)
            : base(getAttacker, getTarget)
        {
        }

        public EndAttackEvent(IMonsterCard attacker, ICharacter target)
            : this(() => attacker, () => target)
        {
        }

        public EndAttackEvent(Func<IMonsterCard> getAttacker, ICharacter target)
            : this(getAttacker, () => target)
        {
        }

        public EndAttackEvent(IMonsterCard attacker, Func<ICharacter> getTarget)
            : this(() => attacker, getTarget)
        {
        }

        public override object Clone()
        {
            return new EndAttackEvent((IMonsterCard)this.Attacker.Clone(), (ICharacter)this.Target.Clone());
        }
    }
}
