using UnityEngine;

[RequireComponent(typeof(HealthScript), typeof(HitboxScript))]
public class BreakableScript : MonoBehaviour
{
    [field:SerializeField] private GameObject[] objects;
    [field: SerializeField] private ParticleSystem breakingEffect;

    private HealthScript health;
    private HitboxScript hitbox;

    private void OnEnable()
    {
        health = GetComponent<HealthScript>();
        hitbox = GetComponent<HitboxScript>();

        hitbox.health = this.health;

        health.OnDie += (x,y,z) => Break();
    }

    private void OnDisable()
    {
        health.OnDie -= (x,y,z) => Break();
    }

    void Break()
    {
        foreach (var o in objects)
        {
            Instantiate(breakingEffect, o.transform.position, transform.rotation);
            o.SetActive(false);
        }

        Destroy(this.gameObject);
    }
}
