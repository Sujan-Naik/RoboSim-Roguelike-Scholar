using UnityEngine;
using UnityEngine.Experimental.Rendering;
using System;

public class DesertGeneration : CoreGeneration
{

    private FastNoiseLite terrainNoise;
    
    public float terrainFrequency = 0.005f;
    public float hillFrequency = 0.01f;
    private FastNoiseLite hillNoise;

    void Start()
    {
        SetTerrainNoise();
        SetHillNoise();
        base.Start();
    }

    /**
     * The main terrain noise to ensure flatness
     */
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
    
    private const float TERRAIN_WEIGHT = 1f, HILL_WEIGHT = 0.01f;
    
    protected override float GetNoiseValue(int x, int y)
    {
        var value = (TERRAIN_WEIGHT * terrainNoise.GetNoise(x, y) + 
                HILL_WEIGHT * hillNoise.GetNoise(x,y)) /
               TERRAIN_WEIGHT + HILL_WEIGHT;

        return MathF.Round(value/10, 2, MidpointRounding.AwayFromZero)/8;
    }
    
    
    
    
    
}
