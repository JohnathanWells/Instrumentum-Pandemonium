using System.Collections.Generic;
using UnityEngine;

public class SpectatorFirstPersonControls : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Camera playerCamera;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();


    public UnityEngine.Events.UnityAction<Vector2, float> OnMove;

    private Vector3 targetVelocity;
    private Vector3 targetDirections;
    float targetMovingSpeed;


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
        targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        targetDirections = (playerCamera.transform.forward * input.y + playerCamera.transform.right * input.x).normalized;

         
        // Apply movement.
        rigidbody.linearVelocity = targetDirections * targetMovingSpeed;
    }

    public void MoveVertical(float verticalInput)
    {
        // Get targetVelocity from input.
        targetDirections.y = Mathf.Clamp(targetDirections.y + verticalInput, -1, 1);
        targetDirections = targetDirections.normalized;


        // Apply movement.
        rigidbody.linearVelocity = targetDirections * targetMovingSpeed;
    }
}