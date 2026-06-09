using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private Toggle _toggleVolume;
    [SerializeField] private Slider _sliderVolume;
    [SerializeField] private AudioSource _audioSource;

    private float _volume;
    private void Awake()
    {
        Load();

        ValueAudio();
    }
    public void Slide()
    {
        _volume = _sliderVolume.value;
        
        Save();
        ValueAudio();
    }

    public void Toggle()
    {
        if(_toggleVolume != null)
        {
            _volume = _toggleVolume.isOn ? 1f : 0f;

            ValueAudio();
            Save();
        }
    }
    private void ValueAudio()
    {
        if( _audioSource != null )
        {
            _audioSource.volume = _volume;
            _sliderVolume.value = _volume;
        
            _toggleVolume.isOn = _volume == 0 ? false : true;
        }
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("Volume", _volume);
    }
    private void Load()
    {
        _volume = PlayerPrefs.GetFloat("Volume", 0.07f);
    }
}
