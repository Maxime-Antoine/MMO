using UnityEngine;

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
        GetComponent<Animator>().SetFloat("Distance", _agent.remainingDistance);
    }
	
	public void NavigateTo (Vector3 destination)
    {
        _agent.SetDestination(destination);
        _targeter.target = null;
        _animator.SetBool("Attack", false);
	}

    public void ResetDestination()
    {
        _agent.SetDestination(transform.position);
    }
}
