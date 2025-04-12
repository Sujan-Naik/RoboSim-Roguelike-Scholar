using UnityEngine;

public static class TerrainNoiseHelper
{

    public static Vector3 GetRandomPosition(float[,] terrainNoiseData)
    {
        var x = Randomisation.GetInt(CoreGeneration.LENGTH * CoreGeneration.CHUNK_LENGTH);
        var z = Randomisation.GetInt(CoreGeneration.LENGTH * CoreGeneration.CHUNK_LENGTH);
        var position = new Vector3(x, terrainNoiseData[x, z] * ChunkGeneration.LENGTH, z);
        
        return position;
    }

    public static Vector3 GetPosition(float[,] terrainNoiseData, int x, int z)
    {
        return new Vector3(x, terrainNoiseData[x, z] * ChunkGeneration.LENGTH, z);
    }
}