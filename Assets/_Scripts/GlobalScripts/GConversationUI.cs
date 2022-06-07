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
    public GameObject subtitleBg;
    public Button[] choices = new Button[4];
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

    //Button
    public void Button_ContinueConvo(int answer)
    {
        int convoLength = convo.GetChoices().Length;
        if (convoLength > answer)
        {
            UpdateSubtitle(convo.GetChoices()[answer]);
        }
    }

    public void UpdateConvo(Conversation newConversation)
    {
        talkAble = newConversation;
    }

    public void UpdateSubtitle(ConvoOptions option)
    {
        subtitleBg.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        convo = option;
        subtitle.text = option.returnSentence;
        canMakeChoice = false;
        StartCoroutine(EndSubtitle(option));
    }

    public void SetupChoices(ConvoOptions option)
    {
        foreach (Button t in choices)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < option.GetChoices().Length; i++)
        {
            choices[i].gameObject.SetActive(true);
            choices[i].GetComponentInChildren<TMP_Text>().text = (i + 1) + " " + option.GetChoices()[i].playerSentence;
        }
    }

    IEnumerator EndSubtitle(ConvoOptions option)
    {
        yield return new WaitForSeconds(option.GetSubtitlesLength());
        canMakeChoice = true;
        if (option.endConv || option.hostileOption)
        {
            subtitle.text = "";
            foreach (Button t in choices)
            {
                t.gameObject.SetActive(false);
            }
            canMakeChoice = false;
            subtitleBg.SetActive(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

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