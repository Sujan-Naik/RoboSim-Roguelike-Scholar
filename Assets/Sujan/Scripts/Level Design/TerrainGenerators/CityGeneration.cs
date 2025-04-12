using UnityEngine;
using UnityEngine.Experimental.Rendering;
using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;

public class CityGeneration : CoreGeneration
{

    private FastNoiseLite roadNoise;

    private int GRID = 40;
    private readonly float PAVEMENT_LEVEL = 0.001f;
    
    public GameObject[] oneSidedBuildings;
    public GameObject[] twoSidedBuildings;

    public GameObject straightRoadPrefab;
    public GameObject closedRoadPrefab;
    public GameObject turnPrefab; //minus x to z
    public GameObject tJunctionPrefab;  // comes in from -x
    public GameObject crossroadsPrefab;
    public GameObject pavementPrefab;
    public GameObject lampPrefab;
    public GameObject[] lootablesPrefab;
    public GameObject[] naturePrefabs;
    public GameObject[] vehiclePrefabs;
    
    public float terrainFrequency = 0.001f;


    private System.Random random; 
    
    void Start()
    {
        GRID = (int) straightRoadPrefab.GetComponent<MeshRenderer>().bounds.size.z;

        base.Start();
        
        SetRoadNoise();
        CreateRoads();
        CreateBuildings();

    }
    
