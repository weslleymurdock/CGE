using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public abstract class ActionEvent : Event, IActionEvent
{
    public IAction Action { get; protected set; }

/// <summary>Initializes a new instance of the <see cref="ActionEvent"/> type.</summary>
/// <param name="action">The action value.</param>
    [JsonConstructor]
    public ActionEvent(IAction action)
    {
        Action = action;
    }

/// <summary>Performs the IsBefore operation.</summary>
/// <param name="type">The type value.</param>
/// <returns>The result of the operation.</returns>
    public abstract bool IsBefore(Type type);

/// <summary>Performs the IsAfter operation.</summary>
/// <param name="type">The type value.</param>
/// <returns>The result of the operation.</returns>
    public abstract bool IsAfter(Type type);
}
