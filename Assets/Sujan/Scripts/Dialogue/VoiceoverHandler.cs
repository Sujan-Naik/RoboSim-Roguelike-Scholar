using UnityEngine;
using LLMUnity;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Serialization;


public class VoiceoverHandler : MonoBehaviour
{
	
	private bool _mIsAcceptingQuestions, _mHasStartedSpeaking, _mHasParsedMessage;

	public DialogueAsset kCurrentDialogue;
	
    public TTS kSpeech;
    public GameObject kPlayer;
    
    private DialogueController _mDialogueController;
    private void Start()
    {
	    _mDialogueController = kPlayer.GetComponent<DialogueController>();
    }

    private void Update()
    {
	  if (!_mHasStartedSpeaking)
	  {
		  StartCoroutine(MoveThroughDialogue(kCurrentDialogue));

		  _mHasStartedSpeaking = true;
	  }
	  else
	  {
		  if (!_mHasParsedMessage) return;
		  if (!kSpeech.IsPlaying()) return;
		  if (_mDialogueController.IsBusy())
		  {
			  kSpeech.Pause();
		  }
		  else
		  {
			  kSpeech.Unpause();
		  }
	  }

	  
	}

    private void HandleReply(string message)
    {
	    if (_mHasParsedMessage) return;
	    kSpeech.Speak(message);
	    _mHasParsedMessage = true;

    }

	private IEnumerator MoveThroughDialogue(DialogueAsset dialogueAsset)
	{
		_mIsAcceptingQuestions = false;
    	foreach (var line in dialogueAsset.dialogue)
	    {
		    _mHasParsedMessage = false;
		    HandleReply(line);
      
			yield return new WaitUntil(() => !kSpeech.IsPlaying() && _mHasParsedMessage && !_mDialogueController.IsFocused);
		    
	    }
	}
	
}
