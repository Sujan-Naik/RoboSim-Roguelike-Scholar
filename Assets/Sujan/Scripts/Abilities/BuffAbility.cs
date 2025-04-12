using UnityEngine;

public class BuffAbility : CoreAbility
{
    public BuffAbility() :
        base(BuffDelegates.FireBuff, BuffDelegates.UpdateBuff)
    {

    }

}