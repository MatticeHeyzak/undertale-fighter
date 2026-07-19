using System.IO;
using UndertaleBattle.Core.Assets;

namespace UndertaleBattle.Assets;

/// <summary>
/// Maps every AssetKey constant to its file path on disk.
/// </summary>
public static class AssetManifest
{
    /// <summary>
    /// Root folder relative to the executable.
    /// </summary>
    private static readonly string Root = Path.Combine(AppContext.BaseDirectory, "Assets");

    public static IReadOnlyDictionary<string, string> Textures { get; } = new Dictionary<string, string>
    {
        // UI buttons
        [AssetKey.UI.ButtonFight] = $"{Root}\\UI\\button_fight.png",
        [AssetKey.UI.ButtonFightActive] = $"{Root}\\UI\\button_fight_active.png",
        [AssetKey.UI.ButtonAct] = $"{Root}\\UI\\button_act.png",
        [AssetKey.UI.ButtonActActive] = $"{Root}\\UI\\button_act_active.png",
        [AssetKey.UI.ButtonItem] = $"{Root}\\UI\\button_item.png",
        [AssetKey.UI.ButtonItemActive] = $"{Root}\\UI\\button_item_active.png",
        [AssetKey.UI.ButtonMercy] = $"{Root}\\UI\\button_mercy.png",
        [AssetKey.UI.ButtonMercyActive] = $"{Root}\\UI\\button_mercy_active.png",
        
        // Soul
        [AssetKey.Soul.Heart] = $"{Root}\\Soul\\heart.png",
    };
    
    public static IReadOnlyDictionary<string, string> Fonts { get; } = new Dictionary<string, string>
    {
        [AssetKey.Fonts.Main] = $"{Root}\\Fonts\\main.ttf",
    };
}