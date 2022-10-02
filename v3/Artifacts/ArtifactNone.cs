namespace FighterGame.Artifacts;

public class ArtifactNone : Artifact
{
    public override int AttackDamage(int damage)
    {
        return damage;
    }

    public override int DamageOnHealth(int damage)
    {
        return damage;
    }
}