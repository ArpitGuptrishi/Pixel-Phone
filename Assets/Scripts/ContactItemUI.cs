using UnityEngine;
using TMPro;

public class ContactItemUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI numberText;

    public ContactData contact;
    public CallFlowController callFlow;

    public void Setup(ContactData data)
    {
        contact = data;
        nameText.text = data.contactName;
        numberText.text = data.phoneNumber;
    }

    // Called when the contact row is clicked
    public void OnCallPressed()
    {
        callFlow.StartCall(contact);
    }
}
