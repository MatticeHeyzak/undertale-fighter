using System.Numerics;
using UndertaleBattle.Core.Enums;

namespace UndertaleBattle.Core.Input;

public interface IInputState
{
    Vector2 MovementDirection { get; }
    bool IsConfirmPressed { get; }
    bool IsCancelPressed { get; }
    MenuInput MenuInput { get; }

    /// <summary>
    /// Refreshes all input properties for the current frame.
    /// Must be called exactly once per frame, before the state machine updates.
    /// </summary>
    void Poll();
}