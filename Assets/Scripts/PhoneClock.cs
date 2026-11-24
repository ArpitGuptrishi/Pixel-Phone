using System;
using UnityEngine;
using TMPro;

public class PhoneClock : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    void Start()
    {
        UpdateTime();
        InvokeRepeating(nameof(UpdateTime), 60f, 60f);
    }

    void UpdateTime()
    {
        timeText.text = DateTime.Now.ToString("HH:mm");
    }
}
