using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLookatGeneric : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] Vector3 offset = new Vector3(180, 90, 0);
    [SerializeField] Vector3 lookOffset = new Vector3(0, 1, 0);
    Transform target;
    EnemyBehaviour eb;

    private void Start()
    {
        eb = GetComponent<EnemyBehaviour>();
    }

    private void LateUpdate()
    {
        if (eb.currentState == EnemyBehaviour.EnemyStates.inCombat || eb.currentState == EnemyBehaviour.EnemyStates.chasing)
        {
            if (eb.PlayerInSight())
            {
                if (target == null)
                {
                    target = eb.player;
                }
                head.LookAt(target.position + lookOffset);
                head.rotation = head.rotation * Quaternion.Euler(offset);
            }
        }
    }
}
