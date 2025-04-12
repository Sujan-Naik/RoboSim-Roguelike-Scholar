using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class Randomisation
{
    public static Random random = new Random();

    private const int SEED_UPPER_BOUND = 10000;

    public static T GetRandom<T>(T[] array)
    {
        return array[random.Next(array.Length)];
    }
    
    
    public static T GetRandom<T>(List<T> list)
    {
        return list[random.Next(list.Count)];
    }

    public static int GetInt(int maxExclusive)
    {
        return random.Next(maxExclusive);
    }

    public static int GetSeed()
    {
        return GetInt(SEED_UPPER_BOUND);
    }

    
    public static Vector3 GetRandomFlatVector3()
    {
        var vec = UnityEngine.Random.insideUnitSphere;
        vec.Scale(new Vector3(1f, 0, 1f));
        return vec;
    }
}