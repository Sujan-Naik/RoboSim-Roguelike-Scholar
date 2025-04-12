using System;
using UnityEngine;
using LLMUnity;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.Serialization;


public class DialogueHandler : AbstractCommunicationHandler
{
	
	private bool _mIsAcceptingQuestions;

    public TTS speech;
    public DialogueAsset kCurrentDialogue;

    private bool _mHasStartedSpeaking, _mHasQueriedSpeech, _mHasAskedQuestion;
    private DialogueController _mDialogueController;

    public LLMCharacter llmCharacter;

    private GameObject _mPlayer;

    private UnityEngine.AI.NavMeshAgent _mAgent;

    private readonly List<string> _mLinesToSpeak = new ();

    
    private void Start()
    {
	    speech = GetComponent<TTS>();
	    _mPlayer = GameObject.FindGameObjectWithTag("Player");
	    _mDialogueController = _mPlayer.GetComponent<DialogueController>();
	    _mAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
    
    public override bool CanPlayerSpeak()
    {
	    return _mIsAcceptingQuestions;
    }

    public override void SendCommunicationMessage(string message)
    {
	    if (!_mIsAcceptingQuestions || speech.IsPlaying() || _mHasAskedQuestion)
	    {
		    return;
	    }

	    _mHasAskedQuestion = true;
	    _mHasQueriedSpeech = false;
	    _ = llmCharacter.Chat( message, HandleUnfinishedReply, HandleReply, true);
	    _mDialogueController.SendReply("Generating response, please wait...", this);
	    
    }

    void Update()
    {
	    if (_mHasStartedSpeaking)
	    {
		    return;
	    }
	    _mAgent.SetDestination(_mPlayer.transform.position);
    }
    
    void OnTriggerEnter(Collider other)
    {
	    if (!other.CompareTag("Player"))
	    {
		    return;
	    }
	    if (_mHasStartedSpeaking)
	    {
		    return;
	    }
	    
	    Debug.Log("Started dialogue");
	    StartCoroutine(MoveThroughDialogue(kCurrentDialogue));
	    
	    Debug.Log("1");

	    // dialogueController = player.GetComponent<DialogueController>();
	    
	   _mDialogueController.Focus(this);
	   Time.timeScale = 0;

	    _mHasStartedSpeaking = true;
    }


    private void HandleUnfinishedReply(string message)
    {
	    _mDialogueController.SendReply(message, this);
	    // Inspired by https://stackoverflow.com/questions/4957226/split-text-into-sentences-in-c-sharp
	    
	    var sentences = Regex.Split(Regex.Replace(message.Trim(), @"\s{2,}", "") ,
			    @"(?<=[\.!\?])\s+"); // positive lookbehind (precedes with a final character)
	    var line = sentences.Last();

	    if (!Regex.IsMatch(line, @"(?=[\.!\?])")) return; // does not end with a final character
	    
	    if (_mLinesToSpeak.Contains(line)) return;
	    
	    if (_mHasQueriedSpeech)
	    {
		    if (!speech.IsPlaying())
		    {
			    _mHasQueriedSpeech = false;
		    }
	    }
	    if (!speech.IsPlaying() && !_mHasQueriedSpeech)
	    {
		    SpeakLine(line);
	    }
	    else
	    {
		    _mLinesToSpeak.Add(line);

	    }

    }


    void HandleReply()
    {
	
	    StartCoroutine("FinishSpeaking");
    }

    IEnumerator FinishSpeaking()
    {
	    SpeakLine(_mLinesToSpeak[0]);
	    _mLinesToSpeak.RemoveAt(0);
	    yield return _mLinesToSpeak.Count == 0;
	    _mHasAskedQuestion = false;
    }

    void SpeakLine(string message)
    {
	    if (_mHasQueriedSpeech) return;
	    speech.Speak(message);
	    _mHasQueriedSpeech = true;
	    if (_mHasAskedQuestion)
	    {
		    _mHasAskedQuestion = false;
	    }
    }
    
	private IEnumerator MoveThroughDialogue(DialogueAsset dialogueAsset)
	{

		_mIsAcceptingQuestions = false;
    	foreach (var line in dialogueAsset.dialogue)
	    {
		    _mHasQueriedSpeech = false;
			SpeakLine(line);
			_mDialogueController.SendReply(line, this);

			yield return new WaitUntil(() => !speech.IsPlaying() && _mHasQueriedSpeech);
	    }
	    _mIsAcceptingQuestions = true;
	}
	
}
