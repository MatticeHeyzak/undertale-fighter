namespace UndertaleBattle.Core.Models;

/// <summary>
/// A consumable battle item.
/// </summary>
public sealed class Item
{
    public string Name { get; init; } = "Item";
    public int HealAmount { get; init; }

    public string UseDialogueTemplate { get; init; } = "You used the {0}. Nothing much really happened.";
    
    public string BuildUseDialogue() => string.Format(UseDialogueTemplate, Name);
}