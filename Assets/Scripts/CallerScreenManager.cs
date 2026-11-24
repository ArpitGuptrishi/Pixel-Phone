using UnityEngine;

public class CallerScreenManager : MonoBehaviour
{
    public GameObject contactsScreen;
    public GameObject dialerScreen;
    public GameObject callingScreen;

    void Start()
    {
        ShowContacts();
    }

    public void ShowContacts()
    {
        contactsScreen.SetActive(true);
        if (dialerScreen != null) dialerScreen.SetActive(false);
        callingScreen.SetActive(false);
    }

    public void ShowDialer()
    {
        contactsScreen.SetActive(false);
        if (dialerScreen != null) dialerScreen.SetActive(true);
        callingScreen.SetActive(false);
    }

    public void ShowCalling()
    {
        contactsScreen.SetActive(false);
        if (dialerScreen != null) dialerScreen.SetActive(false);
        callingScreen.SetActive(true);
    }
}
