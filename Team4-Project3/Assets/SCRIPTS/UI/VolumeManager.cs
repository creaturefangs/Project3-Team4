using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text volumeText;
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private string volumeParameter = "volume";
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private float multiplier = 30.0f;

    private float currentVolume;

    private void Awake()
    {
        // Retrieve volume from PlayerPrefs or use a default value of 0.75f.
        currentVolume = PlayerPrefs.GetFloat(volumeParameter, 0.75f);
        slider.value = currentVolume;

        // Set volume in AudioMixer to match the slider value.
        mixer.SetFloat(volumeParameter, Mathf.Log10(currentVolume) * multiplier);

        // Set text to match the slider value.
        volumeText.text = slider.value.ToString("f1");

        slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }

    private void HandleSliderValueChanged(float value)
    {
        // Every time the slider value changes, update the volume in PlayerPrefs and AudioMixer.
        mixer.SetFloat(volumeParameter, Mathf.Log10(value) * multiplier);
        volumeText.text = slider.value.ToString("f1");
        PlayerPrefs.SetFloat(volumeParameter, value);
        PlayerPrefs.Save();  // Immediately save PlayerPrefs after updating.
    }
}
