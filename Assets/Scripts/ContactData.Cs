using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ContactData
{
    [Header("Basic Info")]
    public string contactName;
    public string phoneNumber;
    public Sprite contactIcon;

    [Header("Call Audio")]
    public AudioClip ringingClip;

    [Header("Dialogue Data")]
    public string startNodeId;
    public List<DialogueNode> dialogueNodes;
}
