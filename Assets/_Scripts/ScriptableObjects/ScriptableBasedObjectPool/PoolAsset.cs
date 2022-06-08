using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "PoolObject", menuName = "ScriptableObjects/PoolObject", order = 2)]
public class PoolAsset : ScriptableObject
{
    public string poolName;
    [Space]
    public int amount;
    public List<GameObject> objects = new List<GameObject>();

    [ReadOnly] public List<GameObject> spawnedObjects = new List<GameObject>();
}