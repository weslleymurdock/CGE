using System;
using Newtonsoft.Json;

namespace CardGameEngine
{
    [Serializable]
    public class ManaPoolStat : Stat
    {
        /// <summary>
        /// Represents available mana.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="baseValue"></param>
        [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="ManaPoolStat"/> type.</summary>
/// <param name="value">The value value.</param>
/// <param name="baseValue">The baseValue value.</param>
        public ManaPoolStat(int value, int baseValue) : base(value, baseValue)
        {
        }

        [JsonIgnore]
        public override int Value
        {
            get => base.Value;
            set => base.Value = Math.Max(0, value);
        }

        [JsonIgnore]
        public override int BaseValue
        {
            get => base.BaseValue;
            set => base.BaseValue = Math.Max(0, value);
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override object Clone()
        {
            return new ManaPoolStat(Value, BaseValue);
        }
    }
}
