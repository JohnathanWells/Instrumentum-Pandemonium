using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ParticleSystem))]
public class ShotgunProjectileScript : MonoBehaviour, IDamager
{
    [field:SerializeField]public IEntity dealer { get; set; }
    [field:SerializeField]public int minDamage { get; set; }
    [field:SerializeField]public int maxDamage { get; set; }

    public ParticleSystem particleSystem;

    private void OnEnable()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject collider)
    {
        if (collider.gameObject.TryGetComponent<HitboxScript>(out var hb))
        {
            Debug.Log(dealer.displayName);
            hb.SpawnHitParticle(transform.position, transform.rotation);
            hb.Damage(dealer, Random.Range(minDamage, maxDamage));

            List<ParticleCollisionEvent> events;
            events = new List<ParticleCollisionEvent>();


            //A quick an dirty way to destroy particles on contact with an enemy. If something breaks, get rid of it.
            //ParticleSystem.Particle[] m_Particles;
            //m_Particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];

            //ParticlePhysicsExtensions.GetCollisionEvents(particleSystem, gameObject, events);
            //foreach (ParticleCollisionEvent coll in events)
            //{
            //    if (coll.intersection != Vector3.zero)
            //    {
            //        int numParticlesAlive = particleSystem.GetParticles(m_Particles);

            //        // Check only the particles that are alive
            //        for (int i = 0; i < numParticlesAlive; i++)
            //        {

            //            //If the collision was close enough to the particle position, destroy it
            //            if (Vector3.Magnitude(m_Particles[i].position - coll.intersection) < 0.05f)
            //            {
            //                m_Particles[i].remainingLifetime = -1; //Kills the particle
            //                particleSystem.SetParticles(m_Particles); // Update particle system
            //                break;
            //            }
            //        }
            //    }
            //}
        }
    }

    //private void OnParticleTrigger()
    //{
    //    //Get all particles that entered a box collider
    //    List<ParticleSystem.Particle> enteredParticles = new List<ParticleSystem.Particle>();
    //    int enterCount = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enteredParticles);

    //    //Get all fires
    //    GameObject[] fires = GameObject.FindGameObjectsWithTag("Fire");

    //    foreach (ParticleSystem.Particle particle in enteredParticles)
    //    {
    //        for (int i = 0; i < fires.Length; i++)
    //        {
    //            Collider collider = fires[i].GetComponent<Collider>();
    //            if (collider.bounds.Contains(particle.position))
    //            {
    //                if (collider.gameObject.TryGetComponent<HitboxScript>(out var hb))
    //                {
    //                    hb.SpawnHitParticle(transform.position, transform.rotation);
    //                    hb.Damage(dealer, Random.Range(minDamage, maxDamage));
    //                }
    //            }
    //        }
    //    }
    //}
}
