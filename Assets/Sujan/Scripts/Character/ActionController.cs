
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    
    private InputAction _mJumpAction, _mMeleeAttackAction, _mRangedAttackAction;

    public float kJumpRecoveryTime = 1f;
    
    public RawImage kCrosshair;
    public Image kRangedImage;
    public Image kMeleeImage;
    
    private GameObject melee, ranged, rangedOutput, rangedProjectile, currentArrow;

    private bool _mHoldingMelee, _mHoldingRanged;
    
    private CharacterController _mCharacterController;
    private PlayerController _mPlayerController;

    private Animator _mAnimator;
    
    public float kMeleeRecoveryTime = 1.0f;
    public float _kRangedRecoveryTime = 0.6f;
    private ControllerManager _mControllerManager;

    private EquipmentHandler _mEquipmentHandler;
    void Start()
    {
        _mControllerManager = GetComponent<ControllerManager>();

        _mCharacterController = GetComponent<CharacterController>();
        _mPlayerController = GetComponent<PlayerController>();
        _mEquipmentHandler = GetComponent<EquipmentHandler>();
        _mAnimator = GetComponent<Animator>();

        _mJumpAction = InputSystem.actions.FindAction("Jump");

        _mMeleeAttackAction = InputSystem.actions.FindAction("Melee");
        _mRangedAttackAction = InputSystem.actions.FindAction("Ranged");
    }

    private void RefreshEquipment()
    {
        melee = _mEquipmentHandler.GetMelee();
        ranged = _mEquipmentHandler.GetRanged();
        rangedOutput = _mEquipmentHandler.GetRangedOutput();
        rangedProjectile = _mEquipmentHandler.GetRangedProjectile();
    }
    


    private void HandleJump()
    {
        _mAnimator.CrossFade("Jump", 0.1f);
    }

    private void HandleJumpRecovery()
    {
        _mControllerManager.SetBusy(false);
    }

    private const float BACKSWING_ANGLE = 30f;
    
    private void HandleMelee()
    {
        melee.GetComponent<DamageHandler>().enabled = true;
        melee.GetComponentInParent<MeleeWeaponFollow>().kPitch += BACKSWING_ANGLE;
        HoldMelee();
        EffectSingleton.Instance().PlayMeleeEffect(melee.transform);
        _mAnimator.CrossFade("Shomenuchi", 0.1f);
        
        kMeleeImage.CrossFadeAlpha(0.2f, 0.1f, false);

        
    }

    private void HandleMeleeRecovery()
    {
        melee.GetComponentInParent<MeleeWeaponFollow>().kPitch -= BACKSWING_ANGLE;

        kMeleeImage.CrossFadeAlpha(1f, 0.1f, false);

        melee.GetComponent<DamageHandler>().enabled = false;
        _mControllerManager.SetBusy(false);
    }

    private void HandleRanged()
    {
        kRangedImage.CrossFadeAlpha(0.2f, 0.1f, false);
        HoldRanged();
        EffectSingleton.Instance().PlayRangedEffect(ranged.transform);
        melee.gameObject.SetActive(false);
        _mPlayerController.SetCameraState(CameraStates.RANGED);
        kCrosshair.enabled = true;
    }

    private void LeaveBowView()
    {
        if (_mRangedAttackAction.IsPressed()) return;
        _mPlayerController.SetCameraState(CameraStates.DEFAULT);
        melee.gameObject.SetActive(true);


    }

    private void HandleRangedRecovery()
    {
        if (_mEquipmentHandler.GetRangedType() == RangedTypes.BOW)
        {
            if (!_mRangedAttackAction.IsPressed())
            {

                _mAnimator.SetBool("holdingRanged",false);

            
                _mControllerManager.SetBusy(false);
                
                if (!currentArrow)
                {
                    currentArrow  = GameObject.Instantiate(rangedProjectile,
                        rangedOutput.transform.position,
                        Quaternion.AngleAxis(90, transform.right) * Quaternion.AngleAxis(180, Vector3.up));
                    currentArrow.SetActive(true);
                }
                currentArrow.GetComponent<Rigidbody>().useGravity = true;

                Rigidbody rigidbody = currentArrow.GetComponent<Rigidbody>();
                rigidbody.AddForce(Camera.main.transform.forward * 20, ForceMode.VelocityChange);

                kCrosshair.enabled = false;
                kRangedImage.CrossFadeAlpha(1f, 0.1f, false);
                Invoke("LeaveBowView", 1f);
                currentArrow = null;
            }
            else
            {
                if (!currentArrow)
                {
                    currentArrow  = Instantiate(rangedProjectile,
                        rangedOutput.transform.position,
                        Quaternion.AngleAxis(90, transform.right) * Quaternion.AngleAxis(180, Vector3.up));
                    currentArrow.GetComponent<Rigidbody>().useGravity = false;
                    currentArrow.SetActive(true);
                }
                currentArrow.transform.rotation = Quaternion.AngleAxis(90 + Camera.main.transform.rotation.eulerAngles.x, transform.right) * Quaternion.AngleAxis(180, Vector3.up);
                currentArrow.transform.position = rangedOutput.transform.position - Camera.main.transform.forward / 4;

                _mAnimator.CrossFade("Bow-Arrow-Hold", 0.1f);

                Invoke("HandleRangedRecovery", 0.001f);
            }
        }
        else if (_mEquipmentHandler.GetRangedType() == RangedTypes.TWO_HANDED)
        {
            if (!_mRangedAttackAction.IsPressed())
            {
                melee.gameObject.SetActive(true);

                _mAnimator.SetBool("holdingRanged",false);
                
            
                _mControllerManager.SetBusy(false);
                _mPlayerController.SetCameraState(CameraStates.DEFAULT);
            }
            else
            {
                EffectSingleton.Instance().PlayGunshotEffect(rangedOutput.transform);
                GameObject newRangedProjectile  = GameObject.Instantiate(rangedProjectile,
                    rangedOutput.transform.position,
                     Quaternion.AngleAxis(180, Vector3.up));
                newRangedProjectile.SetActive(true);
                Rigidbody rigidbody = newRangedProjectile.GetComponent<Rigidbody>();
                rigidbody.AddForce(transform.forward * 30, ForceMode.VelocityChange);
                Invoke("HandleRangedRecovery", 0.001f);
            }
        } else if (_mEquipmentHandler.GetRangedType() == RangedTypes.ONE_HANDED)
        {
            if (!_mRangedAttackAction.IsPressed())
            {
                melee.gameObject.SetActive(true);

                _mAnimator.SetBool("holdingRanged",false);
                
            
                _mControllerManager.SetBusy(false);
                _mPlayerController.SetCameraState(CameraStates.DEFAULT);
            }
            else
            {
                EffectSingleton.Instance().PlayGunshotEffect(rangedOutput.transform);

                GameObject newRangedProjectile  = GameObject.Instantiate(rangedProjectile,
                    rangedOutput.transform.position,
                    Quaternion.AngleAxis(180, Vector3.up));
                newRangedProjectile.SetActive(true);
                Rigidbody rigidbody = newRangedProjectile.GetComponent<Rigidbody>();
                rigidbody.AddForce(transform.forward * 30, ForceMode.VelocityChange);
                Invoke("HandleRangedRecovery", 0.001f);
            }
        }
    }
	
    private IEnumerator PerformLater(float time, ActionCallback callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    public delegate void BlockingAction();
    public delegate void ActionCallback();

    public void DoBlockingAction(InputAction input, BlockingAction action, ActionCallback callback, float time)
    {
        if (!_mControllerManager.IsBusy() && input.IsPressed())
        {	
            _mControllerManager.SetBusy(true);
            action();
            StartCoroutine(PerformLater(time, callback));
        }
        
    }
    void Update()
    {
        RefreshEquipment();
        if (_mControllerManager.IsBusy()) return;
        if (melee != null)
        {
            DoBlockingAction(_mMeleeAttackAction, HandleMelee, HandleMeleeRecovery, kMeleeRecoveryTime);

        }

        if (ranged != null)
        {
            DoBlockingAction(_mRangedAttackAction, HandleRanged, HandleRangedRecovery, _kRangedRecoveryTime);

        }

        if (_mCharacterController.isGrounded)
        {
            DoBlockingAction(_mJumpAction, HandleJump, HandleJumpRecovery, kJumpRecoveryTime);

        }
    }

    private void HoldMelee()
    {
        _mHoldingMelee = true;
        melee.gameObject.SetActive(true);

        if (!_mHoldingRanged) return;
        ranged.SetActive(false);
        _mHoldingRanged = false;
    }
    
    private void HoldRanged()
    {
        if (_mHoldingMelee)
        {
            melee.gameObject.SetActive(false);

            _mHoldingMelee = false;
        }

        _mAnimator.SetBool("holdingRanged",true);

        if (_mEquipmentHandler.GetRangedType() == RangedTypes.BOW)
        {
            if (!_mHoldingRanged)
            {
                _mAnimator.CrossFade("Bow-Draw", 0.1f);
            }

            _mAnimator.CrossFade("Bow-Arrow-Draw", 0.1f);
        } else if (_mEquipmentHandler.GetRangedType() == RangedTypes.TWO_HANDED)
        {
            _mAnimator.CrossFade("Two-Handed-Gun-Hold", 0.1f);
        } else if (_mEquipmentHandler.GetRangedType() == RangedTypes.ONE_HANDED)
        {
            _mAnimator.CrossFade("One-Handed-Gun-Hold", 0.1f);

        }

        _mHoldingRanged = true;
        ranged.SetActive(true);
       
    }
}