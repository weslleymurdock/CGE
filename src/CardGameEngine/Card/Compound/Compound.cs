using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public abstract class Compound : ICompound
{
    public List<ICardComponent> Components { get; }

    [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="Compound"/> type.</summary>
/// <param name="components">The components value.</param>
    public Compound(List<ICardComponent> components)
    {
        Components = components;
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public abstract object Clone();
}
