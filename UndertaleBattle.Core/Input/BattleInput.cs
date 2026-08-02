using System.Numerics;
using UndertaleBattle.Core.Enums;

namespace UndertaleBattle.Core.Input;

/// <summary>
/// Immutable input captured for one simulation frame.
/// The application creates this; Core reads it without storing it.
/// </summary>
public readonly record struct BattleInput(
    Vector2 Movement,
    bool LeftPressed,
    bool RightPressed,
    bool UpPressed,
    bool DownPressed,
    bool ConfirmPressed,
    bool CancelPressed)
{
    public static BattleInput None => new(
        Movement: Vector2.Zero,
        LeftPressed: false,
        RightPressed: false,
        UpPressed: false,
        DownPressed: false,
        ConfirmPressed: false,
        CancelPressed: false);
}