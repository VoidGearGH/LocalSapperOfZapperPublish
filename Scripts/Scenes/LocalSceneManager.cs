using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class LocalSceneManager : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    public async void LoadMainMenuScene(string sceneName)
    {
        AudioListener oldListener = FindFirstObjectByType<AudioListener>();
        if (oldListener != null)
            oldListener.enabled = false;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, 0);
        asyncLoad.allowSceneActivation = false;

        PlayerPrefs.SetFloat("PlayedMusicTime", _source.time);
        PlayerPrefs.SetInt("IsPlayed", 1);

        while (asyncLoad.progress < 0.9f)
        {
            await Task.Yield();
        }

        asyncLoad.allowSceneActivation = true;
        Time.timeScale = 1.0f;
    }
}
