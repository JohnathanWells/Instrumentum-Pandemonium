using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;
    public Transform playerTransform;

    [SerializeField] Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();


    public UnityEngine.Events.UnityAction<Vector2, float> OnMove;


    void Awake()
    {
        // Get the rigidbody on this.
        //rigidbody = GetComponent<Rigidbody>();
    }

    public void MoveHorizontal(Vector2 input, bool isPressingRun = false)
    {
        // Update IsRunning from input.
        IsRunning = canRun && isPressingRun;

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector3 targetVelocity = new Vector3(input.x * targetMovingSpeed, rigidbody.linearVelocity.y, input.y * targetMovingSpeed);

        //Send the event updating this character's movement
        OnMove?.Invoke(new Vector3(input.x, input.y).normalized, targetVelocity.magnitude);

        // Apply movement.
        rigidbody.linearVelocity = rigidbody.transform.TransformDirection(targetVelocity);
    }
}