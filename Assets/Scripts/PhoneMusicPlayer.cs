using UnityEngine;

public class PhoneMusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public SongData[] songs;

    private int currentIndex = -1;
    private bool isPaused = false;

    public delegate void SongChanged(SongData song, int index);
    public event SongChanged OnSongChanged;

    public delegate void PlayStateChanged(bool isPlaying);
    public event PlayStateChanged OnPlayStateChanged;


    public void PlaySongByIndex(int index)
    {
        if (songs == null || songs.Length == 0)
            return;

        if (index < 0 || index >= songs.Length)
            return;

        currentIndex = index;
        audioSource.clip = songs[currentIndex].audioClip;
        audioSource.Play();
        OnPlayStateChanged?.Invoke(true);

        isPaused = false;

        OnSongChanged?.Invoke(songs[currentIndex], currentIndex);
    }

    public void TogglePlayPause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            isPaused = true;
            OnPlayStateChanged?.Invoke(false);
        }
        else
        {
            if (isPaused)
            {
                audioSource.UnPause();
            }
            else
            {
                audioSource.Play();
            }

            isPaused = false;
            OnPlayStateChanged?.Invoke(true);
        }
    }


    public void NextSong()
    {
        if (songs == null || songs.Length == 0)
            return;

        int next = currentIndex + 1;
        if (next >= songs.Length)
            next = 0;

        PlaySongByIndex(next);
        OnPlayStateChanged?.Invoke(true);

    }

    public void PreviousSong()
    {
        if (songs == null || songs.Length == 0)
            return;

        int prev = currentIndex - 1;
        if (prev < 0)
            prev = songs.Length - 1;

        PlaySongByIndex(prev);
        OnPlayStateChanged?.Invoke(true);

    }

    
    public SongData GetCurrentSong()
    {
        if (currentIndex < 0 || currentIndex >= songs.Length)
            return null;

        return songs[currentIndex];
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }
    
    public bool IsSongPlaying(int index)
    {
        return index == currentIndex && audioSource.isPlaying;
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
    }

    public float GetVolume()
    {
        return audioSource.volume;
    }

    public void Pause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            isPaused = true;
            OnPlayStateChanged?.Invoke(false);
        }
    }

    public void Resume()
    {
        if (isPaused)
        {
            audioSource.UnPause();
            isPaused = false;
            OnPlayStateChanged?.Invoke(true);
        }
    }




}
