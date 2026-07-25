using System.Numerics;
using Raylib_cs;
using UndertaleBattle.Core.Assets;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Interfaces;
using UndertaleBattle.Rendering;

namespace UndertaleBattle.Renderers;

public sealed class MenuRenderer : IStateRenderer
{
    public BattleStateIdentity TargetState => BattleStateIdentity.Menu;

    private readonly SpriteStore _sprites;

    public MenuRenderer(SpriteStore sprites)
    {
        _sprites = sprites;
    }

    public void Draw(BattleContext context)
    {
        string key = MenuButtonLayout.ActiveKeys[context.Menu.SelectedIndex];
        var sprite = _sprites.Get(key);
        if (sprite is null)
            return;

        var pos = MenuButtonLayout.PositionFor(context.Menu.SelectedIndex);
        Raylib.DrawTexturePro(sprite.Texture, sprite.SourceRect, sprite.DestRect(pos), Vector2.Zero, 0f, Color.White);
    }
}