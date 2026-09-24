using System;

namespace CardGameEngine
{
    [Serializable]
    public class StartDrawCardEvent : Event
    {
/// <summary>Initializes a new instance of the <see cref="StartDrawCardEvent"/> type.</summary>
        public StartDrawCardEvent()
        {
        }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public override object Clone()
        {
            return new StartDrawCardEvent();
        }
    }
}
