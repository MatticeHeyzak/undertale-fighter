using UndertaleBattle.Core.Enums;

namespace UndertaleBattle.Core.Context.StateData;

public sealed class DialogueStateData
{
    public string CurrentDialog { get; set; } = string.Empty;
    public int VisibleCharCount { get; set; }
    
    /// <summary>
    /// State to transition to once the current dialogue is fully read and confirmed.
    /// Set this right before switching to <see cref="BattleStateIdentity.TextDialogue"/>.
    /// </summary>
    public BattleStateIdentity NextState { get; set; }
}