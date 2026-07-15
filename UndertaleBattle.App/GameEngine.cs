using Raylib_cs;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.RaylibApp.Interfaces;

namespace UndertaleBattle.RaylibApp;

public sealed class GameEngine
{
    private readonly BattleContext _context;
    private readonly IBattleStateMachine _stateMachine;
    private readonly IRaylibRenderer _renderer;

    public GameEngine(BattleContext context, IBattleStateMachine stateMachine, IRaylibRenderer renderer)
    {
        _context     = context;
        _stateMachine = stateMachine;
        _renderer    = renderer;
    }

    public void Run()
    {
        Raylib.InitWindow(800, 600, "Undertale Battle");
        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {
            float dt = Raylib.GetFrameTime();

            PollInput();
            _stateMachine.Update(_context, dt);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            _renderer.Draw(_context);
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    private void PollInput()
    {
        // Movement
        var dir = System.Numerics.Vector2.Zero;
        if (Raylib.IsKeyDown(KeyboardKey.Left))  dir.X -= 1;
        if (Raylib.IsKeyDown(KeyboardKey.Right)) dir.X += 1;
        if (Raylib.IsKeyDown(KeyboardKey.Up))    dir.Y -= 1;
        if (Raylib.IsKeyDown(KeyboardKey.Down))  dir.Y += 1;
        _context.MovementInput = dir;

        // Menu
        if (Raylib.IsKeyPressed(KeyboardKey.Z) || Raylib.IsKeyPressed(KeyboardKey.Enter))
            _context.PendingMenuInput = Core.Enums.MenuInput.Confirm;
        else if (Raylib.IsKeyPressed(KeyboardKey.Left))
            _context.PendingMenuInput = Core.Enums.MenuInput.Left;
        else if (Raylib.IsKeyPressed(KeyboardKey.Right))
            _context.PendingMenuInput = Core.Enums.MenuInput.Right;
    }
}