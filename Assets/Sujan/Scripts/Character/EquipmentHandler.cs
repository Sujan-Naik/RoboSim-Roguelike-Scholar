using UnityEngine;
public class EquipmentHandler : MonoBehaviour
{
    
    private GameObject _mMelee;
    public bool holdingMelee = false;
    private GameObject _mRanged;
    private GameObject _mRangedOutput;
    private GameObject _mRangedProjectile;
    private RangedTypes _mRangedType;

    public void SetMelee(GameObject newMelee)
    {
        _mMelee = newMelee;
        _mMelee.GetComponent<DamageHandler>().enabled = false;

    }

    public void SetRangedType(RangedTypes rangedType)
    {
        _mRangedType = rangedType;
    }
    
    public void SetRanged(GameObject newRanged)
    {
        _mRanged = newRanged;
    }
    
    public void SetRangedOutput(GameObject newRangedOutput)
    {
        _mRangedOutput = newRangedOutput;
    }
    
    public void SetRangedProjectile(GameObject newRangedProjectile)
    {
        _mRangedProjectile = newRangedProjectile;
    }

    public GameObject GetMelee()
    {
        return _mMelee;
    }

    public RangedTypes GetRangedType()
    {
        return _mRangedType;
    }
    
    public GameObject GetRanged()
    {
        return _mRanged;
    }
    
    public GameObject GetRangedOutput()
    {
        return _mRangedOutput;
    }
    
    public GameObject GetRangedProjectile()
    {
        return _mRangedProjectile;
    }
    
}