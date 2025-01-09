using System.Collections;
using UnityEngine;

public abstract class PickupScriptBase : MonoBehaviour
{
    public string playerTag;
    public Transform pickableParent;
    public bool isPickedUp = false;
    public float resetTime = 0;

    public IEnumerator StartResetTimer()
    {
        if (resetTime < 0)
            yield break;

        yield return new WaitForSeconds(resetTime);

        ResetPickup();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnPickup(other);
    }

    public abstract void ResetPickup();

    public abstract void PickUp();

    public abstract void OnPickup(Collider other);
}
