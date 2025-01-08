using UnityEngine;

public class SpinningScript : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.Rotate(transform.up, speed * Time.deltaTime);
    }
}
