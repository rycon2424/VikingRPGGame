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

    public static GConversationUI Instance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;
    }

    public void UpdateSubtitle(ConvoOptions option)
    {
        Debug.Log(option.playerSentence);
        subtitle.text = option.returnSentence;
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
            choices[i].text = option.GetChoices()[i].playerSentence;
        }
    }

    IEnumerator EndSubtitle(ConvoOptions option)
    {
        yield return new WaitForSeconds(option.GetSubtitlesLength());
        if (option.endConv || option.hostileOption)
        {

        }
        else
        {
            SetupChoices(option);
        }
        option.startEvent.Invoke();
        option = null;
    }
}