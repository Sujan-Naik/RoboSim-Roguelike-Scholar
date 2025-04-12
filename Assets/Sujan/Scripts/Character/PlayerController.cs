using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    
    // Constants for movement
    public float kMaximumSpeed = 0.3f; // Meters per second
    public float kForwardAcceleration = 0.2f; // Meters per second
    public float kGravity = 0.01f; // Meters per second
    public float kTerminalVelocity = 0.1f; // Meters per second

    private Vector3 _mPreviousVelocity;
    
    // Constants for look changes
	public float kRotationSpeed = 60f;
	public float kRotationThreshold = 0.01f;

    public float kMaximumPitchDeviation = 40f; // How much the player can look up/down from resting position
    public float kMaxPitchTurnSpeed = 2.0f; // The fastest a player head pitch can change
    public float kPitchTurnAcceleration = 0.2f; // How fast the player can look up/down 

    private float _mPitchSpeed = 0.0f;
    public float kPitchSpeedFriction = 0.1f;
	
	private float _mOldPitch;
    
    
    public float kMaximumYawDeviation = 10f; // How much the player can look right/left from resting position
    public float kMaxYawTurnSpeed = 2.0f; // The fastest a player head yaw can change
    public float kYawTurnAcceleration = 0.5f; // How much the player can look right/left from resting position
    
	private float _mOldYaw;

    private float _mYawSpeed;
    
    private CharacterController _mCharacterController;
    private GameObject _mCamera;
    
    private Animator _mAnimator;
    
    private InputAction _mMoveAction, _mLookAction;

    private CameraStates _mCameraState = CameraStates.DEFAULT;
	
	
	private ControllerManager _mControllerManager;
	

    void Start()
    {
	    _mControllerManager = GetComponent<ControllerManager>();

		_mCamera = Camera.main.gameObject;
		
		_mCamera.transform.position = transform.position;
        _mCharacterController = GetComponent<CharacterController>();
        _mAnimator = GetComponent<Animator>();

        _mMoveAction = InputSystem.actions.FindAction("Move");
        _mLookAction = InputSystem.actions.FindAction("Look");


        _mPitchSpeed = 0;
        _mYawSpeed = 0;
        
        _mPreviousVelocity = new Vector3(0, 0, 0);
		_mOldPitch = transform.eulerAngles.x;
		_mOldYaw = transform.eulerAngles.y;
    }
    
	private void HandleRotation()
	{
		Vector2 lookValue = _mLookAction.ReadValue<Vector2>();

        float yawRight = lookValue.x;
		if (Mathf.Abs(yawRight) < kRotationThreshold)
		{
			_mYawSpeed = 0;
		}
        _mYawSpeed += yawRight * kYawTurnAcceleration * Time.fixedDeltaTime;
		if (_mYawSpeed > kMaxYawTurnSpeed)
		{
			_mYawSpeed = kMaxYawTurnSpeed;
		}
		if (_mYawSpeed < -kMaxYawTurnSpeed)
		{
			_mYawSpeed = -kMaxYawTurnSpeed;
		}
		

        float pitchUp = lookValue.y;
		if (Mathf.Abs(pitchUp) < kRotationThreshold)
		{
			_mPitchSpeed = 0;
		}
        _mPitchSpeed += pitchUp * kPitchTurnAcceleration * Time.fixedDeltaTime;
		if (_mPitchSpeed > kMaxPitchTurnSpeed)
		{
			_mPitchSpeed = kMaxPitchTurnSpeed;
		}
		if (_mPitchSpeed < -kMaxPitchTurnSpeed)
		{
			_mPitchSpeed = -kMaxPitchTurnSpeed;
		}

		var newPitch = _mOldPitch - _mPitchSpeed;
		if (newPitch > 180)
		{
			newPitch -= 360;
		}

		if (newPitch > kMaximumPitchDeviation)
		{
			newPitch = kMaximumPitchDeviation;
		}
		if (newPitch < -kMaximumPitchDeviation)
		{
			newPitch = -kMaximumPitchDeviation;
		}
		
		var newYaw =  _mOldYaw + _mYawSpeed;
		var newYawRelativeToPlayer = newYaw - transform.eulerAngles.y;

		
		if (newYawRelativeToPlayer > kMaximumYawDeviation )
		{

			transform.Rotate(0,kRotationSpeed * Time.fixedDeltaTime, 0);
			newYaw = kMaximumYawDeviation + transform.eulerAngles.y;
		}
		if (newYawRelativeToPlayer < -kMaximumYawDeviation)
		{
			transform.Rotate(0,- kRotationSpeed * Time.fixedDeltaTime, 0);

			newYaw = -kMaximumYawDeviation + transform.eulerAngles.y;
		}

		TransformCamera( newPitch,  newYaw);
		_mOldPitch = newPitch;
		_mOldYaw = newYaw;
	}

	private void TransformCamera(float newPitch, float newYaw)
	{
		_mCamera.transform.rotation = Quaternion.Euler(newPitch, newYaw, 0);
		var potentialCameraPos = _mCameraState.GetTransform(transform);
		var diff = potentialCameraPos - transform.position;
		var heightDiff = new Vector3(0,diff.y,0);
		diff.Scale(new Vector3(1, 0, 1));
		
		var distance = diff.magnitude;
		
		RaycastHit hitInfo;
		var mask = ~LayerMask.GetMask("Character");
		if (Physics.Raycast(transform.position + heightDiff, diff, out hitInfo, distance, mask, QueryTriggerInteraction.Collide))
		{
			_mCamera.transform.position = hitInfo.point;
		}
		else
		{
			
			var clampedDiff = Vector3.ClampMagnitude(potentialCameraPos - _mCamera.transform.position, 0.5f) ;
			_mCamera.transform.position += clampedDiff;

		}
	}

	public void SetCameraState(CameraStates newState)
	{
		_mCameraState = newState;
	}
	

    void FixedUpdate()
    {
        var moveValue = _mMoveAction.ReadValue<Vector2>();
        if (_mControllerManager.IsBusy())
        {
	        moveValue = new Vector2(0, 0);
        }
        
	    HandleRotation();
        
        var right = moveValue.x;

        switch (right)
        {
	        case 1:
		        _mAnimator.CrossFade("Sidestep-Right", 0.01f);
		        return;
	        case -1:
		        _mAnimator.CrossFade("Sidestep-Left", 0.01f);
		        return;
        }
        
        HandleForwardMovement(moveValue);
    }

    void HandleForwardMovement(Vector2 moveValue)
    {
	    var newVelocity = _mPreviousVelocity;
	    float forward = moveValue.y;

	    if (_mCharacterController.isGrounded)
	    {
		    newVelocity += transform.forward * forward * Time.fixedDeltaTime * kForwardAcceleration;
		    //newVelocity += transform.right * right * Time.fixedDeltaTime * k_horizontalAcceleration;
	    }
	    else
	    { 
		    newVelocity += new Vector3(0, -kGravity  * Time.fixedDeltaTime , 0);

		    if (newVelocity.y > kTerminalVelocity)
		    {
			    newVelocity.Set(newVelocity.x, kTerminalVelocity, newVelocity.z);
		    }
		    if (newVelocity.y < -kTerminalVelocity)
		    {
			    newVelocity.Set(newVelocity.x, -kTerminalVelocity, newVelocity.z);
		    }
			
		    _mCharacterController.Move(new Vector3(0, newVelocity.y, 0));
	    }
	    Vector3 horizontalVelocity = new Vector3(newVelocity.x, 0, newVelocity.z);
	    Vector3 clampedVelocity = Vector3.ClampMagnitude(horizontalVelocity, kMaximumSpeed);
	
	    float speedPercent = clampedVelocity.magnitude / kMaximumSpeed;
		
	    if (moveValue.magnitude < 0.1)
	    {
		    _mAnimator.SetFloat("speedPercent",0);
	    }
	    else{
		
		    _mAnimator.SetFloat("speedPercent",speedPercent);
		    newVelocity.Set(clampedVelocity.x, newVelocity.y, clampedVelocity.z);
		    _mPreviousVelocity = newVelocity;
	    }
    }

}