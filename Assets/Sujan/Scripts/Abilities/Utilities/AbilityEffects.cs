using UnityEngine;

public class AbilityEffects
{
    
    private float _mStartTime;
    
    private readonly CoreAbility _mCoreAbility;

    private readonly Transform _mCameraTransform;

    private readonly  AbilityVisuals _mAbilityVisuals;
    
    private readonly AbilityQualities _mAbilityQualities;
	
	private Vector3 _kForward;
	private Vector3 _kOrigin;
	
    
	public AbilityEffects(Transform mCameraTransform, CoreAbility mCoreAbility, AbilityVisuals mAbilityVisuals,
			AbilityQualities mAbilityQualities){

		_mCameraTransform = mCameraTransform;

        _mCoreAbility = mCoreAbility;
	    _mAbilityVisuals = mAbilityVisuals;
        _mAbilityQualities = mAbilityQualities;
	}
    
    public void Fire()
    {
		_kForward = _mCameraTransform.forward;
       	_mAbilityVisuals.PlayStartParticles();
		_kOrigin = _mCoreAbility.transform.position;
    }
    
    public void FireAOE()
    {
		_kForward = _mCameraTransform.forward;
       	
		RaycastHit hitInfo;
		if (Raycast.NonCharacterRaycast(_mCoreAbility.transform.position + _kForward/2, _kForward, out hitInfo,
			    _mAbilityQualities.range))
		{
			_mCoreAbility.transform.position = hitInfo.point;
		}
		
		_mAbilityVisuals.PlayStartParticles();
		_kOrigin = _mCoreAbility.transform.position;

    }
    
	private void Remove()
	{
		_mCoreAbility.Remove();
		_mAbilityVisuals.Remove();
	
	}
	
    public void ProgressForward()
    {
	    var distance = Time.deltaTime * _mAbilityQualities.speed;
	    
	    DamageNonPlayerCharactersForward(distance);
	    
        _mCoreAbility.transform.position += _kForward * distance;
        
		CheckEnvironmentCollisions();
		
		RangeCheck();


    }

    private void DamageNonPlayerCharactersForward(float distance)
    {
	    
	    if (!Raycast.CharacterRaycast(_mCoreAbility.transform.position, _mAbilityQualities.hitbox, _kForward,
		        out var hitInfo, distance)) return;
	    
	    GameObject potential = hitInfo.collider.gameObject;
	    if (potential.gameObject.CompareTag("Player")) return;
	    
	    
	    Damage(potential);

    }

    private void RangeCheck()
    {
	    if (Vector3.Distance(_kOrigin, _mCoreAbility.transform.position) > _mAbilityQualities.range)
	    {
		    Remove();
	    }
    }

    public void DamageAroundRadius()
    {
	    _kForward = _mCameraTransform.forward;
	    DamageNonPlayerCharactersAroundPoint();
	    CheckEnvironmentCollisions();
    }

    private void DamageNonPlayerCharactersAroundPoint()
    {
	    foreach (var collider in Raycast.CharacterOverlap(_mCoreAbility.transform.position, _mAbilityQualities.hitbox) )
	    {
		    if (collider.gameObject.CompareTag("Player")) continue;
		    
		    Damage(collider.gameObject);
		    return;
	    }
    }
    
    
    public void ApplyBuffs()
    {
	    _mAbilityVisuals.PlayStartParticles();
	    GameObject.FindWithTag("Player");
    }
    
    
    public void ApplyMovement()
    {
	    
    }

    private void CheckEnvironmentCollisions()
    {
	    if (Raycast.EnvironmentOverlap(_mCoreAbility.transform.position, 0.01f).Length > 0)
	    {
		    RemoveHitSolid(_mCoreAbility.transform.position);
	    }
    }

    private void RemoveHitSolid(Vector3 position)
    {
	    _mAbilityVisuals.PlayEnvironmentParticles(position);
		Debug.Log(Time.time);
	    Remove();
    }

    private void Damage(GameObject target)
    {
	    _mAbilityVisuals.PlayEnemyParticles(target.transform.position);
	    target.GetComponent<HealthController>().Damage(_mAbilityQualities.damage);
    }
	
	
}
