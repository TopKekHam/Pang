using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayerSingleton : MonoBehaviour
{

    public AudioMixerGroup Master;

    AudioSource audioSourceMusic;
    AudioSource audioSourceSFX;

    public static AudioPlayerSingleton Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            gameObject.AddComponent<AudioListener>();

            audioSourceMusic = gameObject.AddComponent<AudioSource>();
            audioSourceMusic.outputAudioMixerGroup = Master;
            audioSourceMusic.priority = 1;

            audioSourceSFX = gameObject.AddComponent<AudioSource>();
            audioSourceSFX.outputAudioMixerGroup = Master;
            audioSourceSFX.priority = 1;
        }

    }

    public void PlayerOneShoot(AudioClip clip)
    {
        audioSourceSFX.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        audioSourceMusic.Stop();
        audioSourceMusic.loop = true;
        audioSourceMusic.clip = clip;
        audioSourceMusic.Play();
    }

    public void ToggleMute()
    {
        audioSourceMusic.mute = !audioSourceMusic.mute;
        audioSourceSFX.mute = !audioSourceSFX.mute;
    }

}
