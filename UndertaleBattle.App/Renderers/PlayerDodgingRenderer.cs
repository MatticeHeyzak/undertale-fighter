using System.Numerics;
using Raylib_cs;
using UndertaleBattle.Core.Assets;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Runtime;
using UndertaleBattle.Interfaces;
using UndertaleBattle.Rendering;

namespace UndertaleBattle.Renderers.States;

public sealed class PlayerDodgingRenderer : IStateRenderer
{
    private readonly SpriteStore _sprites;

    public BattleStateIdentity TargetState => BattleStateIdentity.PlayerDodging;

    public PlayerDodgingRenderer(SpriteStore sprites)
    {
        _sprites = sprites;
    }

    public void Draw(BattleSession session)
    {
        DrawSoul(session);
        DrawBullets(session);
    }

    private void DrawSoul(BattleSession session)
    {
        var soul = session.Player;
        var sprite = _sprites.Get(AssetKey.Soul.Heart);

        if (sprite is not null)
        {
            Raylib.DrawTexturePro(
                sprite.Texture,
                sprite.SourceRect,
                sprite.DestRect(soul.Position),
                Vector2.Zero,
                0f,
                Color.White);

            return;
        }

        Raylib.DrawCircleV(
            soul.Position,
            soul.Radius,
            Color.Red);
    }

    private static void DrawBullets(BattleSession session)
    {
        foreach (var bullet in session.Combat.Projectiles)
        {
            Raylib.DrawCircleV(
                bullet.Position,
                bullet.Radius,
                Color.White);
        }
    }
}