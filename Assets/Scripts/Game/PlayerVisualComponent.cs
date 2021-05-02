using UnityEngine;

[RequireComponent(typeof(PlayerComponent), typeof(Animator))]
public class PlayerVisualComponent : MonoBehaviour
{

    PlayerComponent player;
    Animator animator;

    void Start()
    {
        player = GetComponent<PlayerComponent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("ShootingHarpoon", player.shootingHarpoon);
        if (player.shootingHarpoon)
        {
            animator.SetBool("Walking", false);
            return;
        }


        if (player.moveDirection == PlayerMoveDirection.Left)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("Walking", true);
        }
        else if (player.moveDirection == PlayerMoveDirection.Right)
        {
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }
}
