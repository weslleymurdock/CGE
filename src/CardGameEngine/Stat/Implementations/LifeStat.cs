using System;
using Newtonsoft.Json;

namespace CardGameEngine
{
    [Serializable]
    public class LifeStat : Stat
    {
        /// <summary>
        /// Maximum number of damage that can be taken.
        /// </summary>
        public LifeStat(int value) : this(value, value)
        {
        }

        [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="LifeStat"/> type.</summary>
/// <param name="value">The value value.</param>
/// <param name="baseValue">The baseValue value.</param>
        public LifeStat(int value, int baseValue) : base(value, baseValue)
        {
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override object Clone()
        {
            return new LifeStat(Value, BaseValue);
        }
    }
}
