using System;
using Newtonsoft.Json;

namespace CardGameEngine {

    [Serializable]
    public class ManaCostStat : Stat
    {
        /// <summary>
        /// Costs in Mana.
        /// </summary>
        [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="ManaCostStat"/> type.</summary>
/// <param name="value">The value value.</param>
/// <param name="base(value">The base(value value.</param>
/// <param name="baseValue">The baseValue value.</param>
        public ManaCostStat(int value, int baseValue) : base(value, baseValue)
        {
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override object Clone()
        {
            return new ManaCostStat(Value, BaseValue);
        }
    }
}
