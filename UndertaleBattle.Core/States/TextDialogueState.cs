using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.States;

/// <summary>
/// Drives the typewriter effect and waits for player confirmation, then
/// transitions to whatever <see cref="BattleContext.DialogueNextState"/> was
/// set by the caller (see <see cref="BattleContext.ShowDialogue"/>).
/// Reusable across Fight/Act/Item/Mercy resolutions, boss intro text, etc.
/// </summary>
public sealed class TextDialogueState : IBattleState
{
    public BattleStateIdentity Identity => BattleStateIdentity.TextDialogue;

    private const float CharsPerSecond = 30f;
    private float _elapsed;

    public void Enter(BattleContext context)
    {
        _elapsed = 0f;
        context.Dialogue.VisibleCharCount = 0;
    }

    public void Update(BattleContext context, float deltaTime)
    {
        var dialogue = context.Dialogue;
        bool fullyRevealed = dialogue.VisibleCharCount >= dialogue.CurrentDialog.Length;

        if (!fullyRevealed)
        {
            _elapsed += deltaTime;
            dialogue.VisibleCharCount = Math.Min((int)(_elapsed * CharsPerSecond), dialogue.CurrentDialog.Length);
            return;
        }

        if (context.PendingMenuInput == MenuInput.Confirm)
        {
            context.PendingMenuInput = MenuInput.None;
            context.StateMachine.ChangeState(dialogue.NextState, context);
        }
    }

    public void Exit(BattleContext context) { }
}