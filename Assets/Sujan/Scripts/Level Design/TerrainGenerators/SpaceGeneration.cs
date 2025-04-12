using UnityEngine;
using UnityEngine.Experimental.Rendering;
using System;

public class SpaceGeneration : CoreGeneration
{

    private FastNoiseLite terrainNoise;
    
    public float terrainFrequency = 0.001f;
    public float hillFrequency = 0.01f;
    private FastNoiseLite hillNoise;

    void Start()
    {
        SetTerrainNoise();
        SetHillNoise();
        base.Start();
    }

    void SetTerrainNoise()
    {
        FastNoiseLite noise = new FastNoiseLite();
        noise.SetSeed(Randomisation.GetSeed());
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        noise.SetFrequency(terrainFrequency);
        noise.SetFractalType(FastNoiseLite.FractalType.FBm);
        noise.SetFractalOctaves(3);
        noise.SetFractalLacunarity(2f);
        noise.SetFractalGain(0.5f);
		noise.SetFractalWeightedStrength(0.5f);
        this.terrainNoise = noise;
    }
    
    void SetHillNoise()
    {
        FastNoiseLite noise = new FastNoiseLite();
        noise.SetSeed(Randomisation.GetSeed());
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        noise.SetFrequency(hillFrequency);
        noise.SetFractalType(FastNoiseLite.FractalType.FBm);
        noise.SetFractalOctaves(3);
        noise.SetFractalLacunarity(3f);
        noise.SetFractalGain(0.5f);
        this.hillNoise = noise;
    }

	/*
    protected override float GetNoiseValue(int x, int y)
	{
		return terrainNoise.GetNoise(x,y);
	}
	*/
    private static readonly float TERRAIN_WEIGHT = 1f, HILL_WEIGHT = 0.1f;
    
    protected override float GetNoiseValue(int x, int y)
    {
        float value = (TERRAIN_WEIGHT * terrainNoise.GetNoise(x, y) + 
                HILL_WEIGHT * hillNoise.GetNoise(x,y)) /
               TERRAIN_WEIGHT + HILL_WEIGHT;

        return MathF.Round(value/3, 2, MidpointRounding.AwayFromZero)/8;

        // if (x > 50) return 1;
        // return 0;
    }
    
    
    
    
    
}
