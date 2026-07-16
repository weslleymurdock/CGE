using System;

namespace CardGameEngine
{
    public interface IManaful : ICloneable
    {
        int ManaValue { get; set; }
        int ManaBaseValue { get; set; }
    }
}
