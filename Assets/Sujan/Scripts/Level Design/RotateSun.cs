using UnityEngine;

public class RotateSun : MonoBehaviour
{

    public float dayLength;

    private void Update()
    {
        transform.Rotate(Time.deltaTime/dayLength  * 360, 0, 0);
    }
}
