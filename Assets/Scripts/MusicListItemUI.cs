using UnityEngine;

public class MusicListItemUI : MonoBehaviour
{
    public int songIndex;
    public GameObject playingArrow;

    public void SetPlaying(bool isPlaying)
    {
        if (playingArrow != null)
            playingArrow.SetActive(isPlaying);
    }
}
