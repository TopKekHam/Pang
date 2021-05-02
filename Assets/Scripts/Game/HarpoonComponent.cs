using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HarpoonComponent : MonoBehaviour
{

    public float HarpoonSpeed = 3;
    public Vector2 HarpoonHeadSize = new Vector2(0.25f, 0.5f);
    public AudioClip HitCeilingSound;

    [HideInInspector]
    public float HarpoonLength;
    BoxCollider2D boxCollider;

    void Start()
    {
        // we always start with a head of the harpoon spawned
        HarpoonLength = HarpoonHeadSize.y;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        HarpoonLength += Time.fixedDeltaTime * HarpoonSpeed;

        boxCollider.offset = new Vector2(0, HarpoonLength / 2);
        boxCollider.size = new Vector2(HarpoonHeadSize.x, HarpoonLength);

        var collisionHits = PhysicsUtils.hits;
        int hitCount = boxCollider.Cast(Vector2.up, collisionHits, 0);

        for (int i = 0; i < hitCount; i++)
        {
            if (collisionHits[i].collider.tag == PhysicsUtils.WallTag)
            {
                if (collisionHits[i].normal.y == -1)
                {
                    var wallObj = collisionHits[i].collider.gameObject;

                    if (wallObj.TryGetComponent<DistracableWall>(out var distractableWall))
                    {
                        distractableWall.Destroy();
                    }

                    DestoryHarpoon();
                    AudioPlayerSingleton.Instance.PlayerOneShoot(HitCeilingSound);
                    break;
                }
            }
        }
    }

    public void DestoryHarpoon()
    {
        Destroy(gameObject);
    }
}
