using UnityEngine;

public class Navigator : MonoBehaviour {

    private NavMeshAgent _agent;
    private Follower _follower;

	// Called before Start()
	void Awake ()
    {
        _agent = GetComponent<NavMeshAgent>();
        _follower = GetComponent<Follower>();

    }

    // Update is called once per frame
    void Update ()
    {
        GetComponent<Animator>().SetFloat("Distance", _agent.remainingDistance);
    }
	
	public void NavigateTo (Vector3 destination)
    {
        _agent.SetDestination(destination);
        _follower.target = null;
	}
}
