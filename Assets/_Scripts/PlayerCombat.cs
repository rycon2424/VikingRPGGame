using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public bool attacking;

    public void CombatUpdate(PlayerBehaviour pb)
    {
        if (attacking == false)
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