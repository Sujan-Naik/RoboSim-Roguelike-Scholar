using UnityEngine;
using UnityEngine.Serialization;

public abstract class Holdable : MonoBehaviour
{
    protected GameObject _mHand;

    protected GameObject _mPlayer;
    
    public float kPitch;
    public float kYaw;
    
    protected bool _mIsHolding = false;

    public abstract void SetHolding(bool holding);

    protected void Start()
    {

        _mPlayer = GameObject.FindWithTag("Player");
    }

    protected void Update()
    {
        if (!_mIsHolding) return;
        
        transform.rotation = Quaternion.Euler(new Vector3(0, 270 + _mPlayer.transform.eulerAngles.y + kYaw, -Camera.main.transform.eulerAngles.x + kPitch ));
        
        transform.position = _mHand.transform.position;

    }
}