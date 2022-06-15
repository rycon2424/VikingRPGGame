using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TalkAble : MonoBehaviour
{
    public bool canTalk = true;
    [SerializeField] GameObject talkIndicator;
    [Space]
    [ReadOnly] public bool playerInRange;
    [ReadOnly] public PlayerBehaviour player;

    public virtual void Start()
    {
        talkIndicator.SetActive(false);
    }

    public virtual void Update()
    {
        if (canTalk)
        {
            if (player)
            {
                if (playerInRange)
                {
                    if (Input.GetKeyDown(player.pc.grab))
                    {
                        Talk();
                    }
                }
            }
        }
    }

    public virtual void Talk()
    {

    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (canTalk)
        {
            PlayerBehaviour potentialPlayer = other.GetComponent<PlayerBehaviour>();
            if (potentialPlayer)
            {
                playerInRange = true;
                player = potentialPlayer;
                talkIndicator.SetActive(true);
            }
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (canTalk)
        {
            if (other.GetComponent<PlayerBehaviour>())
            {
                playerInRange = false;
                talkIndicator.SetActive(false);
            }
        }
    }
}
