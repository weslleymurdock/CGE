using System;

namespace CardGameEngine;

public interface ICard : IManaful, IReactive, ICompound, ICloneable
{
    string Name { get; }
}
