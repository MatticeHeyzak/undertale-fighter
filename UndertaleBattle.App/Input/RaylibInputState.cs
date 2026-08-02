using System.Numerics;
using Raylib_cs;
using UndertaleBattle.Core.Input;

namespace UndertaleBattle.Input;

public sealed class RaylibInputState : IBattleInputSource
{
    public BattleInput Poll()
    {
        return new BattleInput(
            Movement: ReadMovement(),
            LeftPressed: Raylib.IsKeyPressed(KeyboardKey.Left),
            RightPressed: Raylib.IsKeyPressed(KeyboardKey.Right),
            UpPressed: Raylib.IsKeyPressed(KeyboardKey.Up),
            DownPressed: Raylib.IsKeyPressed(KeyboardKey.Down),
            ConfirmPressed:
            Raylib.IsKeyPressed(KeyboardKey.Z) ||
            Raylib.IsKeyPressed(KeyboardKey.Enter),
            CancelPressed: Raylib.IsKeyPressed(KeyboardKey.X));
    }

    private static Vector2 ReadMovement()
    {
        var movement = Vector2.Zero;

        if (Raylib.IsKeyDown(KeyboardKey.Left))
            movement.X -= 1f;

        if (Raylib.IsKeyDown(KeyboardKey.Right))
            movement.X += 1f;

        if (Raylib.IsKeyDown(KeyboardKey.Up))
            movement.Y -= 1f;

        if (Raylib.IsKeyDown(KeyboardKey.Down))
            movement.Y += 1f;

        return movement;
    }
}