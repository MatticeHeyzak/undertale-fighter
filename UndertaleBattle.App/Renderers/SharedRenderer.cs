using Raylib_cs;
using System.Numerics;
using UndertaleBattle.Assets;
using UndertaleBattle.Core.Assets;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Interfaces;
using UndertaleBattle.Rendering;

namespace UndertaleBattle.Renderers;

public sealed class SharedRenderer : IRaylibRenderer
{
    private readonly SpriteStore _sprites;

    public SharedRenderer(SpriteStore sprites)
    {
        _sprites = sprites;
    }

    public void Draw(BattleContext context)
    {
        DrawArena(context);
        DrawMenuButtons();
    }

    private static void DrawArena(BattleContext context)
    {
        var a = context.Arena;
        var rect = new Rectangle(a.Left, a.Top, a.Width, a.Height);
        Raylib.DrawRectangleLinesEx(rect, 4f, Color.White);
    }

    private void DrawMenuButtons()
    {
        for (int i = 0; i < MenuButtonLayout.ButtonCount; i++)
        {
            var sprite = _sprites.Get(MenuButtonLayout.Keys[i]);
            if (sprite is null)
                continue;

            var pos = MenuButtonLayout.PositionFor(i);
            Raylib.DrawTexturePro(
                sprite.Texture, sprite.SourceRect,
                sprite.DestRect(pos),
                Vector2.Zero, 0f, Color.White);
        }
    }
}