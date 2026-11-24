using UnityEngine;
using TMPro;

public class ScrollingTextTMP : MonoBehaviour
{
    public float scrollSpeed = 30f;
    public float startDelay = 1f;
    public float endDelay = 1f;

    private TextMeshProUGUI text;
    private float textWidth;
    private float containerWidth;
    private float timer;
    private bool shouldScroll;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        ResetScroll();
    }

    void Update()
    {
        if (!shouldScroll)
            return;

        timer += Time.deltaTime;
        if (timer < startDelay)
            return;

        text.rectTransform.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

        if (Mathf.Abs(text.rectTransform.anchoredPosition.x) > textWidth)
        {
            timer = -endDelay;
            ResetPosition();
        }
    }

    public void ResetScroll()
    {
        Canvas.ForceUpdateCanvases();

        containerWidth = ((RectTransform)text.rectTransform.parent).rect.width;
        textWidth = text.preferredWidth;

        ResetPosition();

        shouldScroll = textWidth > containerWidth;
        timer = 0f;
    }

    private void ResetPosition()
    {
        var pos = text.rectTransform.anchoredPosition;
        pos.x = 0f;          // reset horizontal only
        text.rectTransform.anchoredPosition = pos;
    }

}
