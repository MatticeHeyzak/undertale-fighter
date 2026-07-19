using Raylib_cs;
using System.Numerics;
using UndertaleBattle.Assets;
using UndertaleBattle.Core.Assets;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Interfaces;

namespace UndertaleBattle.Renderers;

public sealed class SharedRenderer : IRaylibRenderer
{
    private readonly IAssetStore _assets;

    public SharedRenderer(IAssetStore assets)
    {
        _assets = assets;
    }

    public void Draw(BattleContext context)
    {
        DrawArena(context);
    }

    private static void DrawArena(BattleContext context)
    {
        var a = context.Arena;
        // 1. Pack your coordinates into a Raylib Rectangle structure
        Rectangle rect = new Rectangle(a.Left, a.Top, a.Width, a.Height);

        // 2. Set your desired line thickness (e.g., 4.0f)
        float thickness = 4f; 

        // 3. Draw using the extended function
        Raylib.DrawRectangleLinesEx(rect, thickness, Color.White);
    }
}