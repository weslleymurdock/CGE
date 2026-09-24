using System.Collections.Generic;

namespace CardGameEngine;

public interface ITargetlessSpellCardComponent : ISpellCardComponent, ITargetless
{
    /// <summary>
    /// Called when spell card is cast. Execute Actions here.
    /// </summary>
    /// <param name="game"></param>
    void Cast(IGame game);
}
