namespace UndertaleBattle.Core.Models;

public sealed class Enemy
{
    public string Name { get; }

    public int Health { get; private set; }

    public int MaxHealth { get; }

    public bool IsDead => Health <= 0;

    public string CheckDescription { get; init; } =
        "ATK 1 DEF 1. Not much is known about this enemy.";

    public Enemy(string name, int maxHealth)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (maxHealth <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxHealth));

        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || IsDead)
            return;

        Health = Math.Max(0, Health - amount);
    }
}