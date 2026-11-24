using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class CallingScreenUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI contactNameText;
    public TextMeshProUGUI contactNumberText;
    public Image contactIconImage;

    public RectTransform nameMaskRect;

    public Transform choicesParent;
    public GameObject choiceButtonPrefab;

    [Header("Logic")]
    public CallerScreenManager screenManager;
    public CallFlowController callFlow;

    Coroutine nameScrollRoutine;

    public void ShowCallUI(ContactData contact)
    {
        if (contactNameText != null)
            contactNameText.text = contact.contactName;

        if (contactNumberText != null)
            contactNumberText.text = contact.phoneNumber;

        if (contactIconImage != null)
        {
            if (contact.contactIcon != null)
            {
                contactIconImage.sprite = contact.contactIcon;
                contactIconImage.enabled = true;
            }
            else
            {
                contactIconImage.enabled = false;
            }
        }

        StartNameScrollIfNeeded();
    }

    void StartNameScrollIfNeeded()
    {
        if (contactNameText == null || nameMaskRect == null)
            return;

        if (nameScrollRoutine != null)
            StopCoroutine(nameScrollRoutine);

        contactNameText.ForceMeshUpdate();

        float textWidth = contactNameText.preferredWidth;
        float maskWidth = nameMaskRect.rect.width;

        RectTransform textRect = contactNameText.rectTransform;

        // Always start fully visible
        textRect.anchoredPosition = new Vector2(0, textRect.anchoredPosition.y);

        if (textWidth > maskWidth)
        {
            nameScrollRoutine = StartCoroutine(ScrollName(textWidth, maskWidth));
        }
    }

    IEnumerator ScrollName(float textWidth, float maskWidth)
    {
        RectTransform textRect = contactNameText.rectTransform;

        float startX = 0f;
        float endX = -(textWidth - maskWidth);
        float scrollSpeed = 30f;

        while (true)
        {
            // Scroll LEFT only
            while (textRect.anchoredPosition.x > endX)
            {
                float newX = textRect.anchoredPosition.x - scrollSpeed * Time.deltaTime;
                newX = Mathf.Max(newX, endX);

                textRect.anchoredPosition =
                    new Vector2(newX, textRect.anchoredPosition.y);

                yield return null;
            }

            // Pause at the end
            yield return new WaitForSeconds(1.2f);

            // Snap cleanly back to start (no slide-in from right)
            textRect.anchoredPosition =
                new Vector2(startX, textRect.anchoredPosition.y);

            // Pause before next loop
            yield return new WaitForSeconds(0.5f);
        }
    }


    public void ShowStatus(string text)
    {
        statusText.text = text;
    }

    public void ShowChoices(DialogueNode node)
    {
        ClearChoices();

        for (int i = 0; i < node.choices.Length; i++)
        {
            int index = i;
            GameObject btn = Instantiate(choiceButtonPrefab, choicesParent);

            btn.GetComponentInChildren<TextMeshProUGUI>().text =
                node.choices[i].choiceText;

            btn.GetComponent<Button>().onClick.AddListener(() =>
                callFlow.OnChoiceSelected(index)
            );
        }
    }


    public void HideChoices()
    {
        ClearChoices();
    }

    void ClearChoices()
    {
        foreach (Transform child in choicesParent)
            Destroy(child.gameObject);
    }

    public void EndCall()
    {
        if (nameScrollRoutine != null)
            StopCoroutine(nameScrollRoutine);

        screenManager.ShowContacts();
    }

    public void OnHangUpPressed()
    {
        // Stop name scrolling
        if (nameScrollRoutine != null)
            StopCoroutine(nameScrollRoutine);

        // Stop call logic
        callFlow.ForceEndCall();
    }

}
