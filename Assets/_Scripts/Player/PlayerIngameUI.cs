using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIngameUI : MonoBehaviour
{
    public float healthPerTick = 0.03f;
    public float staminaPerTick = 0.75f;
    [Space]
    public float staminaIncrease = 1.2f;
    [Space]
    public Image healthSlider;
    public Image staminaSlider;

    private PlayerBehaviour pb;
    private float originalStamina;

    IEnumerator Start()
    {
        originalStamina = staminaPerTick;
        pb = GetComponentInParent<PlayerBehaviour>();
        while (pb.dead == false)
        {
            yield return new WaitForEndOfFrame();
            pb.health += healthPerTick * Time.deltaTime;
            pb.stamina += staminaPerTick * Time.deltaTime;

            if (pb.health > 100)
            {
                pb.health = 100;
            }
            if (pb.stamina > 100)
            {
                pb.stamina = 100;
                ResetGain();
            }

            staminaPerTick *= staminaIncrease;
        }
    }

    private void Update()
    {
        UpdateHealthBar();
        UpdateStaminaBar();
    }

    public void UpdateHealthBar()
    {
        healthSlider.fillAmount = (pb.health / 100);
    }

    public void UpdateStaminaBar()
    {
        staminaSlider.fillAmount = (pb.stamina / 100);
    }

    public void ResetGain()
    {
        staminaPerTick = originalStamina;
    }
}
