using UnityEngine;
using UnityEngine.Serialization;

public class EffectSingleton : MonoBehaviour
{
    
    private static EffectSingleton instance;
    

    public GameObject kGunshotEffect;
    public GameObject kWoundEffect;
    public GameObject kSucceedEffect;
    public GameObject kFailEffect;
    public GameObject kRangedEffect;
    public GameObject kMeleeEffect;
    public GameObject kAbilityEffect;


    public static EffectSingleton Instance()
    {
        return instance;
    }

    private void Start()
    {
        instance = this;
    }

    public void PlayGunshotEffect(Transform transform)
    {
        Instantiate(kGunshotEffect, transform.position, transform.rotation);
    }
    
    public void PlayWoundEffect(Transform effectTransform)
    {
        Instantiate(kWoundEffect, effectTransform.position, effectTransform.rotation);
    }
    
    public void PlaySucceedEffect(Transform effectTransform)
    {
        Instantiate(kSucceedEffect, effectTransform.position, effectTransform.rotation);
    }
    
    public void PlayFailEffect(Transform effectTransform)
    {
        Instantiate(kFailEffect, effectTransform.position, effectTransform.rotation);
    }
    
    public void PlayRangedEffect(Transform effectTransform)
    {
        Instantiate(kRangedEffect, effectTransform.position, effectTransform.rotation);
    }
    
    public void PlayMeleeEffect(Transform effectTransform)
    {
        Instantiate(kMeleeEffect, effectTransform.position, effectTransform.rotation);
    }
    
    public void PlayAbilityEffect(Transform effectTransform)
    {
        Instantiate(kAbilityEffect, effectTransform.position, effectTransform.rotation);
    }
}