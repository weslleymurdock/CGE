using System;
using Newtonsoft.Json;

namespace CardGameEngine
{
    [Serializable]
    public class EndOfTurnEvent : Event
    {
/// <summary>Initializes a new instance of the <see cref="EndOfTurnEvent"/> type.</summary>
        [JsonConstructor]
        public EndOfTurnEvent()
        {
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override object Clone()
        {
            return new EndOfTurnEvent();
        }
    }
}
