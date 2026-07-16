using System;

namespace CardGameEngine
{
    [Serializable]
    public class StartDrawCardEvent : Event
    {
        public StartDrawCardEvent()
        {
        }

        public override object Clone()
        {
            return new StartDrawCardEvent();
        }
    }
}
