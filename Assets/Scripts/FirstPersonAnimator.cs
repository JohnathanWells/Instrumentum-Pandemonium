using StarterAssets;
using UnityEngine;

public class FirstPersonAnimator : MonoBehaviour
{
    //The animator controlling the character's animations
    public Animator animator;
    //
    public FirstPersonController firstPersonController;


    private void OnEnable()
    {
        firstPersonController.OnMove += UpdateMovement;
    }

    private void OnDisable()
    {
        firstPersonController.OnMove -= UpdateMovement;
    }

    public void UpdateMovement(Vector3 val)
    {
        animator.SetFloat("X", val.x);
        animator.SetFloat("Z", val.z);

    }
}
