using UnityEngine;

public class BodyDirectionAnimator : MonoBehaviour
{
    public Animator animator;
    public Transform aim;

    void Update()
    {
        float angle = aim.eulerAngles.z;

        Vector2 dir = Vector2.zero;

        if (angle >= 315 || angle < 45)
            dir = new Vector2(1, 0);
        else if (angle >= 45 && angle < 135)
            dir = new Vector2(0, 1);
        else if (angle >= 135 && angle < 225)
            dir = new Vector2(-1, 0);
        else
            dir = new Vector2(0, -1);

        animator.SetFloat("DirX", dir.x);
        animator.SetFloat("DirY", dir.y);

        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bool isMoving = move.sqrMagnitude > 0;

        animator.SetBool("IsMoving", isMoving);
    }
}