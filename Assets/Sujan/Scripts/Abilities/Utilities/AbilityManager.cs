using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    
    private InputAction _mAbility_1, _mAbility_2, _mAbility_3, _mAbility_4;
    
    private List<AbilitySlot> _mAbilitySlots = new ();


    public Image[] kSlotImages;

    public Sprite[] kSprites;
    
    
    private Animator _mAnimator;


    private List<CoreAbility> _mActiveAbilities = new ();

    public GameObject kHand;
    public PlayerController kPlayerController;
    
    private ControllerManager _mControllerManager;

    private const int SLOTS = 4;

    public Sprite GetSprite()
    {
        return kSprites[new System.Random().Next(0, kSprites.Length)];
    }
    
    public void SetAbility(GameObject ability)
    {
        var hasSet = false;
        foreach (var slot in _mAbilitySlots)
        {
            if (!slot.IsNull() || hasSet) continue;
            slot.SetAbility(ability);
            hasSet = true;
        }

        if (!hasSet)
        {
            _mAbilitySlots[new System.Random().Next(0, _mAbilitySlots.Count)].SetAbility(ability);
        }
    }
    
    private void Start()
    {
        _mControllerManager = GetComponent<ControllerManager>();
        
        for (var i = 1; i <= SLOTS; i++)
        {
            var slot = new AbilitySlot(this, i, null, kSlotImages[i-1]);
            _mAbilitySlots.Add(slot);
        }
        
        _mAnimator = GetComponent<Animator>();
        kPlayerController = GetComponent<PlayerController>();

    }

    private void Update()
    {
        
        if (!_mControllerManager.IsBusy())
        {
            foreach (var abilitySlot in _mAbilitySlots)
            {
                abilitySlot.Tick();
            }
        }

        foreach (var abilitySlot in _mAbilitySlots)
        {
            abilitySlot.TickRemove();
        }

        foreach (var abil in _mActiveAbilities)
        {
            if (abil.GetAbilityStatus() == AbilityStatus.INACTIVE)
            {
                abil.SetAbilityStatus(AbilityStatus.ACTIVE);
            }
            abil.AbilityUpdate();
        }
        _mActiveAbilities.RemoveAll(abil => abil.GetAbilityStatus() == AbilityStatus.FINISHED);
    }

    public void activateAbility(GameObject abilityPrefab)
    {
        CoreAbility ability = abilityPrefab.GetComponent<CoreAbility>();
        EffectSingleton.Instance().PlayAbilityEffect(kHand.transform);
        
        _mControllerManager.SetBusy(true);
        kPlayerController.SetCameraState(CameraStates.ABILITY);
        _mAnimator.CrossFade("Right Punch", 0.1f);
        StartCoroutine(AbilityInstantiation(ability, abilityPrefab));
    }

    private IEnumerator AbilityInstantiation(CoreAbility ability, GameObject abilityPrefab)
    {
        yield return new WaitForSeconds(1f);
        _mControllerManager.SetBusy(false);
        kPlayerController.SetCameraState(CameraStates.DEFAULT);

        CoreAbility newAbility = abilityPrefab.AddComponent(ability.GetType()) as CoreAbility;
        abilityPrefab.transform.position = kHand.transform.position;
        abilityPrefab.transform.rotation = Camera.main.transform.rotation;
    
        _mActiveAbilities.Add(newAbility);
    }
}