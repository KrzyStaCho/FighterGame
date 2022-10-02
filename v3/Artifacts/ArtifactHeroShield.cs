namespace FighterGame.Artifacts;

/// <summary>
/// Class Artifact 'Hero Shield'
/// 10% damage from enemy remove
/// </summary>
public class ArtifactHeroShield : Artifact
{
    public override int AttackDamage(int damage)
    {
        return damage;
    }

    public override int DamageOnHealth(int damage)
    {
        return damage - (damage / 10);
    }
}