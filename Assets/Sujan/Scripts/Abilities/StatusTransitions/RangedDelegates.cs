using UnityEngine;
public class RangedDelegates
{

    public static void FireRanged(AbilityStatus currentStatus, AbilityStatus newStatus, AbilityEffects effect)
    {
        if (currentStatus == AbilityStatus.INACTIVE && newStatus == AbilityStatus.ACTIVE)
        {
           effect.Fire();
        }
    }

    public static void UpdateRanged(AbilityStatus status, AbilityEffects effect)
    {
        switch (status)
        {
            case AbilityStatus.INACTIVE:
            {
                break;
            }
            case AbilityStatus.ACTIVE:
            {
                effect.ProgressForward();
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