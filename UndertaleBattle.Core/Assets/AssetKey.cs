namespace UndertaleBattle.Core.Assets;

/// <summary>
/// String-keyed constants that identify every asset in the game.
/// Core states and renderers reference these keys.
/// To add a new asset, add it here and register it in AssetManifest
/// </summary>
public static class AssetKey
{
    public static class UI
    {
        // Menu buttons
        public const string ButtonFight = "ui/button_fight";
        public const string ButtonFightActive = "ui/button_fight_active";
        public const string ButtonAct = "ui/button_act";
        public const string ButtonActActive = "ui/button_act_active";
        public const string ButtonItem = "ui/button_item";
        public const string ButtonItemActive = "ui/button_item_active";
        public const string ButtonMercy = "ui/button_mercy";
        public const string ButtonMercyActive = "ui/button_mercy_active";
        public const string AttackBackground = "ui/attack_background";
        public const string AttackBar = "ui/attack_bar";
        public const string AttackBarAlt = "ui/attack_bar_alt";
    }

    public static class Soul
    {
        public const string Heart = "soul/heart";
    }

    public static class Fonts
    {
        public const string Main = "fonts/main";
        public const string Dialogue = "fonts/dialogue";
    }
}