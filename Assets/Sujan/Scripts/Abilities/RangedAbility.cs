using UnityEngine;

public class RangedAbility : CoreAbility
{
    public RangedAbility() :
        base(RangedDelegates.FireRanged, RangedDelegates.UpdateRanged)
    {

    }


}