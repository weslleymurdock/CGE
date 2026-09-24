using System;
using Newtonsoft.Json;

namespace CardGameEngine;

[Serializable]
public class AfterActionEvent : ActionEvent
{
    [JsonConstructor]
/// <summary>Initializes a new instance of the <see cref="AfterActionEvent"/> type.</summary>
/// <param name="base(action">The base(action value.</param>
    public AfterActionEvent(IAction action) : base(action)
    {
    }

/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
    public override object Clone()
    {
        return new AfterActionEvent((IAction)Action.Clone());
    }

/// <summary>Performs the IsAfter operation.</summary>
/// <param name="type">The type value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsAfter(Type type)
    {
        return type.IsAssignableFrom(Action.GetType());
    }

/// <summary>Performs the IsBefore operation.</summary>
/// <param name="type">The type value.</param>
/// <returns>The result of the operation.</returns>
    public override bool IsBefore(Type type)
    {
        return false;
    }
}
