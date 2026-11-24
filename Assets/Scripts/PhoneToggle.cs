using UnityEngine;

public class PhoneToggle : MonoBehaviour
{
    public GameObject phoneCanvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            phoneCanvas.SetActive(!phoneCanvas.activeSelf);
        }
    }
}
