using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [Header("Actor Stats")]
    public float health = 100;
    public float stamina = 100;
    public bool dead = false;
    public bool invincible = false;
    public int damage = 20;
    [Space]
    public ParticleSystem blood;
    public LayerMask bloodSplatter;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Vector3 givenVelocity;

    public virtual void TakeDamage(int damage)
    {
        if (dead == true || invincible == true)
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