using Raylib_cs;
using System.Numerics;
using UndertaleBattle.Core.Assets;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Interfaces;
using UndertaleBattle.Rendering;

namespace UndertaleBattle.Renderers.States;

public sealed class PlayerDodgingRenderer : IStateRenderer
{
    public BattleStateIdentity TargetState => BattleStateIdentity.PlayerDodging;

    private readonly SpriteStore _sprites;

    public PlayerDodgingRenderer(SpriteStore sprites)
    {
        _sprites = sprites;
    }

    public void Draw(BattleContext context)
    {
        DrawSoul(context);
        DrawBullets(context);
    }

    private void DrawSoul(BattleContext context)
    {
        var soul   = context.PlayerSoul;
        var sprite = _sprites.Get(AssetKey.Soul.Heart);

        if (sprite is not null)
        {
            Raylib.DrawTexturePro(
                sprite.Texture, sprite.SourceRect,
                sprite.DestRect(soul.Position),
                Vector2.Zero, 0f, Color.White);
        }
        else
        {
            Raylib.DrawCircleV(soul.Position, soul.Radius, Color.Red);
        }
    }

    private void DrawBullets(BattleContext context)
    {
        // todo
    }
}