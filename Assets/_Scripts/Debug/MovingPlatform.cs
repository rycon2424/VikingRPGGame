using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Moving
{
    public Transform beginpoint;
    public Transform endpoint;
    public float speed = 1;

    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endpoint.position, step);

        if (Vector3.Distance(transform.position, endpoint.position) < 0.05f)
        {
            Transform tempBegin = beginpoint;
            Transform tempEnd   = endpoint;
            beginpoint = tempEnd;
            endpoint = tempBegin;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (beginpoint != null && endpoint != null)
        {
            Gizmos.DrawWireCube(beginpoint.position ,Vector3.one);
            Gizmos.DrawWireCube(endpoint.position, Vector3.one);
        }
    }
}
