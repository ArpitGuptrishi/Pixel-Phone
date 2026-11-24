using UnityEngine;

public class MusicListUI : MonoBehaviour
{
    public PhoneMusicPlayer musicPlayer;
    public MusicListItemUI[] items;

    void OnEnable()
    {
        musicPlayer.OnSongChanged += UpdateHighlight;
        RefreshInitial();
    }

    void OnDisable()
    {
        musicPlayer.OnSongChanged -= UpdateHighlight;
    }

    void RefreshInitial()
    {
        int current = musicPlayer.GetCurrentIndex();
        UpdateHighlight(null, current);
    }

    void UpdateHighlight(SongData song, int index)
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].SetPlaying(i == index);
        }
    }
}
