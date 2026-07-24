using System.Numerics;
using Raylib_cs;
using UndertaleBattle.Core.Assets;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Models;
using UndertaleBattle.Interfaces;
using UndertaleBattle.Rendering;

namespace UndertaleBattle.Renderers.States;

public sealed class AttackQteRenderer : IStateRenderer
{
    public BattleStateIdentity TargetState => BattleStateIdentity.AttackQte;

    /// <summary>Marker width in pixels - independent of arena size so it stays a readable thin bar.</summary>
    private const float MarkerWidth = 15f;

    private readonly SpriteStore _sprites;
    private const float FlashesPerSecond = 5f;

    public AttackQteRenderer(SpriteStore sprites)
    {
        _sprites = sprites;
    }

    public void Draw(BattleContext context)
    {
        var arenaRect = ToRectangle(context.Arena);
        DrawStretched(AssetKey.UI.AttackBackground, arenaRect);
        DrawMarker(context, arenaRect);
    }

    private void DrawMarker(BattleContext context, Rectangle arenaRect)
    {
        Console.WriteLine("attack flash timer is: " + context.AttackFlashTimer);
        string key = ShouldShowAlt(context.AttackFlashTimer)
            ? AssetKey.UI.AttackBarAlt
            : AssetKey.UI.AttackBar;

        // Travels the full arena width while staying fully inside its bounds.
        float travel = arenaRect.Width - MarkerWidth;
        float x = arenaRect.X + travel * context.AttackMeterPosition;

        var markerRect = new Rectangle(x, arenaRect.Y, MarkerWidth, arenaRect.Height);
        DrawStretched(key, markerRect);
    }

    private static bool ShouldShowAlt(float attackFlashTimer)
    {
        if (attackFlashTimer <= 0)
            return false;
        
        float period = 1f / FlashesPerSecond;
        float phase = attackFlashTimer % period;
        return phase < period / 2f;
    }

    private void DrawStretched(string key, Rectangle destRect)
    {
        var sprite = _sprites.Get(key);
        if (sprite is null)
            return;

        Raylib.DrawTexturePro(sprite.Texture, sprite.SourceRect, destRect, Vector2.Zero, 0f, Color.White);
    }

    private static Rectangle ToRectangle(BattleArena arena) =>
        new(arena.Left, arena.Top, arena.Width, arena.Height);
}