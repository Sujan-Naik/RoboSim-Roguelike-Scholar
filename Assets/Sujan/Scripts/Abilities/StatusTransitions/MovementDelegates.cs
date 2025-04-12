using UnityEngine;

public class MovementDelegates
{
    public static void FireMovement(AbilityStatus currentStatus, AbilityStatus newStatus, AbilityEffects effect)
    {
        if (currentStatus == AbilityStatus.INACTIVE && newStatus == AbilityStatus.ACTIVE)
        {
            effect.Fire();
        }
    }

    public static void UpdateMovement(AbilityStatus status, AbilityEffects effect)
    {
        switch (status)
        {
            case AbilityStatus.INACTIVE:
            {
                break;
            }
            case AbilityStatus.ACTIVE:
            {
                effect.ApplyMovement();
                
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