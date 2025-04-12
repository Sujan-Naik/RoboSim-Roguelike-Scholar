using UnityEngine;
using System;
public class LootablePopulator : CorePopulator
{
    public int amount = 1;
    
    public GameObject lootablePrefab;

    protected override void GenerateGameObjects(float[,] terrainNoiseData)
    {

        for (var i = 0; i < amount; i++)
        {
            var lootable = Instantiate(lootablePrefab, TerrainNoiseHelper.GetRandomPosition(terrainNoiseData), Quaternion.identity);
            lootable.SetActive(true);
            
        }
    }



}