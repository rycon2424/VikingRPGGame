using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public enum HitboxType { player, enemies}
    public HitboxType type;
    [Space]
    public List<Actor> typeInTrigger = new List<Actor>();

    private void OnTriggerEnter(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();
        switch (type)
        {
            case HitboxType.player:
                break;
            case HitboxType.enemies:
                if (actor is EnemyPawn)
                {
                    typeInTrigger.Add(actor);
                }
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();
        switch (type)
        {
            case HitboxType.player:
                break;
            case HitboxType.enemies:
                if (actor is EnemyPawn)
                {
                    typeInTrigger.Remove(actor);
                }
                break;
            default:
                break;
        }
    }

    public void HurtAllTargets(int damage)
    {
        foreach (Actor enemy in typeInTrigger)
        {
            enemy.TakeDamage(damage);
        }
    }
}
