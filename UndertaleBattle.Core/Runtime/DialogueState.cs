using UndertaleBattle.Core.Enums;

namespace UndertaleBattle.Core.Runtime;

/// <summary>
/// Runtime data for the current displayed dialogue line.
/// </summary>
public sealed class DialogueState
{
    public string Text { get; private set; } = string.Empty;
    
    public int VisibleCharacterCount { get; private set; }
    
    public BattleStateIdentity ContinueWith { get; private set; }
    
    public bool IsFullyVisible => VisibleCharacterCount >= Text.Length;

    public void Begin(string text, BattleStateIdentity continueWith)
    {
        ArgumentNullException.ThrowIfNull(text);

        Text = text;
        ContinueWith = continueWith;
        VisibleCharacterCount = 0;
    }

    public void RevealCharacters(int count)
    {
        VisibleCharacterCount = Math.Clamp(count, 0, Text.Length);
    }

    public void RevealAll()
    {
        VisibleCharacterCount = Text.Length;
    }

    public void Clear()
    {
        Text = string.Empty;
        VisibleCharacterCount = 0;
    }
}