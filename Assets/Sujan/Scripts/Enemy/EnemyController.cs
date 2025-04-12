using UnityEngine;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    private GameObject _mGoal;
    private DialogueController _mGoalDialogue;

    private UnityEngine.AI.NavMeshAgent _mAgent;
    private Animator _mAnimator;
    private float _mLastAttackTime;
    private const float ATTACK_COOLDOWN = 3f,LONG_RANGE = 20f, LONG_RANGE_DISTANCE_TO_RECALCULATE = 5f, ATTACK_RANGE = 2f;
    
    public float kDamage = 20f;
    private Vector3 _mCurrentGoalPos;


    private void Start()
    { 
        _mGoal = GameObject.FindGameObjectWithTag("Player");
        _mCurrentGoalPos = _mGoal.transform.position;
        _mAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		_mAnimator = GetComponent<Animator>();
        _mLastAttackTime = Time.time;
    }

    private void Update()
    {
    if (_mAgent.isOnNavMesh)
    {
        _mAgent.isStopped = false;
        if (Vector3.Distance(transform.position, _mGoal.transform.position) > LONG_RANGE)
        {
            if (Vector3.Distance(_mCurrentGoalPos, _mGoal.transform.position) > LONG_RANGE_DISTANCE_TO_RECALCULATE)
            {
                _mAgent.SetDestination(_mGoal.transform.position);
            }
        }
        else
        {
            _mAgent.SetDestination(_mGoal.transform.position);

        }
        
        
        // agent.destination = goal.transform.position;
        _mAnimator.SetFloat("speedPercent", _mAgent.velocity.magnitude);
    }
    var distance = Vector3.Distance(transform.position, _mGoal.transform.position);
    if (!(distance <= ATTACK_RANGE * transform.lossyScale.y)) return;
    if (!(Time.time - _mLastAttackTime > ATTACK_COOLDOWN)) return;
    _mAnimator.CrossFade("Right-Punch", 0.1f);
    _mGoal.GetComponent<HealthController>().Damage(kDamage);
    _mLastAttackTime = Time.time;
    }

    
}
