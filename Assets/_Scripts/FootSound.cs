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
        int i = 0;
        for (i = 0; i < textureValues.Length; i++)
        {
            if (textureValues[i] == 1)
            {
                break;
            }
        }
        foreach (SoundVariant sound in sounds.soundAssets)
        {
            if (sound.terrainTextureOrder == i)
            {
                sound.Play(footPlayer);
                break;
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
