using System;
using Newtonsoft.Json;

namespace CardGameEngine
{
    [Serializable]
    public class StartOfTurnEvent : Event
    {
        [JsonConstructor]
        public StartOfTurnEvent()
        {
        }

        public override object Clone()
        {
            return new StartOfTurnEvent();
        }
    }
}
