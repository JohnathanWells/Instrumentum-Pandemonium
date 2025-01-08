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


    void Awake()
    {
        // Get the rigidbody on this.
        //rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
        float targetVerticalVelocity = Input.GetAxis("Jump") * targetMovingSpeed + Input.GetAxis("Crouch") * -targetMovingSpeed;


        // Apply movement.
        rigidbody.linearVelocity = playerCamera.transform.forward * targetVelocity.y + playerCamera.transform.right * targetVelocity.x + playerCamera.transform.up * targetVerticalVelocity;
    }
}