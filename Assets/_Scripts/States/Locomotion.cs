using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion : State
{

    public override void AnimatorIKUpdate(PlayerBehaviour pb)
    {

    }

    public override void OnStateEnter(PlayerBehaviour pb)
    {
        pb.anim.ResetTrigger("Jump");
        pb.anim.ResetTrigger("fall");
        pb.anim.applyRootMotion = true;
        pb.cc.enabled = true;
        pb.airtime = 0;
    }

    public override void OnStateExit(PlayerBehaviour pb)
    {

    }

    float turnSmoothVelocity;

    public override void StateUpdate(PlayerBehaviour pb)
    {
        pb.Grounded();
        if (pb.combat.attacking && pb.armed)
        {
            pb.RotateTowardsCamera();
        }
        else
        {
            RotateToCam(pb);
        }
        GrabLedge(pb);
        Movement(pb);
        if (pb.armed)
        {
            pb.combat.CombatUpdate(pb);
        }
        if (Input.GetKeyDown(pb.pc.armWeapons))
        {
            pb.GrabHideWeapon();
        }
        if (pb.airtime > 0.75f)
        {
            pb.anim.SetTrigger("fall");
            pb.stateMachine.GoToState(pb, "InAir");
        }
    }

    void RotateToCam(PlayerBehaviour pb)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(x, 0f, y).normalized;

        if (dir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + pb.oc.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(pb.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            pb.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

    }
    
    void GrabLedge(PlayerBehaviour pb)
    {
        return;
        if (pb.grounded == false)
        {
            if (Input.GetKey(pb.pc.grab))
            {
                pb.LedgeInfo();
            }
        }
    }

    void Movement(PlayerBehaviour pb)
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        pb.anim.SetFloat("x", x);
        pb.anim.SetFloat("y", y);
        pb.anim.SetFloat("y+x", (Mathf.Abs(x) + Mathf.Abs(y)));

        //Walking
        if (x != 0 || y != 0)
        {
            pb.anim.SetBool("Walking", true);
        }
        else
        {
            pb.anim.SetBool("Walking", false);
            pb.anim.SetBool("Sprinting", false);
        }
        //Sprinting
        if (Input.GetKeyDown(pb.pc.sprint))
        {
            pb.anim.SetBool("Sprinting", !pb.anim.GetBool("Sprinting"));
        }
        //else
        //{
        //    pb.anim.SetBool("Sprinting", false);
        //}
        //Jump
        if (Input.GetKeyDown(pb.pc.jump))
        {
            if (pb.anim.GetFloat("CombatAnim") == 0)
            {
                pb.anim.SetTrigger("Jump");
                pb.stateMachine.GoToState(pb, "InAir");
            }
            else
            {
                if (pb.stamina > pb.combat.rollStaminaCost)
                {
                    pb.stamina -= pb.combat.rollStaminaCost;

                    if (pb.stamina < 0)
                        pb.stamina = 0;

                    pb.playerUI.UpdateStaminaBar();
                    pb.playerUI.ResetGain();

                    pb.anim.SetTrigger("roll");
                }
            }
        }
    }
}
