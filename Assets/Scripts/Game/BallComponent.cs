using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class BallComponent : MonoBehaviour
{
    // @NOTE: right now this two values are hard-coded we should make them editable from the editor.
    public const int MIN_BALL_SIZE = 1;
    public const int MAX_BALL_SIZE = 4;

    public Direction StartMoveDirection = Direction.Right;
    public Vector2 MovementSpeed = new Vector2(1, 1);
    public ParticleSystemSizeComponent PopParticle;
    [Range(MIN_BALL_SIZE, MAX_BALL_SIZE)]
    public int Size = 1;
    public GameObject sprite;
    public AudioClip PopSound;

    float horizontalMovementDirection;
    float verticalVelocity;
    CircleCollider2D circleCollider;

    void OnValidate()
    {
        ResizeBall(Size);
    }

    void Start()
    {
        horizontalMovementDirection = (float)StartMoveDirection;
        circleCollider = GetComponent<CircleCollider2D>();
        var rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(horizontalMovementDirection * MovementSpeed.x, rigidbody.velocity.y);
    }

    void FixedUpdate()
    {

        verticalVelocity += Physics2D.gravity.y * Time.fixedDeltaTime;

        float movementDeltaX = horizontalMovementDirection * MovementSpeed.x * Time.fixedDeltaTime;
        float movementDeltaY = verticalVelocity * Time.fixedDeltaTime;

        DoCollision(new Vector2(movementDeltaX, 0));
        DoCollision(new Vector2(0, movementDeltaY));

    }

    void DoCollision(Vector2 moveVector)
    {
        Vector2 movementDirection = moveVector / new Vector2(Mathf.Abs(moveVector.x), Mathf.Abs(moveVector.y));

        int hitCount = 0;
        // if we hit a corner we should resolve the collitions couple times for now 4 seems good
        int timesLeftToResolve = 4;

        // resolving collitions
        do
        {
            var collisionHits = PhysicsUtils.hits;
            hitCount = circleCollider.Cast(moveVector.normalized, collisionHits, moveVector.magnitude);

            if (hitCount == 0) break;

            // looping over hits and breaking on first hit with a wall/floor/ceiling.
            for (int i = 0; i < hitCount; i++)
            {
                var hit = collisionHits[i];

                if (collisionHits[i].collider.tag == PhysicsUtils.HarpoonTag)
                {
                    var harpoon = collisionHits[i].collider.gameObject.GetComponent<HarpoonComponent>();
                    LevelComponent.SceneSingleton.AddScore(harpoon);
                    harpoon.DestoryHarpoon();
                    Pop();
                    return;
                }

                if (collisionHits[i].collider.tag == PhysicsUtils.PlayerTag)
                {
                    Pop();
                    collisionHits[i].collider.gameObject.GetComponent<PlayerComponent>().Kill();
                    return;
                }

                if (hit.collider.tag == PhysicsUtils.WallTag)
                {
                    CollideWithWalls(hit, ref movementDirection, ref moveVector);
                }
            }

            timesLeftToResolve--;

        } while (hitCount > 0 && timesLeftToResolve > 0);

        transform.position += new Vector3(moveVector.x, moveVector.y, 0);
    }

    /// <param name="hit"></param>
    /// <param name="movementDirection"></param>
    /// <param name="movementDelta"></param>
    /// <returns>return true if hit a wall/floor/ceiling else false</returns>
    bool CollideWithWalls(RaycastHit2D hit, ref Vector2 movementDirection, ref Vector2 movementDelta)
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 hitNormal = (position - hit.point).normalized;
        Vector2 hitDirection = hitNormal / new Vector2(Mathf.Abs(hitNormal.x), Mathf.Abs(hitNormal.y));

        //check if we hit a wall.
        //and if we move in the wall direction.
        if (movementDirection.x * -1 == hitDirection.x)
        {
            float destination = hit.point.x;
            var deltaX = PhysicsUtils.DeltaMove(transform.position.x, circleCollider.radius, destination);
            horizontalMovementDirection = hitDirection.x;

            movementDelta.x = deltaX - movementDelta.x;
            return true;
        }

        //check if we hit a floor.
        if (movementDirection.y == -1 && hitDirection.y == 1f)
        {
            float destination = hit.point.y;
            var deltaY = PhysicsUtils.DeltaMove(transform.position.y, circleCollider.radius, destination);

            // we calculate this value to make small balls jump lower and big balls jump higher.
            // if we hit a floor that about the ground level we need to jump less
            float distanceFromFloor = hit.point.y - LevelComponent.SceneSingleton.FloorY;
            float floorHeightMultiplier = 1f - Mathf.Clamp(distanceFromFloor * (Size / MAX_BALL_SIZE), 0, 0.9f);
            verticalVelocity = Size + 4f * floorHeightMultiplier;

            movementDelta.y = deltaY;
            return true;
        }

        //check if we hit a ceiling.
        if (movementDirection.y == 1 && hitDirection.y == -1f)
        {
            float destination = hit.point.y;
            var deltaY = PhysicsUtils.DeltaMove(transform.position.y, circleCollider.radius, destination);

            // we calculate this value to make small balls jump lower and big balls jump higher.
            verticalVelocity = hitDirection.y;

            movementDelta.y = deltaY - movementDelta.y;
            return true;
        }

        return false;
    }

    void ResizeBall(int size)
    {
        float factor = Mathf.Pow(2, size) / 2;

        Size = size;
        sprite.transform.localScale = Vector3.one / 4 * factor;
        GetComponent<CircleCollider2D>().radius = 0.125f * factor;
    }

    public void Pop()
    {
        if (Size > 1)
        {
            var nextBallSize = Size - 1;

            SpawnBall(nextBallSize, Direction.Left);
            SpawnBall(nextBallSize, Direction.Right);
        }

        SpawnPopPartile(transform.position);
        EntitiesContainerComponent.SceneSingleton.RemoveBall(this);
        AudioPlayerSingleton.Instance.PlayerOneShoot(PopSound);

        Destroy(gameObject);
    }

    void SpawnPopPartile(Vector3 position)
    {
        var entityContainer = EntitiesContainerComponent.SceneSingleton.RuntimeEntitiesContainer;
        var particle = Instantiate(PopParticle.gameObject, entityContainer.transform);
        particle.transform.position = position;
        particle.GetComponent<ParticleSystemSizeComponent>().SetSize(Size);
    }

    /// <summary>
    /// spawns ball on the current ball position facing certain direction
    /// </summary>
    /// <param name="size"> size of the ball</param>
    /// <param name="direction"> </param>
    void SpawnBall(int size, Direction direction)
    {
        var entityContainer = EntitiesContainerComponent.SceneSingleton.RuntimeEntitiesContainer;

        var ballObject = Instantiate(gameObject, entityContainer.transform);
        var halfSize = circleCollider.radius / 2;
        ballObject.transform.position = transform.position + new Vector3(halfSize * (int)direction, 0, 0);

        var ball = ballObject.GetComponent<BallComponent>();
        ball.ResizeBall(size);
        ball.StartMoveDirection = direction;
        ball.verticalVelocity = 4;

        EntitiesContainerComponent.SceneSingleton.AddBall(ball);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
    }



}
