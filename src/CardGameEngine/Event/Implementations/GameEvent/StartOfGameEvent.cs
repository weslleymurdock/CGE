using System;
using Newtonsoft.Json;

namespace CardGameEngine
{
    [Serializable]
    public class StartOfGameEvent : Event
    {
/// <summary>Initializes a new instance of the <see cref="StartOfGameEvent"/> type.</summary>
        [JsonConstructor]
        public StartOfGameEvent()
        {
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override object Clone()
        {
            return new StartOfGameEvent();
        }
    }
}
