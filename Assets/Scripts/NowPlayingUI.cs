using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NowPlayingUI : MonoBehaviour
{
    public PhoneMusicPlayer musicPlayer;
    public TextMeshProUGUI songNameText;
    public Image coverImage;

    void OnEnable()
    {
        if (musicPlayer == null) return;

        // Subscribe to future changes
        musicPlayer.OnSongChanged += UpdateUI;

        // 🔑 FORCE SYNC to current song every time screen opens
        SongData currentSong = musicPlayer.GetCurrentSong();
        if (currentSong != null)
        {
            UpdateUI(currentSong, musicPlayer.GetCurrentIndex());
        }
    }

    void OnDisable()
    {
        if (musicPlayer != null)
            musicPlayer.OnSongChanged -= UpdateUI;
    }

    void UpdateUI(SongData song, int index)
    {
        songNameText.text = song.songName;
        coverImage.sprite = song.coverArt;
    }
}
