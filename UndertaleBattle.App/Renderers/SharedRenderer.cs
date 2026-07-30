using System.Numerics;
using Raylib_cs;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Runtime;
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

    public void Draw(
        BattleSession session,
        BattleStateIdentity currentState)
    {
        DrawArena(session);

        if (currentState == BattleStateIdentity.Menu)
            DrawMenuButtons();
    }

    private static void DrawArena(BattleSession session)
    {
        var arena = session.Arena.Shape;

        var rectangle = new Rectangle(
            arena.Left,
            arena.Top,
            arena.Width,
            arena.Height);

        Raylib.DrawRectangleLinesEx(rectangle, 4f, Color.White);
    }

    private void DrawMenuButtons()
    {
        for (int index = 0; index < MenuButtonLayout.ButtonCount; index++)
        {
            var sprite = _sprites.Get(MenuButtonLayout.Keys[index]);
            if (sprite is null)
                continue;

            var position = MenuButtonLayout.PositionFor(index);

            Raylib.DrawTexturePro(
                sprite.Texture,
                sprite.SourceRect,
                sprite.DestRect(position),
                Vector2.Zero,
                0f,
                Color.White);
        }
    }
}