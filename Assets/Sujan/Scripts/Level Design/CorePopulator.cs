using UnityEngine;
using System.Collections;


public abstract class CorePopulator : MonoBehaviour
{
    protected FastNoiseLite noise;
    
    private IEnumerator Start()
    { 
        var coreGeneration = GetComponent<CoreGeneration>();
        yield return new WaitUntil(() => coreGeneration.didStart);
        GenerateNoise(0.1f);
        GenerateGameObjects(coreGeneration.GetNoiseData());
    }
    
    private void GenerateNoise(float noiseFrequency)
    { 
        noise = new FastNoiseLite();
		noise.SetSeed(Randomisation.GetSeed());
        noise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        noise.SetFrequency(noiseFrequency);
        noise.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.Manhattan);
        noise.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);
    }

    protected abstract void GenerateGameObjects(float[,] terrainNoiseData);


}
