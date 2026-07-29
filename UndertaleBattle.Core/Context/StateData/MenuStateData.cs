namespace UndertaleBattle.Core.Context.StateData;

public sealed class MenuStateData
{
    public int SelectedIndex { get; set; }
    public void Reset() => SelectedIndex = 0;

    public void MoveLeft(int optionCount)
    {
        ValidateOptionCount(optionCount);
        SelectedIndex = (SelectedIndex - 1 + optionCount) % optionCount;
    }

    public void MoveRight(int optionCount)
    {
        ValidateOptionCount(optionCount);
        SelectedIndex = (SelectedIndex + 1) % optionCount;
    }

    private static void ValidateOptionCount(int optionCount)
    {
        if (optionCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(optionCount), optionCount, "A menu must contain at least one option.");
    }
}