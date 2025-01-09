using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float lifetime = 1f;

    private void Awake()
    {
        StartCoroutine(SelfDestructTimer());
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    IEnumerator SelfDestructTimer()
    {
        yield return new WaitForSeconds(lifetime);

        Destroy(this.gameObject);
    }

    public void OnEnter(HealthScript target)
    {
        Debug.Log("Hit " + target.gameObject.name);
    }
}
