using UnityEngine;

public class NPCController : MonoBehaviour
{
    private GameObject _mGoal;

    private UnityEngine.AI.NavMeshAgent _mAgent;
    private Animator _mAnimator;

    private void Start()
    {
        _mAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _mAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!_mGoal || !_mAgent.isOnNavMesh) return;
        _mAgent.destination = _mGoal.transform.position;
        _mAnimator.SetFloat("speedPercent", _mAgent.velocity.magnitude);

    }
}