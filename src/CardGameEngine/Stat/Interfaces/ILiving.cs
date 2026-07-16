using System;

namespace CardGameEngine
{
    public interface ILiving : ICloneable
    {
        int LifeValue { get; set; }
        int LifeBaseValue { get; set; }
    }
}
