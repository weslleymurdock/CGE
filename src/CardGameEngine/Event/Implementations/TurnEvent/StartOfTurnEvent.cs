using System;
using Newtonsoft.Json;

namespace CardGameEngine
{
    [Serializable]
    public class StartOfTurnEvent : Event
    {
/// <summary>Initializes a new instance of the <see cref="StartOfTurnEvent"/> type.</summary>
        [JsonConstructor]
        public StartOfTurnEvent()
        {
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override object Clone()
        {
            return new StartOfTurnEvent();
        }
    }
}
