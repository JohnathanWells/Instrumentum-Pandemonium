using UnityEngine;

public class FirstPersonRigidbodyAnimator : MonoBehaviour
{
    //The animator controlling the character's animations
    public Animator animator;
    //
    [SerializeField] private FirstPersonMovement firstPersonController;

    [SerializeField] private float dampTime = 0.2f;


    private void OnEnable()
    {
        firstPersonController.OnMove += UpdateMovement;
    }

    private void OnDisable()
    {
        firstPersonController.OnMove -= UpdateMovement;
    }

    public void UpdateMovement(Vector2 val, float speed)
    {
        animator.SetFloat("X", val.x, dampTime, Time.deltaTime);
        animator.SetFloat("Z", val.y, dampTime, Time.deltaTime);
        animator.SetFloat("Speed", speed, dampTime, Time.deltaTime);

    }
}
