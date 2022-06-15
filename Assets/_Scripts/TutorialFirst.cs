using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFirst : MonoBehaviour
{
    public GameObject setActive;

    void Update()
    {
        if (Input.anyKey)
        {
            this.enabled = false;
            setActive.SetActive(true);
        }
    }
}
