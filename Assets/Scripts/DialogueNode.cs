using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    [Header("Node ID")]
    public string nodeId;   

    [Header("Contact Line")]
    public AudioClip contactLine;

    [Header("Player Choices")]
    public DialogueChoice[] choices;
}
