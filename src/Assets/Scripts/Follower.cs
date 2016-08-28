using UnityEngine;

public class Follower : MonoBehaviour
{
    public Targeter targeter;

    public float scanFrequency = 0.5f;
    public float stopFollowDistance = 1;

    private float _lastScanTime = 0;
    private NavMeshAgent _agent;

    // Use this for initialization
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        targeter = GetComponent<Targeter>();
    }

    
    // Update is called once per frame
    void Update()
    {
        if (IsReadyToScan() && !targeter.IsInRange(stopFollowDistance))
        {
            Debug.Log("Scanning nav path");
            _agent.SetDestination(targeter.target.position);
        }
    }

    #region Private Methods

    private bool IsReadyToScan()
    {
        return Time.time - _lastScanTime > scanFrequency && targeter.target;
    }

    #endregion
}
