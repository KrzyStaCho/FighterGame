namespace FighterGame.Artifacts;

/// <summary>
/// Class Artifact 'Dark Blade'
/// Each attack from player, deal additional 1% damage.
/// </summary>
public class ArtifactDarkBlade : Artifact
{
    private int countOfAttack = 0;
    
    public override int AttackDamage(int damage)
    {
        countOfAttack++;
        return damage + (damage * countOfAttack) / 100;
    }

    public override int DamageOnHealth(int damage)
    {
        return damage;
    }
}