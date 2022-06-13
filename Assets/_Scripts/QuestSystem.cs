using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestSystem : MonoBehaviour
{
    [SerializeField] TMP_Text questTitle;
    [SerializeField] TMP_Text questDescription;

    public void B_UpdateQuestTitle(string title)
    {
        questTitle.text = title;
    }

    public void B_UpdateQuestDescription(string description)
    {
        questDescription.text = description;
    }

}
