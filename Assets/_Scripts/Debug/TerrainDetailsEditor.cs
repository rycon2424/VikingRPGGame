using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TerrainDetailsEditor : MonoBehaviour
{
    public Terrain terrain;
    [Space]
    public Color dryColor;
    public Color healthyColor;

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
            //detail.Validate();
        }
        terrain.terrainData.detailPrototypes = details;
        //terrain.terrainData.RefreshPrototypes();
    }
}
