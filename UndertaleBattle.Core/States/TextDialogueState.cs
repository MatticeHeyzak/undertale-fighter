using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Input;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.States;

public sealed class TextDialogueState : IBattleState
{
    private const float CharactersPerSecond = 30f;

    private float _elapsed;

    public BattleStateIdentity Identity => BattleStateIdentity.TextDialogue;

    public BattleStateIdentity? Enter(BattleSession session)
    {
        _elapsed = 0f;
        session.Dialogue.RevealCharacters(0);

        return null;
    }

    public BattleStateIdentity? Update(
        BattleSession session,
        BattleInput input,
        float deltaTime)
    {
        if (!session.Dialogue.IsFullyVisible)
        {
            _elapsed += deltaTime;

            int visibleCharacters =
                (int)(_elapsed * CharactersPerSecond);

            session.Dialogue.RevealCharacters(visibleCharacters);

            if (input.ConfirmPressed)
                session.Dialogue.RevealAll();

            return null;
        }

        return input.ConfirmPressed
            ? session.Dialogue.ContinueWith
            : null;
    }

    public void Exit(BattleSession session)
    {
        session.Dialogue.Clear();
    }
}