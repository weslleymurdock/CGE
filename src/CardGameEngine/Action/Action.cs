using Newtonsoft.Json;

namespace CardGameEngine;

public abstract class Action : IAction
{
    [JsonProperty]
    protected bool isAborted;

    protected Action() { }

    public Action(bool isAborted = false)
    {
        this.isAborted = isAborted;
    }

    [JsonIgnore]
    public bool IsAborted
    {
        get => isAborted;
        set => isAborted = value;
    }

    public abstract void Execute(IEngine game);
    public abstract bool IsExecutable(IEngineState gameState);
}

public abstract class Action<T> : Action, IAction<T> where T : IEngineState
{
    protected Action() { }

    public Action(bool isAborted = false) : base(isAborted)
    {
    }

    public abstract bool IsExecutable(T gameState);

    public override bool IsExecutable(IEngineState gameState)
    {
        if (gameState is T s)
        {
            return IsExecutable(s);
        }
        else
        {
            return false;
        }
    }

    public abstract void Execute(IEngine<T> game);

    public override void Execute(IEngine game)
    {
        if (game is IEngine<T> g)
        {
            Execute(g);
        }
    }
}
