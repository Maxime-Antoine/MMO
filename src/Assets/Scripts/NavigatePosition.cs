using UnityEngine;
using System.Collections;

public class NavigatePosition : MonoBehaviour {

    private NavMeshAgent _agent;

	// Use this for initialization
	void Start ()
    {
        _agent = GetComponent<NavMeshAgent>();
	}

    // Update is called once per frame
    void Update ()
    {
        GetComponent<Animator>().SetFloat("Distance", _agent.remainingDistance);
    }
	
	public void NavigateTo (Vector3 destination)
    {
        _agent.SetDestination(destination);
	}
}
