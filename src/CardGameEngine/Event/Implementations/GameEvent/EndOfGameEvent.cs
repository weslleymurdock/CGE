using System;
using Newtonsoft.Json;

namespace CardGameEngine
{
    [Serializable]
    public class EndOfGameEvent : Event
    {
/// <summary>Initializes a new instance of the <see cref="EndOfGameEvent"/> type.</summary>
        [JsonConstructor]
        public EndOfGameEvent()
        {
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override object Clone()
        {
            return new EndOfGameEvent();
        }
    }
}
