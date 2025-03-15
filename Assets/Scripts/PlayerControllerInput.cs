using UnityEngine;
using UnityEngine.Events;

public class PlayerControllerInput : MonoBehaviour
{
    public bool inputEnabled = true;

    public SpectatorFirstPersonControls spectatorScript;
    public FirstPersonMovement movementScript;
    public Jump jumpScript;
    public Crouch crouchScript;

    public UnityAction<Vector2, bool> OnHorizontalInput;
    public UnityAction<float> OnVerticalInput;

    public string runInputName = "Run";
    public string jumpInputName = "Jump";
    public string crouchInputName = "Crouch";
    Vector2 input = Vector2.zero;
    float jumpInput = 0f;
    float crouchInput = 0f;


    private void OnEnable()
    {
        //OnHorizontalInput += spectatorScript.MoveHorizontal;
        OnHorizontalInput += movementScript.MoveHorizontal;
        //OnVerticalInput += spectatorScript.MoveVertical;
        OnVerticalInput += jumpScript.UpdateJump;
    }

    private void OnDisable()
    {
        //OnHorizontalInput -= spectatorScript.MoveHorizontal;
        OnHorizontalInput -= movementScript.MoveHorizontal;
        //OnVerticalInput -= spectatorScript.MoveVertical;
        OnVerticalInput -= jumpScript.UpdateJump;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!inputEnabled)
            return;

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        jumpInput = Input.GetAxis(jumpInputName);
        crouchInput = Input.GetAxis(crouchInputName);
        float verticalInput = Mathf.Clamp( jumpInput - crouchInput, -1, 1);

        OnVerticalInput?.Invoke(verticalInput);
        OnHorizontalInput?.Invoke(input, Input.GetButton(runInputName));
    }
}