    List<ValueTuple<int, int>> roadCoordsList = new List<ValueTuple<int, int>>();
    
    
    private void CreateBuildings()
    {
        for (var x = 0; x < LENGTH * CHUNK_LENGTH; x += GRID)
        {
            for (var z = 0; z < LENGTH * CHUNK_LENGTH; z += GRID)
            {
                if (!roadCoordsList.Contains(new ValueTuple<int, int>(x, z)))
                {
                    var neighbourList = GetNeighbourList(x, z);
                    if (neighbourList.Count > 0)
                    {
                        Instantiate(pavementPrefab, new Vector3(x, GetFloorHeight(x, z), z), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(Randomisation.GetRandom(naturePrefabs), new Vector3(x, GetFloorHeight(x, z), z), Quaternion.identity);
                    }
                    switch (neighbourList.Count)
                    {
                        case 1:
                            Quaternion quaternion = Quaternion.identity;
                            
                            if (neighbourList[0] == U)
                            {
                                quaternion = Quaternion.identity;
                                Instantiate(lampPrefab, new Vector3(x , GetFloorHeight(x, z), z + GRID/2), 
                                    Quaternion.Euler(0,-90,0));
                            }
                            else if (neighbourList[0] == L)
                            {
                                quaternion = Quaternion.Euler(0, -90, 0);
                                Instantiate(lampPrefab, new Vector3(x - GRID/2, GetFloorHeight(x, z), z), 
                                    Quaternion.Euler(0,-180,0));
                    
                            }
                            else if (neighbourList[0] == D)
                            {
                                quaternion = Quaternion.Euler(0, -180, 0);
                                Instantiate(lampPrefab, new Vector3(x , GetFloorHeight(x, z), z - GRID/2), 
                                    Quaternion.Euler(0,-270,0));
                            }
                            else if (neighbourList[0] == R)
                            {
                                quaternion = Quaternion.Euler(0, -270, 0);
                                Instantiate(lampPrefab, new Vector3(x + GRID/2 , GetFloorHeight(x, z), z), 
                                    Quaternion.identity);
                            }
                    
                            Instantiate(Randomisation.GetRandom(oneSidedBuildings), new Vector3(x, GetFloorHeight(x, z), z), quaternion);
                            break;
                    
                        case 2:
                    
                            
                            GameObject twoSidedBuilding =
                                Instantiate(Randomisation.GetRandom(twoSidedBuildings),
                                    new Vector3(x, GetFloorHeight(x, z), z), Quaternion.identity);
                    
                           
                            if (!neighbourList.Contains(R))
                            {
                                if (!neighbourList.Contains(D))
                                {
                                    twoSidedBuilding.transform.rotation = Quaternion.Euler(0, 270f, 0);
                                    Instantiate(Randomisation.GetRandom(lootablesPrefab),
                                    new Vector3(x - GRID/2 - 1, GetFloorHeight(x, z) + 0.3f, z + GRID/2 + 1),Quaternion.Euler(0, 270f, 0));
                    
                                }
                                else if (!neighbourList.Contains(U))
                                {
                                   
                                    twoSidedBuilding.transform.rotation = Quaternion.Euler(0, 180f, 0);
                                    Instantiate(Randomisation.GetRandom(lootablesPrefab),
                                        new Vector3(x - GRID/2 - 1, GetFloorHeight(x, z) + 0.3f, z - GRID/2 - 1),Quaternion.Euler(0, 180f, 0));                                    
                                }
                            }
                            else if (!neighbourList.Contains(L))
                            {
                                if (!neighbourList.Contains(D))
                                {
                                    Instantiate(Randomisation.GetRandom(lootablesPrefab),
                                        new Vector3(x + GRID/2 + 1, GetFloorHeight(x, z) + 0.3f, z + GRID/2 + 1),Quaternion.Euler(0, 0, 0));
                    
                                }
                                else if (!neighbourList.Contains(U))
                                {
                                    
                                    twoSidedBuilding.transform.rotation = Quaternion.Euler(0, 90f, 0);
                                    Instantiate(Randomisation.GetRandom(lootablesPrefab),
                                        new Vector3(x + GRID/2 + 1, GetFloorHeight(x, z) + 0.3f, z - GRID/2 - 1),Quaternion.Euler(0, 90f, 0));
                                   
                                }
                            }
                    
                            break;
                    }
                }
            }
        }
    }

    private void CreateRoads()
    {
        for (var x = 0; x < LENGTH * CHUNK_LENGTH; x+=GRID)
        {
            for (var z = 0; z < LENGTH * CHUNK_LENGTH; z+=GRID)
            {
                GridCheck(x, z);
            }
        }
        ComputeRoads();
    }

    private void GridCheck(int x, int z)
    {
        for (var iX = 0; iX < GRID; iX += 1)
        {
            for (var iY = 0; iY < GRID; iY += 1)
            {
                var value = roadNoise.GetNoise(x + iX, z + iY);

                if (!(value > 0.985f)) continue;
                
                roadCoordsList.Add(new ValueTuple<int, int>(x, z)); // creates a list of points where roads are
                return;
            }
        }
    }
    
    
    void ComputeRoads()
    {
        foreach (var roadCoord in roadCoordsList)
        {
           DesignateRoadObject(roadCoord.Item1, roadCoord.Item2);
        }
    }
    
    
    private static readonly ValueTuple<int, int> U = new (0, 1), D = new (0,-1), L = new (-1, 0), R = new (1, 0);
    
    private static readonly ValueTuple<int, int>[] ADJACENT_ARRAY = {U, D, L, R};

    private List<ValueTuple<int, int>> GetNeighbourList(int x, int z)
    {
        var neighbourList = new List<ValueTuple<int, int>>();
        foreach (var neighbour in ADJACENT_ARRAY)
        {
            var scaledNeighbour = new ValueTuple<int, int>(x + neighbour.Item1 * GRID,  z+ neighbour.Item2 * GRID);
            if (roadCoordsList.Contains(scaledNeighbour))
            {
                neighbourList.Add(neighbour);
            }
        }

        return neighbourList;
    }
    private void DesignateRoadObject(int x, int z)
    {

        var neighbourList = GetNeighbourList(x, z);

        switch (neighbourList.Count)
        {
            case 1:
                var obj = Instantiate(closedRoadPrefab, new Vector3(x, GetFloorHeight(x, z), z), Quaternion.identity);
                switch (neighbourList[0].Item1)
                {
                    case -1:
                        obj.transform.rotation = Quaternion.Euler(0, 90f, 0);
                        break;
                    case 1:
                        obj.transform.rotation = Quaternion.Euler(0, -90f, 0);
                        break;
                    case 0 when neighbourList[0].Item2 == -1:
                        obj.transform.rotation = Quaternion.Euler(0, 0f, 0);
                        break;
                    case 0:
                    {
                        if (neighbourList[0].Item2 == 1)
                        {
                            obj.transform.rotation = Quaternion.Euler(0, 180f, 0);
                        }

                        break;
                    }
                }
                break;
            case 2:
                if (neighbourList[0].Item1 == neighbourList[1].Item1) // X is the same
                {
                    Instantiate(straightRoadPrefab, new Vector3(x, GetFloorHeight(x, z), z), Quaternion.identity);
                    Instantiate(vehiclePrefabs[new System.Random().Next(vehiclePrefabs.Length)], new Vector3(x, GetFloorHeight(x, z), z), Quaternion.identity);
                } else if (neighbourList[0].Item2 == neighbourList[1].Item2)
                {
                    Instantiate(straightRoadPrefab, new Vector3(x, GetFloorHeight(x, z), z),Quaternion.Euler(0, 90f, 0));
                }
                else
                {

                    GameObject turn = Instantiate(turnPrefab, new Vector3(x, GetFloorHeight(x, z), z), Quaternion.identity);
                    if (!neighbourList.Contains(R))
                    {
                        if (!neighbourList.Contains(D))
                        {

                        }
                        else if (!neighbourList.Contains(U))
                        {
                            turn.transform.rotation = Quaternion.Euler(0, 270f, 0);

                        }
                    }
                    else if (!neighbourList.Contains(L))
                    {
                        if (!neighbourList.Contains(D))
                        {
                            turn.transform.rotation = Quaternion.Euler(0, -270f, 0);
                        }
                        else if (!neighbourList.Contains(U))
                        {
                            turn.transform.rotation = Quaternion.Euler(0, -180f, 0);

                        }
                    }
                        
                    
                }
                break;
            case 3:
                GameObject tJunctionObject = Instantiate(tJunctionPrefab, new Vector3(x, GetFloorHeight(x, z), z), Quaternion.identity);
                if (!neighbourList.Contains(D))
                {
                    tJunctionObject.transform.rotation = Quaternion.Euler(0, 0f, 0);
                } else if (!neighbourList.Contains(R))
                {
                    tJunctionObject.transform.rotation = Quaternion.Euler(0, 270f, 0);

                } else if (!neighbourList.Contains(U))
                {
                    tJunctionObject.transform.rotation = Quaternion.Euler(0, 180f, 0);
                }
                else
                {
                    tJunctionObject.transform.rotation = Quaternion.Euler(0, 90f, 0);
                }
                break;
            case 4: 
                Instantiate(crossroadsPrefab, new Vector3(x, GetFloorHeight(x, z), z), Quaternion.identity);
                break;
                
        }
    }


    float GetFloorHeight(int x, int z)
    {
        return GetAdjustedNoiseValue(x, z) *LENGTH + 0.7f;
    }
    
    private void SetRoadNoise()
    {
        FastNoiseLite noise = new FastNoiseLite();
        noise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        noise.SetFrequency(0.005f);
        noise.SetFractalType(FastNoiseLite.FractalType.Ridged);
        noise.SetFractalOctaves(1);
        noise.SetFractalLacunarity(0f);
        noise.SetFractalGain(0.5f);
        noise.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.Manhattan);
        noise.SetCellularReturnType(FastNoiseLite.CellularReturnType.Distance2Div);
        noise.SetCellularJitter(0.5f);
        roadNoise = noise;
    }
    
    
    protected override float GetNoiseValue(int x, int z)
    {
        return 0;
    }
    
    
    
    
    
}
