using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public enum HitboxType { player, enemies, npc, playerEnemies, playerNpc, enemiesNPC, all }
    public HitboxType type;
    [Space]
    public List<Actor> typeInTrigger = new List<Actor>();
    
    private void OnTriggerEnter(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();
        Actor rightActor = SetupActor(actor);
        if (rightActor)
        {
            typeInTrigger.Add(rightActor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();
        Actor rightActor = SetupActor(actor);
        if (rightActor)
        {
            typeInTrigger.Remove(rightActor);
        }
    }

    Actor SetupActor(Actor actor)
    {
        switch (type)
        {
            case HitboxType.player:
                if (actor is PlayerBehaviour)
                {
                    return actor;
                }
                break;
            case HitboxType.enemies:
                if (actor is EnemyPawn)
                {
                    return actor;
                }
                break;
            case HitboxType.npc:
                if (actor is Npc)
                {
                    return actor;
                }
                break;
            case HitboxType.playerEnemies:
                if (actor is EnemyPawn || actor is PlayerBehaviour)
                {
                    return actor;
                }
                break;
            case HitboxType.playerNpc:
                if (actor is Npc || actor is PlayerBehaviour)
                {
                    return actor;
                }
                break;
            case HitboxType.enemiesNPC:
                if (actor is EnemyPawn || actor is Npc)
                {
                    return actor;
                }
                break;
            case HitboxType.all:
                if (actor is EnemyPawn || actor is Npc || actor is PlayerBehaviour)
                {
                    return actor;
                }
                break;
            default:
                break;
        }
        actor = null;
        return null;
    }

    public bool HurtAllTargets(int damage)
    {
        foreach (Actor enemy in typeInTrigger)
        {
            enemy.TakeDamage(damage);
        }
        if (typeInTrigger.Count > 0)
        {
            return true;
        }
        return false;
    }
}
