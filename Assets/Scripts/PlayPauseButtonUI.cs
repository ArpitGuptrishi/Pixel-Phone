using UnityEngine;
using UnityEngine.UI;

public class PlayPauseButtonUI : MonoBehaviour
{
    public PhoneMusicPlayer musicPlayer;
    public Image iconImage;

    public Sprite playIcon;
    public Sprite pauseIcon;

    void OnEnable()
    {
        if (musicPlayer == null) return;

        musicPlayer.OnPlayStateChanged += UpdateIcon;
        UpdateIcon(musicPlayer.IsPlaying());
    }

    void OnDisable()
    {
        if (musicPlayer != null)
            musicPlayer.OnPlayStateChanged -= UpdateIcon;
    }

    void UpdateIcon(bool isPlaying)
    {
        iconImage.sprite = isPlaying ? pauseIcon : playIcon;
    }
}
