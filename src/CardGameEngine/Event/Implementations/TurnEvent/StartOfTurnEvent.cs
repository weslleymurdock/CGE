using System;
using Newtonsoft.Json;

namespace CardGameEngine
{
    [Serializable]
    public class StartOfTurnEvent : Event
    {
        [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="StartOfTurnEvent"/> type.</summary>
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
