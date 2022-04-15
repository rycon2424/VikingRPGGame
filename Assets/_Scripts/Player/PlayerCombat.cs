using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public bool attacking;
    public float rollStaminaCost;

    public void CombatUpdate(PlayerBehaviour pb)
    {
        if (attacking == false && pb.armed == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                attacking = true;
                pb.anim.SetTrigger("Attack");
            }
        }
    }

    public void CanAttackAgain()
    {
        attacking = false;
    }
}