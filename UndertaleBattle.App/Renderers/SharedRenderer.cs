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

        DrawOutcomeOverlay(session);
    }
    
    private static void DrawOutcomeOverlay(BattleSession session)
    {
        if (session.Outcome == BattleOutcome.InProgress)
            return;

        string result = session.Outcome switch
        {
            BattleOutcome.EnemyDefeated => "YOU WON",
            BattleOutcome.PlayerDefeated => "YOU DIED",
            _ => throw new InvalidOperationException(
                $"Unhandled outcome '{session.Outcome}'.")
        };

        Raylib.DrawText(
            result,
            Settings.VirtualWidth / 2 - 90,
            Settings.VirtualHeight / 2 - 20,
            36,
            Color.White);

        Raylib.DrawText(
            "Z / ENTER: RETRY     X: EXIT",
            Settings.VirtualWidth / 2 - 190,
            Settings.VirtualHeight / 2 + 30,
            20,
            Color.White);
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