using UnityEngine;
using UnityEngine.Serialization;

public class AbilityFactory : MonoBehaviour
{
    private static AbilityFactory INSTANCE;
    public GameObject _kRangedVisualsPrefab;
    private AbilityVisuals[] _mRangedVisuals;
    public GameObject _kBuffVisualsPrefab;
    private AbilityVisuals[] _mBuffVisuals;

    public GameObject aoeVisualsPrefab;
    private AbilityVisuals[] aoeVisuals;

    public GameObject rangedQualitiesPrefab;
    private AbilityQualities[] rangedQualities;


    public GameObject buffQualitiesPrefab;
    private AbilityQualities[] buffQualities;

    public GameObject aoeQualitiesPrefab;
    private AbilityQualities[] aoeQualities;

    public static AbilityFactory Instance() {
        return INSTANCE;
    }  

    private void Start()
    {
        INSTANCE = this;
        _mRangedVisuals = _kRangedVisualsPrefab.GetComponentsInChildren<AbilityVisuals>();
        rangedQualities = rangedQualitiesPrefab.GetComponentsInChildren<AbilityQualities>();
        
        _mBuffVisuals = _kBuffVisualsPrefab.GetComponentsInChildren<AbilityVisuals>();
        buffQualities = buffQualitiesPrefab.GetComponentsInChildren<AbilityQualities>();
        
        aoeVisuals = aoeVisualsPrefab.GetComponentsInChildren<AbilityVisuals>();
        aoeQualities = aoeQualitiesPrefab.GetComponentsInChildren<AbilityQualities>();

    }

    public GameObject InstantiateRangedAbility()
    {
        GameObject rangedAbility = new GameObject("rangedAbility");
        Instantiate(GetRandomVisual(AbilityTypes.RANGED), rangedAbility.transform);
        Instantiate(GetRandomQualities(AbilityTypes.RANGED), rangedAbility.transform);
        
        rangedAbility.AddComponent<RangedAbility>();
        return rangedAbility;
    }
    
    public GameObject InstantiateAOEAbility()
    {
        GameObject AOEAbility = new GameObject("AOEAbility");
        Instantiate(GetRandomVisual(AbilityTypes.AOE), AOEAbility.transform);
        Instantiate(GetRandomQualities(AbilityTypes.AOE), AOEAbility.transform);
        
        AOEAbility.AddComponent<AOEAbility>();
        return AOEAbility;
    }
    
    
    
    public GameObject InstantiateBuffAbility()
    {
        GameObject buffAbility = new GameObject("buffAbility");
        Instantiate(GetRandomVisual(AbilityTypes.BUFF), buffAbility.transform);
        Instantiate(GetRandomQualities(AbilityTypes.BUFF), buffAbility.transform);
        
        buffAbility.AddComponent<BuffAbility>();
        return buffAbility;
    }
    
    
    private Component GetRandomVisual(AbilityTypes abilityType)
    {
        switch (abilityType)
        {
            case AbilityTypes.AOE:
                return aoeVisuals[new System.Random().Next(aoeVisuals.Length)];
            case AbilityTypes.BUFF: 
                return _mBuffVisuals[new System.Random().Next(_mBuffVisuals.Length)];
            case AbilityTypes.RANGED:
                return _mRangedVisuals[new System.Random().Next(_mRangedVisuals.Length)];
        }

        return null;
    }
    
    private Component GetRandomQualities(AbilityTypes abilityType)
    {
        switch (abilityType)
        {
            case AbilityTypes.AOE:
                return aoeQualities[new System.Random().Next(aoeQualities.Length)];
            case AbilityTypes.BUFF: 
                return buffQualities[new System.Random().Next(buffQualities.Length)];
            case AbilityTypes.RANGED:
                return rangedQualities[new System.Random().Next(rangedQualities.Length)];
        }
        return null;
    }
}
    
