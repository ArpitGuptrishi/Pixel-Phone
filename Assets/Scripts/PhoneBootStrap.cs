using UnityEngine;

public class PhoneBootstrap : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
