using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;         
    public AudioClip responseAudio;   
    public string nextNodeId;         
}
