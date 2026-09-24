using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class ActionQueue : IActionQueue
{
    [JsonProperty]
    protected bool isGameOver = false;

    public bool ExecuteReactions { get; set; }

/// <summary>Initializes a new instance of the <see cref="ActionQueue"/> type.</summary>
/// <param name="executeReactions">The executeReactions value.</param>
    public ActionQueue(bool executeReactions = true)
        : this(executeReactions, false)
    {
    }

/// <summary>Initializes a new instance of the <see cref="ActionQueue"/> type.</summary>
/// <param name="executeReactions">The executeReactions value.</param>
/// <param name="isGameOver">The isGameOver value.</param>
    [JsonConstructor]
    protected ActionQueue(bool executeReactions, bool isGameOver)
    {
        ExecuteReactions = executeReactions;
        this.isGameOver = isGameOver;
    }

/// <summary>Executes this operation against the specified game.</summary>
/// <param name="game">The game value.</param>
/// <param name="action">The action value.</param>
    public virtual void Execute(IGame game, IAction action)
    {
        if (!isGameOver && !action.IsAborted && action.IsExecutable(game))
        {
            ExecReactions(game, new BeforeActionEvent(action));
            if (!action.IsAborted)
            {
                action.Execute(game);
                ExecReactions(game, new AfterActionEvent(action));
            }
        }

        if (action is EndOfGameEvent)
        {
            isGameOver = true;
        }
    }

/// <summary>Performs the ExecReactions operation.</summary>
/// <param name="game">The game value.</param>
/// <param name="actionEvent">The actionEvent value.</param>
    private void ExecReactions(IGame game, IActionEvent actionEvent)
    {
        if (ExecuteReactions)
        {
            game.ReactTo(game, actionEvent);
        }
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public virtual object Clone()
    {
        return new ActionQueue(ExecuteReactions, isGameOver);
    }
}
