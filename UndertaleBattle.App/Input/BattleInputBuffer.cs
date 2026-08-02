using System.Numerics;
using UndertaleBattle.Core.Input;

namespace UndertaleBattle.Input;

/// <summary>
/// Retains edge-trigger input until a fixed simulation step consumes it.
/// Movement remains the most recently sampled held direction.
/// </summary>
public sealed class BattleInputBuffer
{
    private Vector2 _movement;
    private bool _leftPressed;
    private bool _rightPressed;
    private bool _upPressed;
    private bool _downPressed;
    private bool _confirmPressed;
    private bool _cancelPressed;

    public void Capture(BattleInput input)
    {
        _movement = input.Movement;
        _leftPressed |= input.LeftPressed;
        _rightPressed |= input.RightPressed;
        _upPressed |= input.UpPressed;
        _downPressed |= input.DownPressed;
        _confirmPressed |= input.ConfirmPressed;
        _cancelPressed |= input.CancelPressed;
    }

    public BattleInput Consume()
    {
        var input = new BattleInput(
            _movement,
            _leftPressed,
            _rightPressed,
            _upPressed,
            _downPressed,
            _confirmPressed,
            _cancelPressed);

        ClearPressedButtons();
        return input;
    }

    public void Clear()
    {
        _movement = Vector2.Zero;
        ClearPressedButtons();
    }

    private void ClearPressedButtons()
    {
        _leftPressed = false;
        _rightPressed = false;
        _upPressed = false;
        _downPressed = false;
        _confirmPressed = false;
        _cancelPressed = false;
    }
}