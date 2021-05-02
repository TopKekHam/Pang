using UnityEngine;

public class Startup
{

    [RuntimeInitializeOnLoadMethod]
    static void OnGameLoad()
    {
        Application.targetFrameRate = 60;
    }

}
