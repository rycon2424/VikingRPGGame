using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GObjectPool : MonoBehaviour
{
    public static GObjectPool instance;
    public List<PoolAsset> poolObjects = new List<PoolAsset>();
    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        foreach (PoolAsset poolObject in poolObjects)
        {
            poolObject.spawnedObjects.Clear();

            GameObject temp = new GameObject(poolObject.poolName);
            temp.transform.SetParent(transform);

            foreach (GameObject go in poolObject.objects)
            {
                for (int i = 0; i < poolObject.amount; i++)
                {
                    GameObject g = Instantiate(go, temp.transform);
                    g.SetActive(false);
                    poolObject.spawnedObjects.Add(g);
                }
            }
        }
    }

    public void SpawnObject(string poolName, Vector3 position, Vector3 rotation, float time = 2, Vector3 rotationOffset = new Vector3())
    {
        foreach (PoolAsset pool in poolObjects)
        {
            if (pool.poolName == poolName)
            {
                int length = pool.spawnedObjects.Count;
                GameObject g = pool.spawnedObjects[Random.Range(0, length)];
                if (g == null)
                {
                    Debug.Log($"The Pool {poolName} is empty!");
                    return;
                }

                g.transform.position = position;
                Quaternion wantedRotation = Quaternion.LookRotation(rotation);
                g.transform.rotation = wantedRotation;
                g.transform.eulerAngles += rotationOffset;
                g.SetActive(true);

                pool.spawnedObjects.Remove(g);

                StartCoroutine(ReturnToPool(poolName, g, time));
                return;
            }
        }
        Debug.Log($"No pool named '{poolName}' found");
    }

    IEnumerator ReturnToPool(string poolName, GameObject gObject, float time)
    {
        yield return new WaitForSeconds(time);
        foreach (PoolAsset pool in poolObjects)
        {
            if (pool.poolName == poolName)
            {
                gObject.SetActive(false);
                pool.spawnedObjects.Add(gObject);
                break;
            }
        }
    }
}
