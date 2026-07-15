using Newtonsoft.Json;

namespace CardGameEngine;

public abstract class PlayerReaction<T, TGame, TAction> : Reaction<T, TGame, TAction>, IPlayerReaction<T, TGame, TAction>
    where T : IEngineState
    where TGame : IEngine<T>
    where TAction : IAction<T>
{
    [JsonProperty]
    protected IPlayer parentPlayer = null!;

    protected PlayerReaction() { }

    public PlayerReaction(IPlayer parentPlayer)
    {
        this.parentPlayer = parentPlayer;
    }

    [JsonIgnore]
    public IPlayer ParentPlayer
    {
        get
        {
            return parentPlayer;
        }
    }
}
