using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TitleCameraController : MonoBehaviour
{
    // Constants for look changes
    public float kMaximumPitchDeviation = 40f; // How much the player can look up/down from resting position
    public float kMaxPitchTurnSpeed = 2.0f; // The fastest a player head pitch can change
    public float kPitchTurnAcceleration = 0.2f; // How fast the player can look up/down 

    private float _mPitchSpeed = 0.0f;
	
    private float _mOldPitch;
    
    private InputAction _mlookAction;

    public float kMaxYawTurnSpeed = 2.0f; // The fastest a player head yaw can change
    public float kYawTurnAcceleration = 0.5f; // How much the player can look right/left from resting position
    
    private float _mOldYaw, _mYawSpeed;
    
    public GameObject kHead;
    private Camera _mCamera;


    private bool _mReady, _mIsLookingAtCanvas = true;

    private Quaternion _mOriginalHeadRotation;

    private float _mLastLookAtScreenTime;
    private const float LOOK_AT_SCREEN_COOLDOWN = 1f;
    private IEnumerator Start()
    {

	    yield return GameplayManager.Instance().IsReady();
	    _mReady = true;
	    _mlookAction = InputSystem.actions.FindAction("Look");
	    _mOriginalHeadRotation = kHead.transform.rotation;

	    _mPitchSpeed = 0;
        _mYawSpeed = 0;
        _mOldPitch = kHead.transform.localEulerAngles.x;
        _mOldYaw = kHead.transform.localEulerAngles.y;
        _mCamera = Camera.main;
        _mLastLookAtScreenTime = Time.time;
    }
    
    private void HandleRotation()
	{
		var lookValue = _mlookAction.ReadValue<Vector2>();

        var yawRight = lookValue.x;
		if (Mathf.Abs(yawRight) < 0.01)
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
		
        var pitchUp = lookValue.y;
		if (Mathf.Abs(pitchUp) < 0.01)
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

		_mOldPitch = newPitch;
		_mOldYaw = newYaw;
        kHead.transform.localEulerAngles = new Vector3(newPitch, newYaw, 0);
	}

	private bool ShouldLookAtCanvas()
	{
		RaycastHit hitInfo;
		var mask = LayerMask.GetMask("UI");

		return Physics.Raycast(_mCamera.transform.position, _mCamera.transform.forward, out hitInfo,
			1000, mask, QueryTriggerInteraction.Collide);
	}

	private bool ShouldFreeLook()
	{
		var screenPointToRay = _mCamera.ScreenPointToRay(Input.mousePosition).direction;
		var cursorDirection = (new Vector3((screenPointToRay.x-0.5f)*2f, screenPointToRay.y * 1f, 0) + _mCamera.transform.forward).normalized;
		
		RaycastHit hitInfo;
		var mask = LayerMask.GetMask("UI");

		// return !Physics.Raycast(_mCamera.transform.position + _mCamera.transform.forward/4, cursorDirection, out hitInfo,
		// 	1000, mask, QueryTriggerInteraction.Collide);
		Debug.DrawRay(_mCamera.ScreenPointToRay(Input.mousePosition).GetPoint(0), _mCamera.ScreenPointToRay(Input.mousePosition).GetPoint(100));
		return !Physics.Raycast(_mCamera.ScreenPointToRay(Input.mousePosition), out hitInfo,
			1000, mask, QueryTriggerInteraction.Collide);
	}
	private void Update()
	{
		if (!_mReady) return;
			
		if (_mIsLookingAtCanvas)
		{
			if (!ShouldFreeLook()) return;
			
			kHead.transform.LookAt(_mCamera.ScreenPointToRay(Input.mousePosition).GetPoint(100));
			_mOldPitch = kHead.transform.localEulerAngles.x;
			_mOldYaw = kHead.transform.localEulerAngles.y;
			

			Cursor.lockState = CursorLockMode.Locked;
			_mIsLookingAtCanvas = false;
		}
		else
		{
			HandleRotation();

			if (Time.time - _mLastLookAtScreenTime < LOOK_AT_SCREEN_COOLDOWN || !ShouldLookAtCanvas()) return;
			
			_mLastLookAtScreenTime = Time.time;
			Cursor.lockState = CursorLockMode.None;
			_mIsLookingAtCanvas = true;
			kHead.transform.rotation = _mOriginalHeadRotation;



		}
	}
    
}