using UnityEngine;

public class PhoneMessageManager : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("DEBUG TESTING")]
    public bool testMessage1;
    public bool testMessage2;
#endif

    [Header("UI References")]
    public GameObject messagesAppScreen;
    public GameObject homeScreen;
    public GameObject badgeIcon;          // the small "1" icon on app
    public AudioSource notificationSource;
    public AudioClip notificationClip;

    [Header("Messages")]
    public MessageData[] messages;

    int pendingMessageId = -1;
    bool hasUnreadMessage = false;

    void Start()
    {
        badgeIcon.SetActive(false);
    }

    
    public void NotifyNoteCollected(int partId)
    {
        // Find matching message
        foreach (var msg in messages)
        {
            if (msg.messageId == partId)
            {
                pendingMessageId = partId;
                hasUnreadMessage = true;

                // Show badge
                badgeIcon.SetActive(true);

                // Play notification
                if (notificationClip != null && notificationSource != null)
                    notificationSource.PlayOneShot(notificationClip);

                return;
            }
        }

        Debug.LogWarning("No message configured for note part: " + partId);
    }

    
    public void OpenMessagesApp()
    {
        homeScreen.SetActive(false);
        messagesAppScreen.SetActive(true);

        // Clear all message screens first
        foreach (var msg in messages)
        {
            msg.messageScreen.SetActive(false);
        }

        if (hasUnreadMessage)
        {
            ShowMessage(pendingMessageId);
            hasUnreadMessage = false;
            pendingMessageId = -1;
            badgeIcon.SetActive(false);
        }
    }

    void ShowMessage(int messageId)
    {
        foreach (var msg in messages)
        {
            if (msg.messageId == messageId)
            {
                msg.messageScreen.SetActive(true);
                return;
            }
        }
    }

    
    public void CloseMessagesApp()
    {
        messagesAppScreen.SetActive(false);
        homeScreen.SetActive(true);
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

}
