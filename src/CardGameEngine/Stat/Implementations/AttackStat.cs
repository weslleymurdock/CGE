using System;
using Newtonsoft.Json;

namespace CardGameEngine
{
    [Serializable]
    public class AttackStat : Stat
    {
        /// <summary>
        /// Potential damage to be dealt.
        /// </summary>
        public AttackStat(int value) : this(value, value)
        {
        }

        [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="AttackStat"/> type.</summary>
/// <param name="value">The value value.</param>
/// <param name="baseValue">The baseValue value.</param>
        public AttackStat(int value, int baseValue) : base(value, baseValue)
        {
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override object Clone()
        {
            return new AttackStat(Value, BaseValue);
        }
    }
}
