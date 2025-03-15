using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;
    public float jumpStrength = 2;
    public event System.Action Jumped;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    public GroundCheck groundCheck;


    void Reset()
    {
        // Try to get groundCheck.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    public void UpdateJump(float jumpInput)
    {
        // Jump when the Jump button is pressed and we are on the ground.
        if ((!groundCheck || groundCheck.isGrounded))
        {
            rigidbody.AddForce(jumpInput * Vector3.up * 100 * jumpStrength);

            if (jumpInput > 0)
                Jumped?.Invoke();
        }
    }
}
