namespace CardGameEngine;

public abstract class Event : Action
{
    public Event()
    {
    }

    /// <summary>
    /// Executes the event. Events are not meant to alter the game state,
    /// but rather to trigger reactions or other effects that may occur as a result of the event.
    /// </summary>
    /// <param name="game"></param>
    public override void Execute(IEngine game)
    {

    }

    public override bool IsExecutable(IEngineState gameState)
    {
        return true;
    }
}
