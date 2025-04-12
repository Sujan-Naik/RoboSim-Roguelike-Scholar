using UnityEngine;

public class ManualRelocatorPopulator : CorePopulator
{
    public GameObject ToRelocate;

    private Vector3 position;

    protected override void GenerateGameObjects(float[,] terrainNoiseData)
    {
        position = TerrainNoiseHelper.GetRandomPosition(terrainNoiseData);
    }

    protected GameObject SpawnObject()
    {
        GameObject obj =  Instantiate(ToRelocate, position, Quaternion.identity);
        obj.SetActive(true);
        return obj;
    }
}