using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TerrainDetailsEditor : MonoBehaviour
{
    public Terrain terrain;
    [Header("DetailsEditor")]
    public Color dryColor;
    public Color healthyColor;

    [Header("LayerEditor")]
    public Texture2D snowTexture;
    public List<TerrainLayerSettings> layerSettings = new List<TerrainLayerSettings>();

    [Button]
    public void UpdateDetails()
    {
        if (terrain == null)
            return;

        DetailPrototype[] details = terrain.terrainData.detailPrototypes;
        foreach (DetailPrototype detail in details)
        {
            detail.dryColor = dryColor;
            detail.healthyColor = healthyColor;
        }
        terrain.terrainData.detailPrototypes = details;
    }

    [Button]
    public void GetLayers()
    {
        TerrainLayer[] layers = terrain.terrainData.terrainLayers;
        foreach (var item in layers)
        {
            TerrainLayerSettings s = new TerrainLayerSettings();
            s.oldTexture = item.diffuseTexture;
            layerSettings.Add(s);
        }
    }

    [Button]
    public void ResetLayers()
    {
        TerrainLayer[] layers = terrain.terrainData.terrainLayers;
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].diffuseTexture = layerSettings[i].oldTexture;
        }
    }

    [Button]
    public void SetSnowLayers()
    {
        TerrainLayer[] layers = terrain.terrainData.terrainLayers;
        foreach (var item in layers)
        {
            item.diffuseTexture = snowTexture;
        }
    }
}

[System.Serializable]
public struct TerrainLayerSettings
{
    public Texture2D oldTexture;
}