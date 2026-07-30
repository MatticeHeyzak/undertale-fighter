using System.Numerics;
using Raylib_cs;
using UndertaleBattle.Core.Assets;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Runtime;
using UndertaleBattle.Interfaces;
using UndertaleBattle.Rendering;

namespace UndertaleBattle.Renderers.States;

public sealed class AttackQteRenderer : IStateRenderer
{
    private const float MarkerWidth = 15f;
    private const float FlashesPerSecond = 5f;

    private readonly SpriteStore _sprites;

    public BattleStateIdentity TargetState => BattleStateIdentity.AttackQte;

    public AttackQteRenderer(SpriteStore sprites)
    {
        _sprites = sprites;
    }

    public void Draw(BattleSession session)
    {
        Rectangle arenaRect = ToRectangle(session.Arena.Shape);
        DrawStretched(AssetKey.UI.AttackBackground, arenaRect);
        DrawMarker(session, arenaRect);
    }

    private void DrawMarker(
        BattleSession session,
        Rectangle arenaRect)
    {
        string key = ShouldShowAlt(session.Ui.AttackQte.FlashTimer)
            ? AssetKey.UI.AttackBarAlt
            : AssetKey.UI.AttackBar;
        
        float travel = arenaRect.Width - MarkerWidth;
        float x = arenaRect.X + travel * session.Ui.AttackQte.MeterPosition;

        var markerRect = new Rectangle(x, arenaRect.Y, MarkerWidth, arenaRect.Height);
        var sprite = _sprites.Get(key);
        if (sprite is null)
            return;
        
        Raylib.DrawTexturePro(sprite.Texture, sprite.SourceRect, markerRect, Vector2.Zero, 0f, Color.White);
    }

    private static bool ShouldShowAlt(float flashTimer)
    {
        if (flashTimer <= 0f)
            return false;

        float period = 1f / FlashesPerSecond;
        return flashTimer % period < period / 2f;
    }

    private void DrawStretched(
        string key,
        Rectangle destinationRect)
    {
        var sprite = _sprites.Get(key);

        if (sprite is null)
            return;

        Raylib.DrawTexturePro(
            sprite.Texture,
            sprite.SourceRect,
            destinationRect,
            Vector2.Zero,
            0f,
            Color.White);
    }

    private static Rectangle ToRectangle(IArenaShape arena)
    {
        return new Rectangle(
            arena.Left,
            arena.Top,
            arena.Width,
            arena.Height);
    }
}