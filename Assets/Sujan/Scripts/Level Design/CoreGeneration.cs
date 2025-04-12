using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;
using System;
using System.Collections.Generic;
using UnityEngine.TerrainUtils;

public abstract class CoreGeneration : MonoBehaviour
{
    TextureCreationFlags flags;

    public static readonly int LENGTH = 512;
    public static readonly int CHUNK_LENGTH = 4;

    public GameObject terrainObjectTemplate;

    private float[,] noiseData;

    public float[,] GetNoiseData()
    {
	    return noiseData;
    }

    private static readonly ValueTuple<int, int> LEFT = new (-1, 0);
    private static readonly ValueTuple<int, int> TOP = new (0, 1);
    private static readonly ValueTuple<int, int> RIGHT = new (1, 0);
    private static readonly ValueTuple<int, int> BOTTOM = new (0, -1);
    protected void Start()
    {
		GenerateNoiseData();
		
		var chunks = new Dictionary<ValueTuple<int, int>, ChunkGeneration>();
		GenerateChunks(chunks);
		GenerateNavigationMesh(chunks);
    }

    private void GenerateChunks(Dictionary<ValueTuple<int, int>, ChunkGeneration> gridChunkDictionary)
    {
	    for (var chunkX = 0; chunkX < CHUNK_LENGTH; chunkX+=1)
	    {
		    for (var chunkZ = 0; chunkZ < CHUNK_LENGTH; chunkZ+=1 )
		    {
			    var chunkNoiseData = new float[ChunkGeneration.LENGTH, ChunkGeneration.LENGTH];
			    var xCoordinate = chunkX * LENGTH;
			    var zCoordinate = chunkZ * LENGTH;

			    for (var x = 0; x < ChunkGeneration.LENGTH ; x++)
			    {
				    for (var z = 0; z < ChunkGeneration.LENGTH ; z++)
				    {
					    chunkNoiseData[z, x] = noiseData[xCoordinate + x, zCoordinate + z];
				    }
			    }
			    gridChunkDictionary[new ValueTuple<int,int>(chunkX, chunkZ)] = new ChunkGeneration(new Vector3(xCoordinate, 0, zCoordinate), this, chunkNoiseData);
		    }
	    }
    }

    private void GenerateNavigationMesh(Dictionary<ValueTuple<int, int>, ChunkGeneration> chunks)
    {
	    List<NavMeshBuildSource> navMeshBuildSources = new List<NavMeshBuildSource>();

	    
	    foreach (var entry in chunks)
	    {
		    Terrain leftTerrain = null, topTerrain = null, rightTerrain = null, bottomTerrain = null;

		    if (chunks.ContainsKey( AddTuples(entry.Key, LEFT) ))
		    {
			    leftTerrain = chunks[ AddTuples(entry.Key, LEFT) ].GetTerrain();
		    } else if (chunks.ContainsKey( AddTuples(entry.Key, TOP) ))
		    {
			    topTerrain = chunks[ AddTuples(entry.Key, TOP) ].GetTerrain();
		    } else if (chunks.ContainsKey( AddTuples(entry.Key, RIGHT) ))
		    {
			    rightTerrain = chunks[ AddTuples(entry.Key, RIGHT) ].GetTerrain();
		    } else if (chunks.ContainsKey( AddTuples(entry.Key, BOTTOM) ))
		    {
			    bottomTerrain = chunks[ AddTuples(entry.Key, BOTTOM) ].GetTerrain();
		    }
		    entry.Value.GetTerrain().SetNeighbors(leftTerrain, topTerrain, rightTerrain, bottomTerrain);
		    entry.Value.GetTerrain().allowAutoConnect = true;
		    entry.Value.GetTerrain().drawInstanced = true;
		    navMeshBuildSources.Add(entry.Value.TerrainSource());
	    }
        
	    Terrain.SetConnectivityDirty();
	    TerrainUtility.AutoConnect();
	    terrainObjectTemplate.gameObject.SetActive(false);
	    AddNavMesh(navMeshBuildSources);
    }
    
    private void AddNavMesh(List<NavMeshBuildSource> navMeshBuildSources)
    {
	    var buildSettings = NavMesh.CreateSettings();
	    buildSettings.agentTypeID = 0;
	    var localBounds = new Bounds(new Vector3(LENGTH/2, LENGTH/2, LENGTH/2) * CHUNK_LENGTH, 
		    new Vector3(LENGTH,LENGTH, LENGTH) * CHUNK_LENGTH);
	    var rotation = Quaternion.identity;
	    buildSettings.maxJumpAcrossDistance = 1;
	    buildSettings.ledgeDropHeight = 1;
		 
	    var navMeshData = NavMeshBuilder.BuildNavMeshData(buildSettings, 
		    navMeshBuildSources, localBounds, Vector3.zero, rotation); 
		
	    NavMesh.AddNavMeshData(navMeshData);
    }
    

    private ValueTuple<int, int> AddTuples(ValueTuple<int, int> tuple1, ValueTuple<int, int> tuple2)
    {
	    return new ValueTuple<int,int>(tuple1.Item1 + tuple2.Item1, tuple1.Item2 + tuple2.Item2);
    }

	private void GenerateNoiseData()
    {
        noiseData = new float[LENGTH * CHUNK_LENGTH + 1, LENGTH * CHUNK_LENGTH + 1];

        for (var x = 0; x < LENGTH * CHUNK_LENGTH + 1; x++)
        {
            for (var z = 0; z < LENGTH * CHUNK_LENGTH + 1; z++)
            {
	            noiseData[x, z] = GetAdjustedNoiseValue(x, z);
            }
		}
	}

	protected float GetAdjustedNoiseValue(int x, int z)
	{
		return (GetNoiseValue(x, z) + 1) /2;
	}

	protected abstract float GetNoiseValue(int x, int z);

}
