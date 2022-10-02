namespace FighterGame.Artifacts;

public abstract class Artifact
{
    public static readonly int ChanceOfGettingArtifact = 30;
    
    public abstract int AttackDamage(int damage);
    public abstract int DamageOnHealth(int damage);
}