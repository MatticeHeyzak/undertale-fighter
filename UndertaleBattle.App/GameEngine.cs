using Raylib_cs;
using UndertaleBattle.Assets;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Input;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Interfaces;

namespace UndertaleBattle;

public sealed class GameEngine
{
    private readonly BattleContext _context;
    private readonly IBattleStateMachine _stateMachine;
    private readonly IRaylibRenderer _renderer;
    private readonly AssetStore _assets;
    private readonly IInputState _input;
    
    public GameEngine(
        BattleContext context,
        IBattleStateMachine stateMachine,
        IRaylibRenderer renderer,
        AssetStore assets,
        IInputState input)
    {
        _context = context;
        _stateMachine = stateMachine;
        _renderer = renderer;
        _assets = assets;
        _input = input;
    }

    public void Run()
    {
        Raylib.InitWindow(Settings.ScreenWidth, Settings.ScreenHeight, "Undertale Battle");
        Raylib.SetTargetFPS(60);
        
        _assets.LoadAll();

        while (!Raylib.WindowShouldClose())
        {
            ApplyInput();
            
            // Clamp to guard against spiral-of-death on frame hitches (e.g. asset load stalls)
            float deltaTime = Math.Min(Raylib.GetFrameTime(), 1f / 30f);
            _stateMachine.Update(_context, deltaTime);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            _renderer.Draw(_context);
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    private void ApplyInput()
    {
        _input.Poll();

        _context.MovementInput = _input.MovementDirection;
        _context.PendingMenuInput = _input.MenuInput;
    }
}