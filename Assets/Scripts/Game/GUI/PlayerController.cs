using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public PlayerNumber PlayerNumber;
    PlayerComponent player => EntitiesContainerComponent.SceneSingleton.Players[(int)PlayerNumber - 1];

    bool blockInputs = true;

    void Start()
    {
        LevelComponent.SceneSingleton.OnStateChanged += (state) =>
        {

            if (state == LevelState.Paused)
            {
                blockInputs = true;
                ButtonLeftUp();
                ButtonRightUp();
            }

            if (state == LevelState.Playing)
            {
                blockInputs = false;
            };
        };
    }

#if UNITY_EDITOR
    void Update()
    {
        // the code below made for testing in editor without phone.
     
        if (blockInputs) return;
        if (PlayerNumber != PlayerNumber.One) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ButtonLeftDown();
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            ButtonLeftUp();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ButtonRightDown();
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            ButtonRightUp();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ButtonShoot();
        }

    }
#endif

    public void ButtonLeftDown()
    {
        player.MoveDirectionInputDown(PlayerMoveDirection.Left);
    }

    public void ButtonLeftUp()
    {
        player.MoveDirectionInputUp(PlayerMoveDirection.Left);
    }

    public void ButtonRightDown()
    {
        player.MoveDirectionInputDown(PlayerMoveDirection.Right);
    }

    public void ButtonRightUp()
    {
        player.MoveDirectionInputUp(PlayerMoveDirection.Right);
    }

    public void ButtonShoot()
    {
        player.ShootHarpoonInput();
    }
}
