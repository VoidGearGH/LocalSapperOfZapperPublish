using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class AudioProcessor : MonoBehaviour
{
    public static AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.volume = PlayerPrefs.GetFloat("Volume", 0.07f);

            if(PlayerPrefs.GetInt("IsPlayed") == 1)
            {
                audioSource.time = PlayerPrefs.GetFloat("PlayedMusicTime");
                PlayerPrefs.SetInt("IsPlayed", 0);
            }
        }
    }
}
