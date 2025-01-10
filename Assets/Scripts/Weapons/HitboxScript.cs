using UnityEngine;

public class HitboxScript : MonoBehaviour
{
    public HealthScript health;
    public ParticleSystem hitParticle;

    public void SpawnHitParticle(Vector3 position, Quaternion angle)
    {
        if (hitParticle == null)
            return;

        Instantiate(hitParticle, position, angle);
    }

    public void Damage(IEntity dealer, int dmg, WeaponScriptBase weapon = null)
    {
        health.Damage(dmg, weapon, dealer);
    }

    public void Heal(IEntity healer, int amt)
    {
        health.Heal(amt, healer);
    }
}
