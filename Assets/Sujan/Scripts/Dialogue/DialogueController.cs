using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public AbstractCommunicationHandler kDialogueHandler; // the character currently being spoken to
    public GameObject kDialogueObject;
    public GameObject kInputObject;
    public TMP_Text kTextDialogue;
    public TMP_InputField kTextInput;

    private InputAction _mTextExit;

    private float _mLastFocusToggleTime;
    private const float FOCUS_TOGGLE_COOLDOWN = 1f;
    
    private ControllerManager _mControllerManager;
    private bool _mIsFocused = false;

    public bool IsFocused => kDialogueHandler != null;


    private void Start()
    {
        kTextInput.onSubmit.AddListener(SendMessage);
        _mTextExit = InputSystem.actions.FindAction("Text");
		
		_mControllerManager = GetComponent<ControllerManager>();
        _mLastFocusToggleTime = Time.realtimeSinceStartup;
        Unfocus();
    }

    void Update()
    {
        if (kDialogueHandler == null) return;
        if (_mTextExit.IsPressed() && Time.realtimeSinceStartup - _mLastFocusToggleTime > FOCUS_TOGGLE_COOLDOWN)
        {
            Unfocus();
        }
        else if (kDialogueHandler.CanPlayerSpeak())
        {
            kInputObject.GetComponent<Image>().color = Color.white;
        }
        else
        {
            kInputObject.GetComponent<Image>().color = Color.red;
        }
    }

    public void Unfocus()
    {

        kTextDialogue.text = "";
        kTextInput.text = "";
        kInputObject.SetActive(false);
        kDialogueObject.SetActive(false);
		_mControllerManager.SetBusy(false);
        kTextInput.DeactivateInputField();
        Cursor.lockState = CursorLockMode.Locked;
		kDialogueHandler = null;
        
        Time.timeScale = 1f;
    }

    public bool IsBusy()
    {
        return kDialogueHandler != null;
    }

    public void Focus(AbstractCommunicationHandler newDialogueHandler)
    {
        kInputObject.SetActive(true);
        kDialogueObject.SetActive(true);

		_mControllerManager.SetBusy(true);

        _mLastFocusToggleTime = Time.time;

		kDialogueHandler = newDialogueHandler;
        kTextInput.ActivateInputField();
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void SendReply(string reply, AbstractCommunicationHandler handler)
    {
        kTextDialogue.text = reply;
        this.kDialogueHandler = handler;
    }
    
    private void SendMessage(string message)
    {
        if (kDialogueHandler)
        {
            kDialogueHandler.SendCommunicationMessage(message);
        	kTextInput.ActivateInputField();
        }
    }
    
    
}