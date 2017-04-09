using UnityEngine;
using UnityEngine.AI;

public class Navigator : MonoBehaviour {

    private NavMeshAgent _agent;
    private Targeter _targeter;
    private Animator _animator;

	// Called before Start()
	void Awake ()
    {
        _agent = GetComponent<NavMeshAgent>();
        _targeter = GetComponent<Targeter>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!_agent.isStopped)
            GetComponent<Animator>().SetFloat("Distance", _agent.remainingDistance);
    }
	
	public void NavigateTo (Vector3 destination)
    {
        _agent.SetDestination(destination);
        _agent.isStopped = false;
        _targeter.target = null;
        _animator.SetBool("Attack", false);
        Network.Move(destination);
    }

    public void ResetDestination()
    {
        _agent.SetDestination(transform.position);
    }

    public void StopNavigate()
    {
        if (!_agent.isStopped)
        {
            _agent.isStopped = true;
            GetComponent<Animator>().SetFloat("Distance", 0);
            Network.IddlePosition(gameObject.transform.position);
        }
    }
}
