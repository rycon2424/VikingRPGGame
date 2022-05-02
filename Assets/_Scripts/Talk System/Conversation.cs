using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class Conversation : TalkAble
{
    [Space]
    [SerializeField] GameObject convoCamera;
    [ShowInInspector] [SerializeField] ConvoOptions openingChoice = new ConvoOptions();

    public override void Start()
    {
        base.Start();
    }

    public override void Talk()
    {
        base.Talk();
        StartConversation();
    }

    public void StartConversation()
    {
        convoCamera.SetActive(true);
        player.oc.gameObject.SetActive(false);
        GConversationUI.Instance.UpdateConvo(this);
        GConversationUI.Instance.UpdateSubtitle(openingChoice);
    }

    public void EndConversation()
    {
        player.oc.gameObject.SetActive(true);
        convoCamera.SetActive(false);
    }
}

[System.Serializable]
public class ConvoOptions
{
    public string playerSentence;
    [TextArea(0, 5)] public string returnSentence = "sample text";
    public float durationPerWord = 0.35f;
    [Space]
    public bool endConv;
    public bool hostileOption;
    public UnityEvent startEvent = new UnityEvent();
    [Space]

    [ShowIf("@endConv == false")] [SerializeField] ConvoOptions[] choices = new ConvoOptions[0];

    public ConvoOptions[] GetChoices()
    {
        return choices;
    }

    public float GetSubtitlesLength()
    {
        int wordCount = 0, index = 0;
        float length = 0;

        while (index < returnSentence.Length && char.IsWhiteSpace(returnSentence[index]))
            index++;

        while (index < returnSentence.Length)
        {
            while (index < returnSentence.Length && !char.IsWhiteSpace(returnSentence[index]))
                index++;

            wordCount++;

            while (index < returnSentence.Length && char.IsWhiteSpace(returnSentence[index]))
                index++;
        }

        length = wordCount * durationPerWord;

        return length;
    }
}

