using UnityEngine;

public class MeleeWeaponFollow : Holdable
{	
	
	public override void SetHolding(bool holding)
	{
		this._mIsHolding = holding;
		if (!holding) return;
		var equipmentHandler = _mPlayer.GetComponent<EquipmentHandler>();
		var melee = GetComponentInParent<Melee>();
			
		equipmentHandler.SetMelee(melee.kMeleeObject);
		transform.SetParent(_mHand.transform);
	}

    private void Start()
    {
	    base.Start();
	    var info = _mPlayer.GetComponent<HandInformation>();
	    _mHand = info.meleeHandObject;
		kPitch = -50f;
    }

    private void Update()
    {
	    base.Update();
	    
    }
}
