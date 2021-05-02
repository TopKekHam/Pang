using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerComponent : MonoBehaviour
{

    public PlayerNumber PlayerNumber;
    public float MovementSpeed = 1f;
    public float HarpoonShootLockTime = 0.25f;
    public Collider2D PlayerCollider;
    public GameObject HarpoonPrefab;
    public AudioClip HarpoonShootSound;

    public UnityEvent OnPlayerKilled;

    public PlayerMoveDirection moveDirection { get; private set; } = 0;
    public bool shootingHarpoon { get; private set; } = false;
    bool shootInput = false;

    bool inputsLocked;

    void FixedUpdate()
    {
        if (inputsLocked) return;

        HarpoonLogic();
        MovePlayer();
    }

    void MovePlayer()
    {
        if (moveDirection == 0) return;

        float moveDirectionX = 0;

        if (moveDirection.HasFlag(PlayerMoveDirection.Left))
        {
            moveDirectionX -= 1;
        }

        if (moveDirection.HasFlag(PlayerMoveDirection.Right))
        {
            moveDirectionX += 1;
        }

        var movementDelta = Vector2.right * Time.deltaTime * moveDirectionX * MovementSpeed;

        // checking if there a wall to stop the player.
        var collisionHits = PhysicsUtils.hits;

        int hitCount = PlayerCollider.Cast(movementDelta.normalized, collisionHits, movementDelta.magnitude);
        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {

                if (collisionHits[i].collider.tag == PhysicsUtils.WallTag)
                {
                    //check if we hit a wall and not a floor.
                    //and if we move in the wall direction
                    // we only care about x axis right now because we cant move verticly

                    float hitNormalX = collisionHits[i].normal.x;

                    if (hitNormalX != 0 && movementDelta.normalized.x * -1 == hitNormalX)
                    {
                        float origin = transform.position.x + PlayerCollider.offset.x;
                        float size = PlayerCollider.bounds.extents.x;
                        float destination = collisionHits[i].point.x;
                        float deltaX = PhysicsUtils.DeltaMove(origin, size, destination);
                        movementDelta.x = deltaX;
                        break;
                    }
                }
            }
        }

        transform.position += new Vector3(movementDelta.x, movementDelta.y, 0);

    }

    public void Kill()
    {
        OnPlayerKilled.Invoke();
        Destroy(gameObject);
    }

    void HarpoonLogic()
    {
        if (shootInput)
        {
            shootInput = false;
            StartCoroutine(ShootHarpoon());
        }
    }

    IEnumerator ShootHarpoon()
    {
        shootingHarpoon = true;
        inputsLocked = true;
        SpawnHarpoon();
        AudioPlayerSingleton.Instance.PlayerOneShoot(HarpoonShootSound);
        yield return new WaitForSeconds(HarpoonShootLockTime);
        shootingHarpoon = false;
        inputsLocked = false;
    }

    void SpawnHarpoon()
    {
        var entityContainer = EntitiesContainerComponent.SceneSingleton.RuntimeEntitiesContainer;
        var harpoon = Instantiate(HarpoonPrefab, entityContainer.transform);
        harpoon.transform.position = transform.position;
    }

    public void MoveDirectionInputDown(PlayerMoveDirection direction)
    {
        moveDirection |= direction;
    }

    public void MoveDirectionInputUp(PlayerMoveDirection direction)
    {
        moveDirection &= ~direction;
    }

    public void ShootHarpoonInput()
    {
        shootInput = true;
    }
}
