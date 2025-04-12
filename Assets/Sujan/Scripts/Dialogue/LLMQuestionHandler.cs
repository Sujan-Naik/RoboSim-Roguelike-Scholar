using UnityEngine;
using LLMUnity;
using UnityEngine.InputSystem;
using System.Collections;
using Seagull.Interior_01;
using Unity.VisualScripting;
using UnityEngine.Serialization;


public class LLMQuestionHandler : AbstractCommunicationHandler
{
    
    public LLMCharacter kLlmCharacter;
    public DialogueController kDialogueController;

    private GameObject _mPlayer;

    private bool _mHasPromptedQuestion, _mIsAcceptingResponse, _mHasAskedQuestion;
    
    public QuestionBank questionBank;
    public GameObject[] rewards;
    
    private bool _mHasAnswered = false;
    
    private void Start()
    { 
        _mPlayer = GameObject.FindWithTag("Player");
        kLlmCharacter.seed = Randomisation.GetSeed();
    }

    public override void SendCommunicationMessage(string message)
    {
	    if (!_mIsAcceptingResponse)
	    {
		    return;
	    }
	    
	    _mIsAcceptingResponse = false;
	    
	    _ = kLlmCharacter.Chat("Is this incorrect. Explain why if so: " + message,HandleReplyPartial, HandleReply);
	    kDialogueController.SendReply("Evaluating answer and providing feedback, please wait...", this);

    }

    public override bool CanPlayerSpeak()
    {
	    return _mIsAcceptingResponse;
    }

    private string _mReply;

    private void HandleReplyPartial(string reply)
    {
	    this._mReply = reply;
    }
    private void HandleReply(){
	    if ( _mHasAnswered)
	    {
		    return;
	    }
	    
	    _mHasAnswered = true;
	    if (!_mReply.ContainsInsensitive("Incorrect") || !_mReply.ContainsInsensitive("Not Correct"))
	    {
		    foreach (var reward in rewards)
		    {
			    GameObject.Instantiate(reward, _mPlayer.transform.position + Vector3.up * transform.lossyScale.y/2,
				    Quaternion.identity).SetActive(true);
		    }
		    EffectSingleton.Instance().PlaySucceedEffect(_mPlayer.transform);
	    }
	    else
	    {
		    EffectSingleton.Instance().PlayFailEffect(_mPlayer.transform);
	    }

        kDialogueController.SendReply(_mReply + ". Press escape on your keyboard to exit.", this);
        Invoke("Unfocus", 15f);
        Destroy(gameObject, 20f);
    }

    private void Unfocus()
    {
	    _mPlayer.GetComponent<DialogueController>().Unfocus();
    }

    private void HandleUnfinishedReply(string question)
    {
	    kDialogueController.SendReply(question, this);
    }

    private void FinishAskingQuestion()
    {
	    _mIsAcceptingResponse = true;
    }
   
    private void OnTriggerEnter(Collider other)
    {
	    
	    if (_mHasPromptedQuestion || !other.CompareTag("Player"))
	    {
		    return;
	    }

	    _mHasPromptedQuestion = true;
	    kDialogueController = _mPlayer.GetComponent<DialogueController>();
	    kDialogueController.Focus(this);
	    var question = Randomisation.GetRandom(questionBank.questions);
	    kDialogueController.SendReply("Generating question, please wait...", this);
	    
	    _ = kLlmCharacter.Chat("Give me a question loosely related to " + question + " DO NOT INCLUDE THE ANSWER.", HandleUnfinishedReply, FinishAskingQuestion);
    }
}
