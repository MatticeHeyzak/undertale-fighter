using Raylib_cs;
using UndertaleBattle.Assets;
using UndertaleBattle.Core;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Input;
using UndertaleBattle.Interfaces;
using UndertaleBattle.Input;
using UndertaleBattle.Rendering;

namespace UndertaleBattle;

public sealed class GameEngine
{
    private const float FixedDeltaTime = 1f / 120f;
    private const float MaximumFrameTime = 0.25f;

    private readonly IBattleFactory _battleFactory;
    private readonly IRaylibRenderer _renderer;
    private readonly AssetStore _assets;
    private readonly IBattleInputSource _input;

    public GameEngine(
        IBattleFactory battleFactory,
        IRaylibRenderer renderer,
        AssetStore assets,
        IBattleInputSource input)
    {
        _battleFactory = battleFactory ??
            throw new ArgumentNullException(nameof(battleFactory));
        _renderer = renderer ??
            throw new ArgumentNullException(nameof(renderer));
        _assets = assets ??
            throw new ArgumentNullException(nameof(assets));
        _input = input ??
            throw new ArgumentNullException(nameof(input));
    }

    public void Run()
    {
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);

        Raylib.InitWindow(
            Settings.InitialWindowWidth,
            Settings.InitialWindowHeight,
            "Undertale Battle");
        
        Raylib.SetTargetFPS(60);
        _assets.LoadAll();

        VirtualViewport? viewport = null;

        try
        {
            viewport = new VirtualViewport();
            viewport.Initialize();
            
            BattleSimulation simulation = CreateStartedBattle();
            var inputBuffer = new BattleInputBuffer();
            float accumulator = 0f;

            while (!Raylib.WindowShouldClose())
            {
                inputBuffer.Capture(_input.Poll());

                if (simulation.Session.IsComplete)
                {
                    BattleInput resultInput = inputBuffer.Consume();

                    if (resultInput.CancelPressed)
                        break;

                    if (resultInput.ConfirmPressed)
                    {
                        simulation = CreateStartedBattle();
                        accumulator = 0f;
                        inputBuffer.Clear();
                    }
                }
                else
                {
                    accumulator += Math.Min(
                        Raylib.GetFrameTime(),
                        MaximumFrameTime);

                    while (accumulator >= FixedDeltaTime)
                    {
                        simulation.Update(
                            inputBuffer.Consume(),
                            FixedDeltaTime);

                        accumulator -= FixedDeltaTime;

                        if (simulation.Session.IsComplete)
                        {
                            accumulator = 0f;
                            break;
                        }
                    }
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                
                viewport.BeginScene();
                
                _renderer.Draw(
                    simulation.Session,
                    simulation.CurrentState);
                
                viewport.EndScene();
                viewport.Present();
                
                Raylib.EndDrawing();
            }
        }
        finally
        {
            viewport?.Dispose();
            _assets.Dispose();
            Raylib.CloseWindow();
        }
    }

    private BattleSimulation CreateStartedBattle()
    {
        BattleSimulation simulation = _battleFactory.Create();
        simulation.Start(BattleStateIdentity.Menu);
        return simulation;
    }
}