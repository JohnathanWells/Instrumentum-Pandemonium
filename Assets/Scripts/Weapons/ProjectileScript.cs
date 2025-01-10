using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour, IDamager
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private ParticleSystem clinkParticle;

    [field:SerializeField] public int minDamage { get; set; }
    [field: SerializeField] public int maxDamage { get; set; }
    public IEntity dealer 
    {
        get;
        set;
    }

    [SerializeField] private string killVerb = "sprayed";

    private void Awake()
    {
        StartCoroutine(SelfDestructTimer());

        rigidbody.linearVelocity = transform.forward * speed;
    }


    IEnumerator SelfDestructTimer()
    {
        yield return new WaitForSeconds(lifetime);

        Destroy(this.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<HitboxScript>(out var hb))
        {
            hb.SpawnHitParticle(transform.position, transform.rotation);
            hb.Damage(dealer, Random.Range(minDamage, maxDamage));
        }
        else
        {
            Instantiate(clinkParticle, transform.position, transform.rotation);
        }

        DestroyBullet();
    }

    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }

    public void OnEnter(HealthScript target)
    {
        Debug.Log("Hit " + target.gameObject.name);
    }
}
