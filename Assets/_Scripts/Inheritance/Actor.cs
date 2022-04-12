using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [Header("Actor Stats")]
    public int health = 100;
    public bool dead = false;
    public int damage = 20;
    [Space]
    public ParticleSystem blood;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Vector3 givenVelocity;

    public virtual void TakeDamage(int damage)
    {
        if (dead == true)
        {
            return;
        }
        blood.Play();
    }

    public virtual void Death() { }

    public void OnAnimatorMove()
    {
        anim.ApplyBuiltinRootMotion();
        if (dead == false) // Cause ragdoll physics
        {
            transform.position += givenVelocity;
        }
    }
}