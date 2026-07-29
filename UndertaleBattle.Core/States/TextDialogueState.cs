using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.States;

/// <summary>
/// Reveals the current dialogue text over time, then transitions to the
/// configured next state once the player confirms it.
/// </summary>
public sealed class TextDialogueState : IBattleState
{
    public BattleStateIdentity Identity => BattleStateIdentity.TextDialogue;

    private const float CharactersPerSecond = 30f;

    private float _elapsed;

    public void Enter(BattleContext context)
    {
        _elapsed = 0f;
        
        // ShowDialogue calls Begin before transitioning here. This protects
        // against accidentally entering dialogue without configured text.
        context.Dialogue.RevealCharacters(0);
        
        context.ClearTransientInput();
    }

    public void Update(BattleContext context, float deltaTime)
    {
        var dialogue = context.Dialogue;

        if (!dialogue.IsFullyVisible)
        {
            _elapsed += deltaTime;
            
            int characterCount = (int)(_elapsed * CharactersPerSecond);
            dialogue.RevealCharacters(characterCount);
            
            // Confirm during typing completes the line rather than being ignored.
            if (context.PendingMenuInput == MenuInput.Confirm)
            {
                dialogue.RevealAll();
                context.ClearTransientInput();
            }

            return;
        }

        if (context.PendingMenuInput != MenuInput.Confirm)
            return;
        
        var nextState = dialogue.NextState;
        
        context.ClearTransientInput();
        context.StateMachine.ChangeState(nextState, context);
    }
    public void Exit(BattleContext context) {}
}