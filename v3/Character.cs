using FighterGame.Artifacts;

namespace FighterGame;

public class Character
{
    public static readonly int CharacterMaxUpgrade = 10;
    public static readonly int DmgOnCritRate = 120;

    public static readonly int BaseHealth = 100;
    public static readonly int BaseArmor = 0;
    public static readonly int BaseAttackDamage = 5;
    public static readonly int BaseCritRate = 0;
    public static readonly double BaseAttackSpeed = 0.8;

    public static readonly int UpgradeHealthValue = 30;
    public static readonly int UpgradeArmorValue = 3;
    public static readonly int UpgradeAttackDamageValue = 8;
    public static readonly int UpgradeCritRateValue = 20;
    public static readonly double UpgradeAttackSpeedValue = 0.2;

    public string Name { get; }
    public int MaxHealth { get; }
    public int Health { get; private set; }
    public int Armor { get; }
    public int AttackDamage { get; }
    public int CritRate { get; }
    public double AttackSpeed { get; }
    public Artifact CharacterArtifact { get; }
    
    public Character(string name, int upgradeMaxHealth, int upgradeArmor, int upgradeAttackDamage, int upgradeCritRate, double upgradeAttackSpeed, Artifact characterArtifact)
    {
        Name = name;
        MaxHealth = BaseHealth + (UpgradeHealthValue * upgradeMaxHealth);
        Health = MaxHealth;
        Armor = BaseArmor + (UpgradeArmorValue * upgradeArmor);
        AttackDamage = BaseAttackDamage + (UpgradeAttackDamageValue * upgradeAttackDamage);
        CritRate = BaseCritRate + (UpgradeCritRateValue * upgradeCritRate);
        AttackSpeed = BaseAttackSpeed + (UpgradeAttackSpeedValue * upgradeAttackSpeed);
        CharacterArtifact = characterArtifact;
    }

    /// <summary>
    /// Get final damage of character, count crit rate and artifact effect
    /// </summary>
    /// <returns>Final damage</returns>
    public int GetCharacterAttackDamage()
    {
        Random rnd = new Random();
        int finalDamage = ((rnd.Next(100) + 1) <= CritRate) ? (int)((DmgOnCritRate / 100.0) * AttackDamage) : AttackDamage;
        return CharacterArtifact.AttackDamage(finalDamage);
    }

    /// <summary>
    /// Minimise damage with armor and artifact effect and remove damage from health
    /// </summary>
    /// <returns>Damage after hit armor</returns>
    public int DealDamageCharacter(int damage)
    {
        int finalDamage = (damage < Armor) ? 0 : (damage - Armor);
        finalDamage = CharacterArtifact.DamageOnHealth(finalDamage);

        Health -= (finalDamage < Health) ? finalDamage : Health;
        return finalDamage;
    }
}