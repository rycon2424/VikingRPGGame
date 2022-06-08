using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerQuest : MonoBehaviour
{
    [SerializeField] TMP_Text questTitle;
    [SerializeField] TMP_Text questDescription;

    public void UpdateQuestTitle(string newTitle)
    {
        questTitle.text = newTitle;
    }

    public void UpdateQuestDescription(string newDescription)
    {
        questDescription.text = newDescription;
    }
}
