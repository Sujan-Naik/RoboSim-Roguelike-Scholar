public class RangedWeaponFollow : Holdable
{
	public RangedTypes rangedType;

	public override void SetHolding(bool holding)
	{
		
		_mIsHolding = holding;
		if (!holding) return;
		
		var equipmentHandler = _mPlayer.GetComponent<EquipmentHandler>();
		var ranged = GetComponent<Ranged>();

		equipmentHandler.SetRanged(ranged.kRangedObject);
		equipmentHandler.SetRangedType(rangedType);
		equipmentHandler.SetRangedOutput( ranged.kRangedOutputObject);
		equipmentHandler.SetRangedProjectile( ranged.kRangedProjectileObject);


	}

    private void Start()
    {
	    base.Start();
	    var info = _mPlayer.GetComponent<HandInformation>();
	    this._mHand = info.rangedHandObject;
    }

    private void Update()
    {
	    base.Update();
	    
    }
}
