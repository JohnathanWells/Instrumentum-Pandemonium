using UnityEngine;

public class FirstPersonRigidbodyAnimator : MonoBehaviour
{
    //The animator controlling the character's animations
    public Animator animator;
    //
    [SerializeField] private FirstPersonMovement firstPersonController;

    [SerializeField] private FirstPersonLook firstPersonLook;

    [SerializeField] private Jump jumpScript;

    [SerializeField] private float dampTime = 0.2f;


    private void OnEnable()
    {
        firstPersonController.OnMove += UpdateMovement;
        firstPersonLook.OnTurn += Look;
        jumpScript.Jumped += Jump;
        jumpScript.groundCheck.Grounded += IsGrounded;
        jumpScript.groundCheck.NotGrounded += IsNotGrounded;
    }

    private void OnDisable()
    {
        firstPersonController.OnMove -= UpdateMovement;
        firstPersonLook.OnTurn -= Look;
        jumpScript.Jumped -= Jump;
        jumpScript.groundCheck.Grounded -= IsGrounded;
        jumpScript.groundCheck.NotGrounded -= IsNotGrounded;
    }

    public void UpdateMovement(Vector2 val, float speed)
    {
        animator.SetFloat("X", val.x, dampTime, Time.deltaTime);
        animator.SetFloat("Z", val.y, dampTime, Time.deltaTime);
        animator.SetFloat("Speed", speed, dampTime, Time.deltaTime);

    }

    public void Look(float value)
    {
        animator.SetFloat("LookY", value);
    }

    public void MeleeAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void ShootAttack()
    {
        animator.SetTrigger("Shoot");
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }

    public void IsGrounded()
    {
        animator.SetBool("IsGrounded", true);
    }

    public void IsNotGrounded()
    {
        animator.SetBool("IsGrounded", false);
    }
}
