using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderUI : MonoBehaviour
{
    public PhoneMusicPlayer musicPlayer;
    public Slider volumeSlider;

    void Start()
    {
        if (musicPlayer == null) return;

        // Sync slider with current volume
        volumeSlider.value = musicPlayer.GetVolume();

        // Listen for slider changes
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnDestroy()
    {
        volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float value)
    {
        musicPlayer.SetVolume(value);
    }
}
