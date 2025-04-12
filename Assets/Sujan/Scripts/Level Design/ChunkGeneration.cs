using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;

public class ChunkGeneration
{
    TextureCreationFlags flags;

    public static readonly int LENGTH = 513;

    private GameObject terrainObject;
    private Terrain terrain;

    public Terrain GetTerrain()
    {
	    return terrain;
    }

	private readonly Vector3 terrainPos;

	private readonly CoreGeneration coreGeneration;

	private float[,] noiseData;

	private GameObject terrainObjectTemplate;
	

	public ChunkGeneration(Vector3 terrainPos, CoreGeneration coreGeneration, float[,] noiseData)
	{
		this.terrainPos = terrainPos;
		this.coreGeneration = coreGeneration;
		this.noiseData = noiseData;
	
		this.terrainObjectTemplate = coreGeneration.terrainObjectTemplate;
		CreateTerrain();
		AttachTerrainData();
		GameObject.Destroy(terrain, 60000f);
	}
	
    
    private void CreateTerrain()
    {
	    terrainObject = GameObject.Instantiate(terrainObjectTemplate, terrainPos, Quaternion.identity);
	    terrain = terrainObject.GetComponent<Terrain>();
    }

    public NavMeshBuildSource TerrainSource()
    {
        var src = new NavMeshBuildSource();
        src.transform = terrainObject.transform.localToWorldMatrix;
        src.shape = NavMeshBuildSourceShape.Terrain;
        src.size = new Vector3(LENGTH + 1, LENGTH, LENGTH + 1);
		src.sourceObject = terrain.terrainData;
		src.generateLinks = true;
		
        return src;
    }
    
      void AttachTerrainData()
    {
        TerrainData newTerrainData = GameObject.Instantiate(terrain.terrainData);
        
        newTerrainData.alphamapResolution = LENGTH;
        newTerrainData.baseMapResolution = LENGTH * 2;
        
        newTerrainData.SetDetailResolution(LENGTH, 16);

        newTerrainData.heightmapResolution = LENGTH;
        newTerrainData.size = new Vector3(LENGTH,LENGTH,LENGTH);

        newTerrainData.wavingGrassAmount = 10000f;
        
        newTerrainData.SetHeights(0, 0, noiseData);
        
        float[,,] map = new float[LENGTH, LENGTH, 1];
        
        for (int y = 0; y < LENGTH; y++)
        {
            for (int x = 0; x < LENGTH; x++)
            {
				map[x, y, 0] = 1f;
            }
        }
        newTerrainData.SetAlphamaps(0, 0, map);
        
        terrain.terrainData = newTerrainData;
        
        terrainObject.GetComponent<TerrainCollider>().terrainData = newTerrainData;
        
    }
    
    
    
    
}
