using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// ”правл€ет настройками звуков
/// </summary>
public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider slider;
    public string VolumeParameter = "MasterVolume";

    private float _volumeValue;
    private const float _multiplier = 20f;

    public string ReverbParameter = "MasterReverb";
    private bool _reverbEnabled;

    private void Awake()
    {
        slider.onValueChanged.AddListener(HandlerSliderValueChanged);
    }

    private void HandlerSliderValueChanged(float value)
    {
        _volumeValue = Mathf.Log10(value) * _multiplier;
        audioMixer.SetFloat(VolumeParameter, _volumeValue);
    }

    void Start()
    {
        _volumeValue = PlayerPrefs.GetFloat(VolumeParameter, Mathf.Log10(slider.value) * _multiplier);
        slider.value = Mathf.Pow(10f, _volumeValue / _multiplier);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(VolumeParameter, _volumeValue);
    }

    public void ToggleReverb()
    {
        _reverbEnabled = !_reverbEnabled;
        ApplyReverb(_reverbEnabled);
    }

    private void ApplyReverb(bool enabled)
    {
        audioMixer.SetFloat(ReverbParameter, enabled ? 0f : -10000f);
    }
}