using System.Numerics;
using Raylib_cs;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Input;

namespace UndertaleBattle.Input;

public sealed class RaylibInputState : IBattleInputSource
{
    public BattleInput Poll()
    {
        Vector2 movement = ReadMovement();

        bool confirmPressed =
            Raylib.IsKeyPressed(KeyboardKey.Z) ||
            Raylib.IsKeyPressed(KeyboardKey.Enter);

        bool cancelPressed =
            Raylib.IsKeyPressed(KeyboardKey.X);

        MenuInput menuAction = confirmPressed
            ? MenuInput.Confirm
            : Raylib.IsKeyPressed(KeyboardKey.Left)
                ? MenuInput.Left
                : Raylib.IsKeyPressed(KeyboardKey.Right)
                    ? MenuInput.Right
                    : Raylib.IsKeyPressed(KeyboardKey.Up)
                        ? MenuInput.Up
                        : Raylib.IsKeyPressed(KeyboardKey.Down)
                            ? MenuInput.Down
                            : MenuInput.None;

        return new BattleInput(
            Movement: movement,
            MenuAction: menuAction,
            ConfirmPressed: confirmPressed,
            CancelPressed: cancelPressed);
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