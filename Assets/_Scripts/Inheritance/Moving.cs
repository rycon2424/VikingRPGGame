using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    [Header("MovingProperties")]
    public float scaler = 1;
    public Vector3 currentVelocity;
    [Space]
    private Vector3 oldPos;
    private List<Actor> ActorsOnPlatform = new List<Actor>();

    public virtual void FixedUpdate()
    {
        CalculateVelocity();
    }

    void CalculateVelocity()
    {
        currentVelocity = (transform.position - oldPos) / Time.deltaTime;
        oldPos = transform.position;

        foreach (Actor a in ActorsOnPlatform)
        {
            a.givenVelocity = (currentVelocity * scaler) * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();
        if (actor)
        {
            ActorsOnPlatform.Add(actor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();
        if (actor)
        {
            ActorsOnPlatform.Remove(actor);
        }
    }
}
