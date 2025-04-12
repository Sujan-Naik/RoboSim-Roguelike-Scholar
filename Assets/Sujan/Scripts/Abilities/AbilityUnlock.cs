using UnityEngine;

public class AbilityUnlock : MonoBehaviour
{
    
    private const float ROTATE_SPEED = 60f;
    private void Update()
    {
        transform.Rotate(0, ROTATE_SPEED * Time.deltaTime, 0);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        var ability = GenerateAbilityBasedOnType();
        var player = other.gameObject;
            
        player.GetComponent<AbilityManager>().SetAbility(ability);
        Destroy(gameObject, 0.1f);
    }

    GameObject GenerateAbilityBasedOnType()
    {
        // AbilityTypes[] types =  { AbilityTypes.AOE, AbilityTypes.RANGED, AbilityTypes.BUFF };
        AbilityTypes[] types =  { AbilityTypes.AOE, AbilityTypes.RANGED };
        AbilityTypes abilityType = types[Random.Range(0, types.Length)];
        switch (abilityType){
            case AbilityTypes.AOE:
                return AbilityFactory.Instance().InstantiateAOEAbility();
            case AbilityTypes.BUFF: 
                return AbilityFactory.Instance().InstantiateBuffAbility();
            case AbilityTypes.RANGED:
                return AbilityFactory.Instance().InstantiateRangedAbility();
        }

        return null;
    }
}
