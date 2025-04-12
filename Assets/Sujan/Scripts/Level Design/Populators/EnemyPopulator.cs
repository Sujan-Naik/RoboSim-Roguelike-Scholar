using UnityEngine;
public class EnemyPopulator : CorePopulator
{

    public int gridLacunarity;

    public GameObject[] toPopulate;
    
    protected override void GenerateGameObjects(float[,] terrainNoiseData)
    {
        for (var x = 0; x < CoreGeneration.LENGTH * CoreGeneration.CHUNK_LENGTH; x+= gridLacunarity)
        {
            for (var z = 0; z < CoreGeneration.LENGTH * CoreGeneration.CHUNK_LENGTH; z+= gridLacunarity)
            {
                if (noise.GetNoise(x, z) < 0.7f) continue;
                
                Instantiate(Randomisation.GetRandom(toPopulate), TerrainNoiseHelper.GetPosition(terrainNoiseData, x,z),
                    Quaternion.identity).SetActive(true);
            }
        }
    }
}