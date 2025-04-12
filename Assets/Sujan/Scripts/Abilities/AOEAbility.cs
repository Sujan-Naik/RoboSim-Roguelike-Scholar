using UnityEngine;

public class AOEAbility : CoreAbility
{
    public AOEAbility() :
        base(AOEDelegates.FireAOE, AOEDelegates.UpdateAOE)
    {

    }

}