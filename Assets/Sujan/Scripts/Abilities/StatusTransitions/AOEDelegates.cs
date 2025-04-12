using UnityEngine;

public class AOEDelegates
{
    public static void FireAOE(AbilityStatus currentStatus, AbilityStatus newStatus, AbilityEffects effect)
    {
        if (currentStatus == AbilityStatus.INACTIVE && newStatus == AbilityStatus.ACTIVE)
        {
            effect.FireAOE();
        }
    }
    
    
    public static void UpdateAOE(AbilityStatus status, AbilityEffects effect)
    {
        switch (status)
        {
            case AbilityStatus.INACTIVE:
            {
                break;
            }
            case AbilityStatus.ACTIVE:
            {
                effect.DamageAroundRadius();
                break;
            }
            case AbilityStatus.DAMAGING:
            {
                break;
            }
            case AbilityStatus.FINISHED:
            {
                break;
            }
        }
    }
}