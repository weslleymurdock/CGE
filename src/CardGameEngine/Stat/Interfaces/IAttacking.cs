using System;

namespace CardGameEngine
{
    public interface IAttacking : ITargetful, ICloneable
    {
        int AttackValue { get; set; }
        int AttackBaseValue { get; set; }
    }
}
