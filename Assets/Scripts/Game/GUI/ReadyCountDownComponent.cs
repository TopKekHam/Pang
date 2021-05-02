using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
public class ReadyCountDownComponent : MonoBehaviour
{

    TMP_Text label;

    public AudioClip ReadySound, SetSound, GoSound;

    void Awake()
    {
        label = GetComponent<TMP_Text>();
        label.text = "";
    }

    public void RunCountDown()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        float countDownTime = LevelComponent.SceneSingleton.CountDownTime / 3f;

        label.text = "Ready";
        AudioPlayerSingleton.Instance.PlayerOneShoot(ReadySound);
        yield return new WaitForSecondsRealtime(countDownTime);

        label.text = "Set";
        AudioPlayerSingleton.Instance.PlayerOneShoot(SetSound);
        yield return new WaitForSecondsRealtime(countDownTime);

        label.text = "Go!";
        AudioPlayerSingleton.Instance.PlayerOneShoot(GoSound);
        yield return new WaitForSecondsRealtime(countDownTime);

        label.text = "";
        yield return null;
    }
}
