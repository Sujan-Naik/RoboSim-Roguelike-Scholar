using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class AbilitySlot
{
    
    private readonly AbilityManager _mAbilityManager;
    private int _mSlot; // 1 - 4

    private readonly Image _mSlotImage;

    private GameObject _mAbility;

    private bool _mIsAvailable = true;

    private readonly InputAction _mInput;

    private CoreAbility _mCurrentAbility;
    
    public AbilitySlot(AbilityManager mAbilityManager, int mSlot, GameObject mAbility, Image mSlotImage)
    {
        this._mAbilityManager = mAbilityManager;
        this._mSlot = mSlot;
        this._mAbility = mAbility;
        this._mSlotImage = mSlotImage;
        _mInput = InputSystem.actions.FindAction("Ability_" + mSlot);

    }

    public void Tick()
    {
        if (!_mAbility || !_mIsAvailable || !_mInput.IsPressed() ) return;
 
        FadeImage();
        _mAbilityManager.activateAbility(_mAbility);
        _mIsAvailable = false;
    }

    public void TickRemove()
    {
        if (!_mAbility) return;

        if (_mAbility.GetComponents<CoreAbility>()[_mAbility.GetComponents<CoreAbility>().Length - 1]
                .GetAbilityStatus() != AbilityStatus.FINISHED) return;
        
        _mIsAvailable = true;
        UnfadeImage();
    }

    public bool IsNull()
    {
        return _mAbility == null;
    }

    private void FadeImage()
    {
        _mSlotImage.CrossFadeAlpha(0.3f, 0.1f, false);

    }
    void UnfadeImage()
    {
        _mSlotImage.CrossFadeAlpha(1f, 0.1f, false);

    }

    public void SetAbility(GameObject ability)
    {
        this._mAbility = ability;
        _mSlotImage.sprite = _mAbilityManager.GetSprite();
    }
    
    
    
}