using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FootSound : MonoBehaviour
{
    public SoundAsset sounds;
    public AudioSource footPlayer;
    [Space]
    public bool isOnTerrain;
    public LayerMask layerMask;
    [Space]
    public Terrain terrainObject;
    [ReadOnly] public float[] textureValues;

    private Transform player;

    private void Start()
    {
        player = transform;
    }

    public void X_PlayFootStep()
    {
        isOnTerrain = RayHit(transform.position + transform.up * 0.5f, Vector3.down, 1, layerMask);
        if (isOnTerrain)
        {
            UpdatePosition();
            PlayRandomFootStep();
        }
    }

    void PlayRandomFootStep()
    {
        int highesti = 0;
        float highestSize = 0;
        for (int i = 0; i < textureValues.Length; i++)
        {
            if (highesti == 0 && textureValues[i] > 0)
            {
                highestSize = textureValues[i];
                highesti = i;
            }
            else if (textureValues[i] > highestSize)
            {
                highestSize = textureValues[i];
                highesti = i;
            }
        }
        foreach (SoundVariant sound in sounds.soundAssets)
        {
            foreach (var order in sound.terrainTextureOrder)
            {
                if (order == highesti)
                {
                    sound.Play(footPlayer);
                    return;
                }
            }
        }
    }

    void UpdatePosition()
    {
        Vector3 terrainPos = player.position - terrainObject.transform.position;
        Vector3 mapPosition = new Vector3
            (
            terrainPos.x / terrainObject.terrainData.size.x,
            0,
            terrainPos.z / terrainObject.terrainData.size.z
            );
        float xCoord = mapPosition.x * terrainObject.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * terrainObject.terrainData.alphamapHeight;

        CheckTexture((int)xCoord, (int)zCoord);
    }

    void CheckTexture(int x, int z)
    {
        float[,,] splatMap = terrainObject.terrainData.GetAlphamaps(x, z, 1, 1);
        textureValues = new float[splatMap.Length];
        for (int i = 0; i < splatMap.Length; i++)
        {
            textureValues[i] = splatMap[0, 0, i];
        }
    }

    public bool RayHit(Vector3 start, Vector3 dir, float length, LayerMask lm)
    {
        RaycastHit hit;
        Ray ray = new Ray(start, dir);
        Debug.DrawRay(start, dir * length, Color.blue, 0.1f);
        if (Physics.Raycast(ray, out hit, length, lm))
        {
            if (hit.collider.tag == "Terrain")
            {
                Terrain terrainHit = hit.collider.GetComponent<Terrain>();
                if (terrainObject == null || terrainObject.gameObject.name != terrainHit.gameObject.name)
                {
                    terrainObject = terrainHit;
                }
                return true;
            }
        }
        return false;
    }
}
