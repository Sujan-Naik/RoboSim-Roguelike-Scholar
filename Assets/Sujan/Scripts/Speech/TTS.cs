using UnityEngine;

public class TTS : MonoBehaviour
{
    

    public RunJets textToSpeech;

    private bool _mHasEnteredText, _mHasBegunSpeaking, _mAudioPlaying = false;
    
    public void Speak(string inputText)
    {
        _mHasEnteredText = true;
        textToSpeech.inputText = inputText;
        textToSpeech.TextToSpeech();
        
    }
    
    public bool IsPlaying()
    {
        return _mHasEnteredText || textToSpeech.audioSource.isPlaying;
    }
    
    public void Pause()
    { 
        textToSpeech.audioSource.Pause();
    }
    
    
    public void Unpause()
    { 
        textToSpeech.audioSource.UnPause();
    }

    private void Update()
    {
        if (!_mHasEnteredText) return;
        if (textToSpeech.audioSource.isPlaying)
        {
            _mHasBegunSpeaking = true;
        }
        else if (_mHasBegunSpeaking)
        {
            _mHasBegunSpeaking = false;
            _mHasEnteredText = false;
        }
    }

 
}
