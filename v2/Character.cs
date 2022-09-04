namespace FighterGame;

public class Character
{
    public static readonly int MaxUpgradeBaseAttributes = 5;
    public static readonly int DmgOnCritRate = 120;

    public static readonly int BaseHealth = 100;
    public static readonly int BaseAttackDamage = 5;
    public static readonly int BaseCritRate = 0;
    public static readonly double BaseAttackSpeed = 0.8;

    public static readonly int UpgradeHealthValue = 20;
    public static readonly int UpgradeAttackDamageValue = 7;
    public static readonly int UpgradeCritRateValue = 20;
    public static readonly double UpgradeAttackSpeedValue = 0.2;
    
    public string Name { get; }
    public int MaxHealth { get; }
    public int Health { get; private set; }
    public int AttackDamage { get; }
    public int CritRate { get; }
    public double AttackSpeed { get; }

    public Character(string name, int upgradeMaxHealth, int upgradeAttackDamage, int upgradeCritRate, double upgradeAttackSpeed)
    {
        Name = name;
        MaxHealth = BaseHealth + (UpgradeHealthValue * upgradeMaxHealth);
        Health = MaxHealth;
        AttackDamage = BaseAttackDamage + (UpgradeAttackDamageValue * upgradeAttackDamage);
        CritRate = BaseCritRate + (UpgradeCritRateValue * upgradeCritRate);
        AttackSpeed = BaseAttackSpeed + (UpgradeAttackSpeedValue * upgradeAttackSpeed);
    }

    /// <summary>
    /// Get character dmg and remove health from enemy character
    /// </summary>
    /// <param name="attackPlayer">This object attack</param>
    /// <param name="defCharacter">This object is attacking</param>
    /// <param name="moveText">Text when character deal dmg enemy character</param>
    public static void DoMove(Character attackPlayer, Character defCharacter, string moveText)
    {
        Random rnd = new Random();
        int characterDmg = attackPlayer.AttackDamage;

        if (attackPlayer.CritRate != 0)
        {
            characterDmg += (int)((rnd.Next(1, 100) <= attackPlayer.CritRate) ? (attackPlayer.AttackDamage * 0.5) : 0);
        }

        defCharacter.Health -= characterDmg;

        Console.WriteLine(moveText, attackPlayer.Name, characterDmg, defCharacter.Name);
    }
}