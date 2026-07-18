using Raylib_cs;
using System.Numerics;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Interfaces;

namespace UndertaleBattle.Renderers;

public sealed class ComponentRenderer : IRaylibRenderer
{
    public void Draw(BattleContext context)
    {
        DrawArena(context);
        DrawSoul(context);
        DrawBullets(context);
        DrawHud(context);
    }

    private static void DrawArena(BattleContext context)
    {
        var a = context.Arena;
        Raylib.DrawRectangleLines(
            (int)a.Left, (int)a.Top, (int)a.Width, (int)a.Height, Color.White);
    }

    private static void DrawSoul(BattleContext context)
    {
        var soul = context.PlayerSoul;
        Raylib.DrawCircleV(soul.Position, soul.Radius, Color.Red);
    }

    private static void DrawBullets(BattleContext context)
    {
        foreach (var bullet in context.Bullets)
            Raylib.DrawCircleV(bullet.Position, bullet.Radius, Color.White);
    }

    private static void DrawHud(BattleContext context)
    {
        Raylib.DrawText($"HP: {context.PlayerSoul.Health}", 10, 10, 20, Color.White);
    }
}