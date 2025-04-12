using UnityEngine;
using System;
using System.Collections.Generic;

public class EquipmentGenerator : MonoBehaviour
{
    public static EquipmentGenerator instance;
    public GameObject[] meleeArray;
    public GameObject[] rangedArray;
    
    private void Start()
    {
        instance = this;
    }

    public void GenerateMelee(Vector3 position)
    {
        var toGenerateMelee = Instantiate(Randomisation.GetRandom(meleeArray), position, Quaternion.identity);
        toGenerateMelee.SetActive(true);
    }

  	public void GenerateRanged(Vector3 position)
    {
        var toGenerateRanged = Instantiate(Randomisation.GetRandom(rangedArray), position, Quaternion.identity);
        toGenerateRanged.SetActive(true);
    }
}