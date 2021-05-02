using UnityEngine;

public static class PhysicsUtils
{

    public const string WallTag = "Wall";
    public const string BallTag = "Ball";
    public const string HarpoonTag = "Harpoon";
    public const string PlayerTag = "Player";

    // preallocated hit array for physics
    public static RaycastHit2D[] hits = new RaycastHit2D[16]; 

    public static float DeltaMove(float origin, float size, float destination)
    {
        float normalDirection = (destination - origin) / Mathf.Abs(destination - origin);
        return destination - (origin + (size * normalDirection));
    }

}
