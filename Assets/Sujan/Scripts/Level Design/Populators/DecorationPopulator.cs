using UnityEngine;
public class DecorationPopulator : CorePopulator
{

    public int gridLacunarity;

    public GameObject[] toPopulate;
    
    
    
    protected override void GenerateGameObjects(float[,] terrainNoiseData)
    {
        for (var x = 0; x < CoreGeneration.LENGTH * CoreGeneration.CHUNK_LENGTH; x+= gridLacunarity)
        {
            for (var z = 0; z < CoreGeneration.LENGTH * CoreGeneration.CHUNK_LENGTH; z+= gridLacunarity)
            {
                if (!(noise.GetNoise(x, z) > 0f)) continue;
                
                Instantiate(Randomisation.GetRandom(toPopulate), new Vector3(x, terrainNoiseData[x, z] * ChunkGeneration.LENGTH, z ) + Randomisation.GetRandomFlatVector3(),
                Quaternion.identity).SetActive(true);
            }
        }
    }
}