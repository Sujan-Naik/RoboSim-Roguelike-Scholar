using UnityEngine;
using System;
public class Relocator : CorePopulator
{
    public GameObject[] ToRelocate;
    protected override void GenerateGameObjects(float[,] terrainNoiseData)
    {
        var position = TerrainNoiseHelper.GetRandomPosition(terrainNoiseData);
        foreach (var go in ToRelocate)
        {
            go.transform.position = position;
            go.SetActive(true); 
        }
        
    }
}