namespace FighterGame;

public class Character
{
    public string Name { get; }
    public int MaxHealth { get; }
    public int Health { get; set; }
    public int AttackDamage { get; }
    public double AttackSpeed { get; }

    public Character(string name, int maxHealth, int attackDamage, double attackSpeed)
    {
        Name = name;
        MaxHealth = maxHealth;
        Health = MaxHealth;
        AttackDamage = attackDamage;
        AttackSpeed = attackSpeed;
    }
}