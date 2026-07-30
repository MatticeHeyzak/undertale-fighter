namespace UndertaleBattle.Core.Runtime;

/// <summary>
/// Visual/runtime data used only by the player attack timing minigame.
/// </summary>
public sealed class AttackQteUiState
{
    public float MeterPosition { get; private set; }
    
    public float FlashTimer { get; private set; }

    public bool IsFlashing => FlashTimer > 0f;

    public void Reset()
    {
        MeterPosition = 0f;
        FlashTimer = 0f;
    }

    public void AdvanceMeter(float amount)
    {
        MeterPosition = Math.Clamp(MeterPosition + amount, 0f, 1f);
    }

    public void StartFlash(float duration)
    {
        FlashTimer = Math.Max(0f, duration);
    }

    public void TickFlash(float deltaTime)
    {
        FlashTimer = Math.Max(0f, FlashTimer - deltaTime);
    }
}