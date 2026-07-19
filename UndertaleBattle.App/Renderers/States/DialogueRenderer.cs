using System.Numerics;
using Raylib_cs;
using UndertaleBattle.Assets;
using UndertaleBattle.Core.Assets;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.States;
using UndertaleBattle.Interfaces;

namespace UndertaleBattle.Renderers.States;

public class DialogueRenderer : IStateRenderer
{
    public BattleStateIdentity TargetState => BattleStateIdentity.TextDialogue;

    private const int FontSize = 35;
    private const int LineSpacing = 4;
    private const int BoxPadding = 16;

    private readonly IAssetStore _assets;

    public DialogueRenderer(IAssetStore assets)
    {
        _assets = assets;
    }

    public void Draw(BattleContext context)
    {
        var arena = context.Arena;
        var boxRect = new Rectangle(arena.Left, arena.Top, arena.Width, arena.Height);

        //Raylib.DrawRectangleRec(boxRect, Color.Black);
        Raylib.DrawRectangleLinesEx(boxRect, 4f, Color.White);

        string visibleText = context.CurrentDialog.Length == 0
            ? string.Empty
            : context.CurrentDialog[..Math.Min(context.VisibleDialogCharCount, context.CurrentDialog.Length)];

        var textPos = new Vector2(arena.Left + BoxPadding, arena.Top + BoxPadding);

        if (_assets.TryGetFont(AssetKey.Fonts.Dialogue, out var font))
            Raylib.DrawTextEx(font, visibleText, textPos, FontSize, LineSpacing, Color.White);
        else
            Raylib.DrawText(visibleText, (int)textPos.X, (int)textPos.Y, FontSize, Color.White);
    }
}