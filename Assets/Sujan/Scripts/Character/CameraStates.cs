using UnityEngine;
public class CameraStates
{
    public static CameraStates DEFAULT = new CameraStates(0f, 2f, -3f);
    public static CameraStates ABILITY = new CameraStates(0f, 1.7f, 0f);
    public static CameraStates RANGED = new CameraStates(0f, 1.7f, 0f);
    
    float _mRight;
    float _mUp;
    float _mForward;
    private CameraStates(float mRight, float mUp, float mForward)
    {
        this._mRight = mRight;
        this._mUp = mUp;
        this._mForward = mForward;
    }

    public Vector3 GetTransform(Transform transform)
    {
        return transform.position +
               _mForward * transform.forward * transform.lossyScale.z +
               _mUp * Vector3.up * transform.lossyScale.y +
               _mRight * transform.right * transform.lossyScale.x;

    }


}