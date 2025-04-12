using UnityEngine;

public class DamageHandler : MonoBehaviour
{

	public float damage = 100f;

	private GameObject ignore;
    private void Start()
    {
        ignore = GameObject.FindWithTag("Player");
    }



	private void OnTriggerExit(Collider other)
    {
		if (other.gameObject != ignore && 
		    Mathf.Approximately(other.gameObject.layer, Mathf.Log(LayerMask.GetMask("Character"), 2))){

			Damage(other.gameObject);
		}
	}
	
	private static void Damage(GameObject target)
	{
	    target.GetComponent<HealthController>().Damage(10f);
		EffectSingleton.Instance().PlayWoundEffect(target.transform);
    }
	
}
