using Raylib_cs;
using System.Numerics;
using UndertaleBattle.Assets;
using UndertaleBattle.Core.Assets;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Interfaces;

namespace UndertaleBattle.Renderers;

public sealed class ComponentRenderer : IRaylibRenderer
{
    private readonly IAssetStore _assets;

    private static readonly string[] ButtonKeys =
    {
        AssetKey.UI.ButtonFight,
        AssetKey.UI.ButtonAct,
        AssetKey.UI.ButtonItem,
        AssetKey.UI.ButtonMercy,
    };
    
    private static readonly string[] ButtonActiveKeys =
    {
        AssetKey.UI.ButtonFightActive,
        AssetKey.UI.ButtonActActive,
        AssetKey.UI.ButtonItemActive,
        AssetKey.UI.ButtonMercyActive,
    };

    public ComponentRenderer(IAssetStore assets)
    {
        _assets = assets;
    }
    
    public void Draw(BattleContext context)
    {
        DrawArena(context);
        DrawSoul(context);
        DrawMenuButtons(context);
        DrawHud(context);
    }
    
    private static void DrawArena(BattleContext context)
    {
        var a = context.Arena;
        Raylib.DrawRectangleLines((int)a.Left, (int)a.Top, (int)a.Width, (int)a.Height, Color.White);
    }

    private void DrawSoul(BattleContext context)
    {
        var soul = context.PlayerSoul;
        // Use sprite if available, otherwise fall back to circle
        if (_assets.TryGetTexture(AssetKey.Soul.Heart, out var tex))
            Raylib.DrawTextureV(tex, soul.Position - new Vector2(tex.Width / 2f, tex.Height / 2f), Color.White);
    }

    private void DrawMenuButtons(BattleContext context)
    {
        const int startX = 50;
        const int startY = 480;
        const int spacing = 160;

        for (int i = 0; i < ButtonKeys.Length; i++)
        {
            bool isSelected = context.SelectedMenuIndex == i;
            string key = isSelected ? ButtonActiveKeys[i] : ButtonKeys[i];

            if (_assets.TryGetTexture(key, out var tex))
                Raylib.DrawTexture(tex, startX + i * spacing, startY, Color.White);
        }
    }

    private void DrawHud(BattleContext context)
    {
        Font font = _assets.TryGetFont(AssetKey.Fonts.Main, out var f) ? f : Raylib.GetFontDefault();
        Raylib.DrawTextEx(font, $"HP: {context.PlayerSoul.Health}", new Vector2(10, 10), 20, 1, Color.White);
    }
}