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
    
    private static readonly string[] ButtonKeys =
    {
        AssetKey.UI.ButtonFight, AssetKey.UI.ButtonAct,
        AssetKey.UI.ButtonItem,  AssetKey.UI.ButtonMercy,
    };
    private static readonly string[] ButtonActiveKeys =
    {
        AssetKey.UI.ButtonFightActive, AssetKey.UI.ButtonActActive,
        AssetKey.UI.ButtonItemActive,  AssetKey.UI.ButtonMercyActive,
    };
    
    public MenuRenderer(SpriteStore sprites)
    {
        _sprites = sprites;
    }

    public void Draw(BattleContext context)
    {
        const int startY = Settings.ScreenHeight - 50;
        const int startX = 110;
        const float spaceBetween = Settings.ScreenWidth / 4f;
        
        for (int i = 0; i < ButtonKeys.Length; i++)
        {
            bool selected = context.SelectedMenuIndex == i;
            string key = selected ? ButtonActiveKeys[i] : ButtonKeys[i];
            var pos = new Vector2(startX + spaceBetween * i, startY);
            var sprite = _sprites.Get(key);

            if (sprite is null)
                throw new NullReferenceException(nameof(sprite));

            Raylib.DrawTexturePro(
                sprite.Texture,
                sprite.SourceRect,
                sprite.DestRect(pos),
                Vector2.Zero, 0f, Color.White);
        }
    }
}