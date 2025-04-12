using UnityEngine;
using LLMUnity;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Serialization;


public class QuestionHandler : AbstractCommunicationHandler
{
    
    public DialogueController kDialogueController;

    private GameObject _mPlayer;

    private const float REQUIRED_DISTANCE = 3;

    private bool _mHasAskedQuestion, _mIsAcceptingQuestions;

    public string kQuestions;
    public string kCorrectInput;
    public GameObject[] kRewards;

    private void Start()
    { 
        _mPlayer = GameObject.FindWithTag("Player");

    }

    public override void SendCommunicationMessage(string message)
    {
	    if (message == kCorrectInput)
	    {
		    foreach (var reward in kRewards)
		    {
			    Instantiate(reward, transform.position + Vector3.up / 2 + Randomisation.GetRandomFlatVector3() / 2,
				    Quaternion.identity).SetActive(true);
		    }
	    } 
	    
	    kDialogueController.Unfocus();
	    kDialogueController = null;
    }

    void SendCustomMessage(string message)
    {
	    kDialogueController.SendReply(message, this);
    }
    
    
    public override bool CanPlayerSpeak()
    {
	    return _mIsAcceptingQuestions;
    }

    private void Update()
    {
	    if (_mHasAskedQuestion)
	    {
		    return;
	    }
	    if (!(Vector3.Distance(transform.position, _mPlayer.transform.position) < REQUIRED_DISTANCE)) return;
	    

	    _mHasAskedQuestion = true;
	    kDialogueController = _mPlayer.GetComponent<DialogueController>();
	    kDialogueController.Focus(this);
		SendCustomMessage(kQuestions);
		_mIsAcceptingQuestions = true;

    }
    
    
	
	
	
}
