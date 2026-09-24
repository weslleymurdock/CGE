using System;
using Newtonsoft.Json;

namespace CardGameEngine
{
    [Serializable]
    public abstract class Stat : IStat, ICloneable
    {
        public const int GlobalMin = -99;
        public const int GlobalMax = 99;

        [JsonProperty]
        protected int value;

        [JsonProperty]
        protected int baseValue;

        /// <summary>
        /// Represents a Card's property.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="baseValue"></param>
        [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="Stat"/> type.</summary>
/// <param name="value">The value value.</param>
/// <param name="baseValue">The baseValue value.</param>
        public Stat(int value, int baseValue)
        {
            this.baseValue = baseValue;
            this.value = value;
        }

        [JsonIgnore]
        public virtual int Value {
            get => value;
            set => this.value = Math.Max(GlobalMin, Math.Min(GlobalMax, value));
        }

        [JsonIgnore]
        public virtual int BaseValue
        {
            get => baseValue;
            set => baseValue = Math.Max(GlobalMin, Math.Min(GlobalMax, value));
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public abstract object Clone();
    }
}
