using UnityEngine;


public abstract class CoreAbility : MonoBehaviour
{
	
    
    private AbilityStatus _mStatus = AbilityStatus.INACTIVE;
    
    private AbilityQualities _mAbilityQualities;
    private AbilityEffects _mAbilityEffects;
    

    protected delegate void StatusTransitionDelegate(AbilityStatus currentStatus, AbilityStatus newStatus, AbilityEffects abilityEffects);
    protected delegate void StateUpdateDelegate(AbilityStatus currentStatus, AbilityEffects abilityEffects);
    
    private StatusTransitionDelegate _mTransitionDelegate;
    private StateUpdateDelegate _mUpdateDelegate;

    private float time;
    
    protected CoreAbility(StatusTransitionDelegate statusMTransitionDelegate, StateUpdateDelegate stateMUpdateDelegate)
    {
        this._mTransitionDelegate = statusMTransitionDelegate;
        this._mUpdateDelegate = stateMUpdateDelegate;
    }

	private void Start()
	{
		_mAbilityQualities = GetComponentInChildren<AbilityQualities>();

		_mAbilityEffects = new AbilityEffects(Camera.main.transform, this, GetComponentInChildren<AbilityVisuals>(), _mAbilityQualities);
		time = Time.time;
	}
	
    public void SetAbilityStatus(AbilityStatus status)
    {
        _mTransitionDelegate(_mStatus, status, _mAbilityEffects);
        
        _mStatus = status;
    }
	
	public AbilityStatus GetAbilityStatus()
	{
		return _mStatus;
	}
	

    public void AbilityUpdate()
    {
	    if (Time.time - _mAbilityQualities.duration > time)
	    {
		    SetAbilityStatus(AbilityStatus.FINISHED);
	    }
        _mUpdateDelegate(_mStatus, _mAbilityEffects); 
    }

	public void Remove()
	{
		SetAbilityStatus(AbilityStatus.FINISHED);
	}

}
