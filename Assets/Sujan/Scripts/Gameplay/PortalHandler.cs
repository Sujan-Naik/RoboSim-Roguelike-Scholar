using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameplayManager.Instance().LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}