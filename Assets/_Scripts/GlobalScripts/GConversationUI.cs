using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using Sirenix.Serialization;

public class GConversationUI : MonoBehaviour
{
    public TMP_Text subtitle;
    public TMP_Text[] choices = new TMP_Text[4];
    [Space]
    [ReadOnly][ShowInInspector] bool canMakeChoice;
    [ReadOnly][ShowInInspector] Conversation talkAble;
    [ReadOnly][ShowInInspector] ConvoOptions convo;

    public static GConversationUI Instance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;
    }

    private void Start()
    {
        convo = new ConvoOptions();
    }

    private void Update()
    {
        if (canMakeChoice)
        {
            int convoLength = convo.GetChoices().Length;
            if (convoLength > 0 && Input.GetKeyDown(KeyCode.Alpha1))
            {
                UpdateSubtitle(convo.GetChoices()[0]);
            }
            if (convoLength > 1 && Input.GetKeyDown(KeyCode.Alpha2))
            {
                UpdateSubtitle(convo.GetChoices()[1]);
            }
            if (convoLength > 2 && Input.GetKeyDown(KeyCode.Alpha3))
            {
                UpdateSubtitle(convo.GetChoices()[2]);
            }
            if (convoLength > 3 && Input.GetKeyDown(KeyCode.Alpha4))
            {
                UpdateSubtitle(convo.GetChoices()[3]);
            }
        }
    }

    public void UpdateConvo(Conversation newConversation)
    {
        talkAble = newConversation;
    }

    public void UpdateSubtitle(ConvoOptions option)
    {
        convo = option;
        subtitle.text = option.returnSentence;
        canMakeChoice = false;
        StartCoroutine(EndSubtitle(option));
    }

    public void SetupChoices(ConvoOptions option)
    {
        foreach (TMP_Text t in choices)
        {
            t.text = "";
        }
        for (int i = 0; i < option.GetChoices().Length; i++)
        {
            choices[i].text = (i + 1) + " " + option.GetChoices()[i].playerSentence;
        }
    }

    IEnumerator EndSubtitle(ConvoOptions option)
    {
        yield return new WaitForSeconds(option.GetSubtitlesLength());
        canMakeChoice = true;
        if (option.endConv || option.hostileOption)
        {
            subtitle.text = "";
            foreach (TMP_Text t in choices)
            {
                t.text = "";
            }
            canMakeChoice = false;
            talkAble.EndConversation();
        }
        else
        {
            SetupChoices(option);
        }
        option.startEvent.Invoke();
        option = null;
    }
}