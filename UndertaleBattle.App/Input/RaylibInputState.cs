using System.Numerics;
using Raylib_cs;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Input;

namespace UndertaleBattle.Input;

public sealed class RaylibInputState : IInputState
{
    public Vector2 MovementDirection { get; private set; }
    public bool IsConfirmPressed { get; private set; }
    public bool IsCancelPressed { get; private set; }
    public MenuInput MenuInput { get; private set; }

    public void Poll()
    {
        var dir = Vector2.Zero;
        if (Raylib.IsKeyDown(KeyboardKey.Left)) dir.X -= 1;
        if (Raylib.IsKeyDown(KeyboardKey.Right)) dir.X += 1;
        if (Raylib.IsKeyDown(KeyboardKey.Up)) dir.Y -= 1;
        if (Raylib.IsKeyDown(KeyboardKey.Down)) dir.Y += 1;
        MovementDirection = dir;

        IsConfirmPressed = Raylib.IsKeyPressed(KeyboardKey.Z) || Raylib.IsKeyPressed(KeyboardKey.Enter);
        IsCancelPressed = Raylib.IsKeyPressed(KeyboardKey.X);

        MenuInput = IsConfirmPressed ? MenuInput.Confirm
            : Raylib.IsKeyPressed(KeyboardKey.Left) ? MenuInput.Left
            : Raylib.IsKeyPressed(KeyboardKey.Right) ? MenuInput.Right
            : Raylib.IsKeyPressed(KeyboardKey.Up) ? MenuInput.Up
            : Raylib.IsKeyPressed(KeyboardKey.Down) ? MenuInput.Down
            : MenuInput.None;
    }
}