using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.States;

/// <summary>
/// Drives the typewriter effect and waits for player confirmation.
/// The renderer reads <see cref="BattleContext.CurrentDialog"/> and
/// <see cref="BattleContext.VisibleDialogCharCount"/> to draw the text.
/// </summary>
public sealed class TextDialogueState : IBattleState
{
    public BattleStateIdentity Identity => BattleStateIdentity.TextDialogue;

    private const float CharsPerSecond = 30f;
    private float _elapsed;
    private BattleStateIdentity _nextState;

    public TextDialogueState(BattleStateIdentity nextState)
    {
        _nextState = nextState;
    }

    public void Enter(BattleContext context)
    {
        _elapsed = 0f;
        context.VisibleDialogCharCount = 0;
    }

    public void Update(BattleContext context, float deltaTime)
    {
        bool fullyRevealed = context.VisibleDialogCharCount >= context.CurrentDialog.Length;

        if (!fullyRevealed)
        {
            _elapsed += deltaTime;
            context.VisibleDialogCharCount =
                Math.Min((int)(_elapsed * CharsPerSecond), context.CurrentDialog.Length);
            return;
        }

        // Fully revealed — wait for confirmation
        if (context.PendingMenuInput == MenuInput.Confirm)
        {
            context.PendingMenuInput = MenuInput.None;
            context.StateMachine.ChangeState(_nextState, context);
        }
    }

    public void Exit(BattleContext context) { }
}