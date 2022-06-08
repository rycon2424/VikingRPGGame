using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EncounterSpawner : MonoBehaviour
{
    public GameObject enemiesToSpawn;
    [Range(0, 101)] public int chanceToSpawn = 50;
    [Range(1, 4)] public int spawnCount;
    public Transform[] spawnpositions;
    [Space]
    public int cooldown;

    [SerializeField] [ReadOnly] private bool onCooldown;
    
    public void OnTriggerEnter(Collider other)
    {
        PlayerBehaviour pb = other.GetComponent<PlayerBehaviour>();
        if (onCooldown)
        {
            if (pb != null)
            {
                int randomPercentage = Random.Range(1, 101);
                if (randomPercentage <= chanceToSpawn)
                {
                    for (int i = 0; i < spawnCount; i++)
                    {
                        Instantiate(enemiesToSpawn, spawnpositions[i].position, Quaternion.identity);
                    }
                    onCooldown = true;
                    StartCoroutine(Cooldown());
                }
            }
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}