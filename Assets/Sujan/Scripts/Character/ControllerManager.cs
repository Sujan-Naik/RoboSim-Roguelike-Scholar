using UnityEngine;
public class ControllerManager : MonoBehaviour
{ 
    
    private bool _mBusy = false;
    
    public void SetBusy(bool newBusy)
    {
        this._mBusy = newBusy;
    }
    
    public bool IsBusy()
    {
        return _mBusy;
    }
    
}