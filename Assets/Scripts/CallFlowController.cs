using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CallFlowController : MonoBehaviour
{
    [Header("Music Integration")]
    public PhoneMusicPlayer musicPlayer;

    [Header("Audio Sources")]
    public AudioSource voiceSource;
    public AudioSource interferenceSource;

    [Header("Audio")]
    public AudioClip disconnectClip;


    [Header("UI")]
    public CallingScreenUI callingUI;
    public CallerScreenManager screenManager;

    ContactData currentContact;
    DialogueNode currentNode;
    Dictionary<string, DialogueNode> nodeLookup;

    bool wasMusicPlayingBeforeCall;


    public void StartCall(ContactData contact)
    {
        // 🎵 Pause music if playing
        if (musicPlayer != null)
        {
            wasMusicPlayingBeforeCall = musicPlayer.IsPlaying();
            if (wasMusicPlayingBeforeCall)
                musicPlayer.Pause();
        }

        currentContact = contact;

        // Build lookup table
        nodeLookup = new Dictionary<string, DialogueNode>();
        foreach (var node in contact.dialogueNodes)
        {
            if (!nodeLookup.ContainsKey(node.nodeId))
                nodeLookup.Add(node.nodeId, node);
        }

        // Resolve start node
        if (!nodeLookup.TryGetValue(contact.startNodeId, out currentNode))
        {
            Debug.LogError($"Start node '{contact.startNodeId}' not found!");
            return;
        }

        screenManager.ShowCalling();
        callingUI.ShowCallUI(contact);
        callingUI.ShowStatus("Calling...");

        StartCoroutine(CallRoutine());
    }

    IEnumerator CallRoutine()
    {
        voiceSource.PlayOneShot(currentContact.ringingClip);
        yield return new WaitForSeconds(currentContact.ringingClip.length);

         
        interferenceSource.Play();

        
        PlayCurrentNode();

    }

    void PlayCurrentNode()
    {
        if (currentNode == null)
        {
            EndCall();
            return;
        }

        callingUI.ShowStatus("Connected");

        voiceSource.PlayOneShot(currentNode.contactLine);
        StartCoroutine(WaitThenShowChoices(currentNode.contactLine.length));
    }

    IEnumerator WaitThenShowChoices(float delay)
    {
        yield return new WaitForSeconds(delay);
        callingUI.ShowChoices(currentNode);
    }

    public void OnChoiceSelected(int index)
    {
        DialogueChoice choice = currentNode.choices[index];

        callingUI.HideChoices();

        StartCoroutine(PlayResponseWithDelay(choice));
    }



    IEnumerator PlayResponseWithDelay(DialogueChoice choice)
    {
        // ⏳ Natural pause before speaking
        yield return new WaitForSeconds(0.8f);

        // 🔇 If no response audio, skip straight to next node
        if (choice.responseAudio == null)
        {
            GoToNextNode(choice);
            yield break;
        }

        // 🔊 Play response audio
        voiceSource.PlayOneShot(choice.responseAudio);

        yield return new WaitForSeconds(choice.responseAudio.length);

        GoToNextNode(choice);
    }

    void GoToNextNode(DialogueChoice choice)
    {
        if (!string.IsNullOrEmpty(choice.nextNodeId) &&
            nodeLookup.TryGetValue(choice.nextNodeId, out var nextNode))
        {
            currentNode = nextNode;
            PlayCurrentNode();
        }
        else
        {
            EndCall();
        }
    }



    IEnumerator EndCallRoutine()
    {
        
        if (interferenceSource.isPlaying)
            interferenceSource.Stop();

        
        if (disconnectClip != null)
            voiceSource.PlayOneShot(disconnectClip);

        
        callingUI.HideChoices();
        callingUI.ShowStatus("Ended");

        
        yield return new WaitForSeconds(1.5f);

        
        if (musicPlayer != null && wasMusicPlayingBeforeCall)
        {
            musicPlayer.Resume();
        }


        
        screenManager.ShowContacts();
        wasMusicPlayingBeforeCall = false;

    }




    public void ForceEndCall()
    {
        StopAllCoroutines();

        if (voiceSource.isPlaying)
            voiceSource.Stop();

        if (interferenceSource.isPlaying)
            interferenceSource.Stop();

        StartCoroutine(EndCallRoutine());
    }



    void EndCall()
    {
        StartCoroutine(EndCallRoutine());
    }

}
