using UnityEngine;

public abstract class AbstractCommunicationHandler : MonoBehaviour
{
    public abstract void SendCommunicationMessage(string message);

    public abstract bool CanPlayerSpeak();

}
