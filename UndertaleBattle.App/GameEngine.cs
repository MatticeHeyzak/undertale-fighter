using Raylib_cs;
using UndertaleBattle.Assets;
using UndertaleBattle.Core;
using UndertaleBattle.Core.Input;
using UndertaleBattle.Interfaces;

namespace UndertaleBattle;

public sealed class GameEngine
{
    private readonly BattleSimulation _simulation;
    private readonly IRaylibRenderer _renderer;
    private readonly AssetStore _assets;
    private readonly IBattleInputSource _input;

    public GameEngine(
        BattleSimulation simulation,
        IRaylibRenderer renderer,
        AssetStore assets,
        IBattleInputSource input)
    {
        _simulation = simulation ?? throw new ArgumentNullException(nameof(simulation));
        _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        _assets = assets ?? throw new ArgumentNullException(nameof(assets));
        _input = input ?? throw new ArgumentNullException(nameof(input));
    }

    public void Run()
    {
        Raylib.InitWindow(
            Settings.ScreenWidth,
            Settings.ScreenHeight,
            "Undertale Battle");

        Raylib.SetTargetFPS(60);

        _assets.LoadAll();

        try
        {
            while (!Raylib.WindowShouldClose())
            {
                var input = _input.Poll();

                float deltaTime = Math.Min(
                    Raylib.GetFrameTime(),
                    1f / 30f);

                _simulation.Update(input, deltaTime);

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                _renderer.Draw(
                    _simulation.Session,
                    _simulation.CurrentState);

                Raylib.EndDrawing();
            }
        }
        finally
        {
            _assets.Dispose();
            Raylib.CloseWindow();
        }
    }
}