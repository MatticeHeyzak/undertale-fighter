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
        int selectedIndex = context.Menu.SelectedIndex;

        if (selectedIndex < 0 || selectedIndex >= MenuButtonLayout.ButtonCount)
            return;

        string key = MenuButtonLayout.ActiveKeys[selectedIndex];

        var sprite = _sprites.Get(key);
        if (sprite is null)
            return;

        var position = MenuButtonLayout.PositionFor(selectedIndex);

        Raylib.DrawTexturePro(
            sprite.Texture,
            sprite.SourceRect,
            sprite.DestRect(position),
            Vector2.Zero,
            0f,
            Color.White);
    }
}