using System.Numerics;
using UndertaleBattle.Core.Assets;

namespace UndertaleBattle.Renderers;

/// <summary>
/// Single source of truth for the FIGHT/ACT/ITEM/MERCY button keys and their
/// screen layout. Shared between SharedRenderer (base row, always visible)
/// and MenuRenderer (selection highlight) so the two can never drift apart.
/// </summary>
public static class MenuButtonLayout
{
    public const int ButtonCount = 4;

    public static readonly string[] Keys =
    {
        AssetKey.UI.ButtonFight, AssetKey.UI.ButtonAct,
        AssetKey.UI.ButtonItem,  AssetKey.UI.ButtonMercy,
    };

    public static readonly string[] ActiveKeys =
    {
        AssetKey.UI.ButtonFightActive, AssetKey.UI.ButtonActActive,
        AssetKey.UI.ButtonItemActive,  AssetKey.UI.ButtonMercyActive,
    };

    private const int StartY = Settings.VirtualHeight - 50;
    private const int StartX = 110;
    private const float SpaceBetween = Settings.VirtualWidth / 4f;

    public static Vector2 PositionFor(int index) => new(StartX + SpaceBetween * index, StartY);
}