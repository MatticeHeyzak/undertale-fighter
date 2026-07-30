using System.Numerics;
using Raylib_cs;
using UndertaleBattle.Assets;
using UndertaleBattle.Core.Assets;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Runtime;
using UndertaleBattle.Interfaces;

namespace UndertaleBattle.Renderers.States;

public sealed class DialogueRenderer : IStateRenderer
{
    private const int FontSize = 35;
    private const int LineSpacing = 4;
    private const int BoxPadding = 16;

    private readonly IAssetStore _assets;

    public BattleStateIdentity TargetState => BattleStateIdentity.TextDialogue;

    public DialogueRenderer(IAssetStore assets)
    {
        _assets = assets;
    }

    public void Draw(BattleSession session)
    {
        var arena = session.Arena.Shape;

        var boxRect = new Rectangle(
            arena.Left,
            arena.Top,
            arena.Width,
            arena.Height);

        Raylib.DrawRectangleLinesEx(boxRect, 4f, Color.White);

        var dialogue = session.Dialogue;

        string visibleText = dialogue.Text.Length == 0
            ? string.Empty
            : dialogue.Text[..Math.Min(
                dialogue.VisibleCharacterCount,
                dialogue.Text.Length)];

        var textPosition = new Vector2(
            arena.Left + BoxPadding,
            arena.Top + BoxPadding);

        if (_assets.TryGetFont(AssetKey.Fonts.Dialogue, out var font))
        {
            Raylib.DrawTextEx(
                font,
                visibleText,
                textPosition,
                FontSize,
                LineSpacing,
                Color.White);
        }
        else
        {
            Raylib.DrawText(
                visibleText,
                (int)textPosition.X,
                (int)textPosition.Y,
                FontSize,
                Color.White);
        }
    }
}