using UnityEngine;
using System;
using System.Collections;
public class EquipmentDropper : MonoBehaviour
{

    private Vector3 GetRandom()
    {
        Vector3 vec = UnityEngine.Random.onUnitSphere;
        vec.Scale(new Vector3(1 * transform.lossyScale.x, 0, 1 * transform.lossyScale.z));
        return vec;
    }
    IEnumerator Start()
    {
        yield return new WaitUntil(() => EquipmentGenerator.instance != null);
        DropMelee();
        DropRanged();
    }

     void DropMelee()
    {
        EquipmentGenerator.instance.GenerateMelee( GetRandom() + transform.position + Vector3.up/2);
    }
    
    
     void DropRanged()
    {
        EquipmentGenerator.instance.GenerateRanged(GetRandom() + transform.position + Vector3.up/2 );
    }
}