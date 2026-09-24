using System;
namespace CardGameEngine
{
    [Serializable]
    public abstract class AttackEvent : Event
    {
        protected Func<IMonsterCard> GetAttacker;
        protected Func<ICharacter> GetTarget;

/// <summary>Initializes a new instance of the <see cref="AttackEvent"/> type.</summary>
/// <param name="getAttacker">The getAttacker value.</param>
/// <param name="getTarget">The getTarget value.</param>
        public AttackEvent(Func<IMonsterCard> getAttacker, Func<ICharacter> getTarget)
        {
            GetAttacker = getAttacker;
            GetTarget = getTarget;
        }

        public IMonsterCard Attacker { get => GetAttacker(); }
        public ICharacter Target { get => GetTarget(); }
    }
}
