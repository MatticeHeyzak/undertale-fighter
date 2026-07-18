using System.Numerics;
using UndertaleBattle.Core.Enums;

namespace UndertaleBattle.Core.Input;

public interface IInputState
{
    Vector2 MovementDirection { get; }
    bool IsConfirmPressed { get; }
    bool IsCancelPressed { get; }
    MenuInput MenuInput { get; }
}