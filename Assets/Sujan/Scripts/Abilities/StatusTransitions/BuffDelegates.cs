using UnityEngine;

public class BuffDelegates
{
    public static void FireBuff(AbilityStatus currentStatus, AbilityStatus newStatus, AbilityEffects effect)
    {
        if (currentStatus == AbilityStatus.INACTIVE && newStatus == AbilityStatus.ACTIVE)
        {
            effect.Fire();
        }
    }

    public static void UpdateBuff(AbilityStatus status, AbilityEffects effect)
    {
        switch (status)
        {
            case AbilityStatus.INACTIVE:
            {
                break;
            }
            case AbilityStatus.ACTIVE:
            {
                effect.ApplyBuffs();
                // Progress
                break;
            }
            case AbilityStatus.DAMAGING:
            {
                // Do the damage effects
                break;
            }
            case AbilityStatus.FINISHED:
            {
                // Do the end effects
                break;
            }
        }
    }
}
