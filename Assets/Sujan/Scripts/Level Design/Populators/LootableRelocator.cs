public class LootableRelocator : ManualRelocatorPopulator
{
    
    protected override void GenerateGameObjects(float[,] terrainNoiseData)
    {
        base.GenerateGameObjects(terrainNoiseData);
        ToRelocate.AddComponent(typeof(EquipmentDropper));
        SpawnObject();
    }
}