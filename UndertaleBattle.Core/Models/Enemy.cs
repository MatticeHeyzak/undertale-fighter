namespace UndertaleBattle.Core.Models;

public class Enemy
{
    public string Name { get; set; } = "The destroyer";
    public int Health { get; private set; }
    public int MaxHealth { get; init; }
    public bool IsDead => Health <= 0;
    
    public string CheckDescription { get; init; } = "ATK 1 DEF 1. Not much is known about this enemy.";
    
    public Enemy(string name, int maxHealth)
    {
        Name      = name;
        MaxHealth = maxHealth;
        Health    = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0)
            return;
        
        Health = Math.Max(0, Health - amount);
    }
}