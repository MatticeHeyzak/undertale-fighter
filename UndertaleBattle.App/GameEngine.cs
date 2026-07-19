using Raylib_cs;
using UndertaleBattle.Assets;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Interfaces;

namespace UndertaleBattle;

public sealed class GameEngine
{
    private readonly BattleContext _context;
    private readonly IBattleStateMachine _stateMachine;
    private readonly IRaylibRenderer _renderer;
    private readonly AssetStore _assets;
    
    public GameEngine(BattleContext context, IBattleStateMachine stateMachine, IRaylibRenderer renderer, AssetStore assets)
    {
        _context     = context;
        _stateMachine = stateMachine;
        _renderer    = renderer;
        _assets = assets;
    }

    public void Run()
    {
        Raylib.InitWindow(800, 600, "Undertale Battle");
        Raylib.SetTargetFPS(60);
        
        _assets.LoadAll();

        while (!Raylib.WindowShouldClose())
        {
            PollInput();
            _stateMachine.Update(_context, Raylib.GetFrameTime());

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