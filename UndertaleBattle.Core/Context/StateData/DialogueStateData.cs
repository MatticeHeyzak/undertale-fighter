using UndertaleBattle.Core.Enums;

namespace UndertaleBattle.Core.Context.StateData;

public sealed class DialogueStateData
{
    public string Text { get; private set; } = string.Empty;
    
    public int VisibleCharacterCount { get; private set; }
    
    public BattleStateIdentity NextState { get; private set; }

    public bool IsFullyVisible => VisibleCharacterCount >= Text.Length;

    public void Begin(string text, BattleStateIdentity nextState)
    {
        ArgumentNullException.ThrowIfNull(text);

        Text = text;
        NextState = nextState;
        VisibleCharacterCount = 0;
    }

    public void RevealCharacters(int count)
        => VisibleCharacterCount = Math.Clamp(count, 0, Text.Length);

    public void RevealAll()
        => VisibleCharacterCount = Text.Length;
}