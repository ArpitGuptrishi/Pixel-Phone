using UnityEngine;

public class PhoneScreenManager : MonoBehaviour
{
    [Header("Screens")]
    public GameObject homeScreen;
    public GameObject musicListScreen;
    public GameObject nowPlayingScreen;
    public GameObject callerApp;
    public GameObject messagesAppScreen;

    [Header("Music")]
    public PhoneMusicPlayer musicPlayer;

    [Header("Messages")]
    public GameObject messageBadgeIcon;
    public AudioSource notificationSource;
    public AudioClip notificationClip;

    public GameObject inboxScreen;
    public GameObject noMessagesScreen;

    public MessageData[] messages;

#if UNITY_EDITOR
    [Header("DEBUG TESTING")]
    public bool testMessage1;
    public bool testMessage2;
#endif

    bool hasUnreadMessage = false;


    void Start()
    {
        

        ShowHome();

        if (messageBadgeIcon != null)
            messageBadgeIcon.SetActive(false);

        
        foreach (var msg in messages)
        {
            if (msg.inboxButton != null)
                msg.inboxButton.SetActive(false);

            if (msg.messageScreen != null)
                msg.messageScreen.SetActive(false);
        }

        if (inboxScreen != null)
            inboxScreen.SetActive(false);

        if (noMessagesScreen != null)
            noMessagesScreen.SetActive(true);


    }


    void Update()
    {
#if UNITY_EDITOR
        if (testMessage1)
        {
            testMessage1 = false;
            NotifyNoteCollected(1);
        }

        if (testMessage2)
        {
            testMessage2 = false;
            NotifyNoteCollected(2);
        }
#endif
    }

   

    public void ShowHome()
    {
        CloseAllApps();
        homeScreen.SetActive(true);
    }

    public void ShowMusicList()
    {
        CloseAllApps();
        musicListScreen.SetActive(true);
    }

    public void ShowNowPlaying()
    {
        CloseAllApps();
        nowPlayingScreen.SetActive(true);
    }

    public void OpenCallerApp()
    {
        CloseAllApps();
        callerApp.SetActive(true);
        callerApp.GetComponent<CallerScreenManager>().ShowContacts();
    }

    public void OpenMessagesApp()
    {
        CloseAllApps();
        messagesAppScreen.SetActive(true);

        if (messageBadgeIcon != null)
            messageBadgeIcon.SetActive(false);

        hasUnreadMessage = false;

        bool anyMessages = false;

        foreach (var msg in messages)
        {
            if (msg.inboxButton != null)
                msg.inboxButton.SetActive(false);

            if (msg.messageScreen != null)
                msg.messageScreen.SetActive(false);

            if (PlayerPrefs.GetInt("MSG_" + msg.messageId, 0) == 1)
            {
                anyMessages = true;

                if (msg.inboxButton != null)
                    msg.inboxButton.SetActive(true);
            }
        }

        if (inboxScreen != null)
            inboxScreen.SetActive(anyMessages);

        if (noMessagesScreen != null)
            noMessagesScreen.SetActive(!anyMessages);
    }



    public void CloseAllApps()
    {
        homeScreen.SetActive(false);
        musicListScreen.SetActive(false);
        nowPlayingScreen.SetActive(false);
        callerApp.SetActive(false);
        messagesAppScreen.SetActive(false);
    }

    

    public void NotifyNoteCollected(int partId)
    {
        foreach (var msg in messages)
        {
            if (msg.messageId == partId)
            {
                PlayerPrefs.SetInt("MSG_" + partId, 1);

                hasUnreadMessage = true;

                if (messageBadgeIcon != null)
                    messageBadgeIcon.SetActive(true);

                if (notificationClip != null && notificationSource != null)
                    notificationSource.PlayOneShot(notificationClip);

                return;
            }
        }

        Debug.LogWarning("No message configured for note part: " + partId);
    }
    public void OpenMessageById(int messageId)
    {
        foreach (var msg in messages)
        {
            if (msg.messageId == messageId)
            {
                if (msg.messageScreen != null)
                    msg.messageScreen.SetActive(true);

                return;
            }
        }
    }



    void ShowMessage(int messageId)
    {
        foreach (var msg in messages)
        {
            if (msg.messageId == messageId && msg.messageScreen != null)
            {
                msg.messageScreen.SetActive(true);
                return;
            }
        }
    }

    

    public void PlaySongAndOpenPlayer(int index)
    {
        if (musicPlayer.IsSongPlaying(index))
        {
            ShowNowPlaying();
            return;
        }

        musicPlayer.PlaySongByIndex(index);
        ShowNowPlaying();
    }

    public void DebugClearMessages()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs cleared.");
    }



}
